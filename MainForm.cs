using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.ComponentModel;
using Newtonsoft.Json;
using Ookii.Dialogs.Wpf;

namespace Steam_Games_Branch_Manager
{
    public partial class MainForm : Form
    {
        static MainForm()
        {
            LoadSaveData();
        }

        // Add fields for background workers
        private System.ComponentModel.BackgroundWorker TryCreateBranchWorker;
        private System.ComponentModel.BackgroundWorker BranchCreateWorker;

        public MainForm()
        {
            InitializeComponent();
            // Set static SaveData reference for BranchHandler
            BranchHandler.SaveDataInstance = SaveData;
            // Initialize background worker for adding games
            TryCreateBranchWorker = new System.ComponentModel.BackgroundWorker();
            TryCreateBranchWorker.DoWork += TryCreateBranchWorker_DoWork;
            TryCreateBranchWorker.RunWorkerCompleted += TryCreateBranchWorker_RunWorkerCompleted;
            TryCreateBranchWorker.WorkerReportsProgress = true; 
            // Initialize background worker for branch creation
            BranchCreateWorker = new System.ComponentModel.BackgroundWorker();
            BranchCreateWorker.WorkerReportsProgress = true; 
            RefreshLists(SaveData.SelectedGame);
            // Wire up branch tab events
            AddBranchButton.Click += AddBranchButton_Click;
            RenameBranchButton.Click += RenameBranchButton_Click;
            DeleteBranchButton.Click += DeleteBranchButton_Click;
        }

        private void RefreshLists(string selectedItem = "")
        {
            LoadSaveData();
            GamesLabel.Text = @"Games";
            Games.Items.Clear();
            if (SaveData.Games.Count == 0) return;
            foreach (var game in SaveData.Games)
                Games.Items.Add(game.Key);

            if (selectedItem == string.Empty) return;
            for (var index = 0; index < Games.Items.Count; index++)
                if (Games.Items[index].ToString() == selectedItem)
                {
                    Games.SetSelected(index, true);
                }
        }

        #region SaveSystems

        internal static SaveData SaveData;

        internal static readonly string SavePath =
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "save.json");

        private static void LoadSaveData()
        {
            if (File.Exists(SavePath))
            {
                SaveData = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(SavePath));
            }
            else
            {
                SaveData = new SaveData();
                Save();
            }
        }

        internal static void Save()
        {
            File.WriteAllText(SavePath,
                JsonConvert.SerializeObject(SaveData,
                    new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        NullValueHandling = NullValueHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));
        }

        #endregion

        private void GamesBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (Games.SelectedItem == null)
            {
                GamesLabel.Text = @"Games";
                SaveData.SelectedGame = string.Empty;
                Games.ClearSelected();
                // Clear branches tab
                BranchesListBox.Items.Clear();
                BranchNameTextBox.Text = string.Empty;
                BranchesTabPage.Text = "Branches";
                MainTabControl.TabPages.Remove(BranchesTabPage); // Hide tab if nothing selected
                return;
            }

            GamesLabel.Text = $@"Selected: {Games.SelectedItem}";
            SaveData.SelectedGame = Games.SelectedItem.ToString();

            // Update branches tab
            LoadBranchesForSelectedGame();
            BranchesTabPage.Text = $"Branches - {SaveData.SelectedGame}";
            if (!MainTabControl.TabPages.Contains(BranchesTabPage))
                MainTabControl.TabPages.Add(BranchesTabPage); // Show tab if selected
            MainTabControl.SelectedTab = BranchesTabPage;
        }

        // Make this public so BranchesForm can refresh the branch list after rename
        public void LoadBranchesForSelectedGame()
        {
            BranchesListBox.Items.Clear();
            BranchNameTextBox.Text = string.Empty;
            BranchesTabGameLabel.Text = SaveData.SelectedGame ?? ""; // Show selected game at top
            if (SaveData.SelectedGame == null) return;
            if (SaveData.Branches.TryGetValue(SaveData.SelectedGame, out var branches))
            {
                foreach (var branch in branches.Where(b => b != "original"))
                {
                    BranchesListBox.Items.Add(branch);
                }
            }
        }

        private void AddGameButton_Click(object sender, EventArgs e)
        {
            // Prompt for game folder
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            const string steamDefault = "C:/Program Files (x86)/Steam/steamapps/common/";
            if (Directory.Exists(steamDefault))
                dialog.SelectedPath = steamDefault;
            if (!(dialog.ShowDialog() ?? false)) return;
            var gamePath = dialog.SelectedPath.Replace(@"\", "/");
            var gameFolderName = Path.GetFileName(gamePath.TrimEnd('/', '\\'));
            AppLogger.Log($"Selected game folder: {gamePath} (folder name: {gameFolderName})");

            // Check if selected folder is a symlink
            var dirInfo = new DirectoryInfo(gamePath);
            if (dirInfo.Exists && dirInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
            {
                // Try to resolve the symlink target
                string targetPath = NativeSymlinkResolver.ResolveSymlink(gamePath);
                if (!string.IsNullOrEmpty(targetPath) && Directory.Exists(targetPath))
                {
                    // Check if target is a branch (usually Branches/original)
                    var parent = Directory.GetParent(targetPath);
                    if (parent != null && parent.Name == "Branches")
                    {
                        var branchNameSymlink = Path.GetFileName(targetPath);
                        var gameRootSymlink = Directory.GetParent(parent.FullName)?.FullName;
                        if (!string.IsNullOrEmpty(gameRootSymlink))
                        {
                            // Find ACF in branch
                            var acfSymlink = Directory.GetFiles(targetPath, "appmanifest_*.acf").FirstOrDefault();
                            if (acfSymlink != null)
                            {
                                // Try to get game name from ACF
                                string foundGameNameSymlink = null;
                                var lines = File.ReadAllLines(acfSymlink);
                                foreach (var line in lines)
                                {
                                    var parts = line.Split(new[] { '"' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (line.Trim().StartsWith("\"name\"") && parts.Length >= 4)
                                        foundGameNameSymlink = parts[3].Trim();
                                }
                                if (!string.IsNullOrWhiteSpace(foundGameNameSymlink))
                                {
                                    // Add to SaveData if not present
                                    if (!SaveData.Games.ContainsKey(foundGameNameSymlink))
                                    {
                                        SaveData.Games.Add(foundGameNameSymlink, gamePath);
                                        SaveData.GamesACFPath.Add(foundGameNameSymlink, acfSymlink.Replace("\\", "/"));
                                        var branchesDirSymlink = Path.Combine(gameRootSymlink, "Branches");
                                        var branchesSymlink = new List<string>();
                                        if (Directory.Exists(branchesDirSymlink))
                                        {
                                            foreach (var dirName in Directory.GetDirectories(branchesDirSymlink))
                                            {
                                                var branch = Path.GetFileName(dirName);
                                                if (!string.IsNullOrWhiteSpace(branch) && !branchesSymlink.Contains(branch))
                                                    branchesSymlink.Add(branch);
                                            }
                                        }
                                        if (!branchesSymlink.Contains(branchNameSymlink))
                                            branchesSymlink.Add(branchNameSymlink);
                                        SaveData.Branches.Add(foundGameNameSymlink, branchesSymlink);
                                        Save();
                                        RefreshLists(foundGameNameSymlink);
                                        MessageBox.Show($"Symlink detected and game '{foundGameNameSymlink}' restored to management.");
                                        return;
                                    }
                                    else
                                    {
                                        MessageBox.Show($"Game '{foundGameNameSymlink}' is already managed.");
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                MessageBox.Show("Selected folder is a symlink, but could not verify or restore the game setup.");
                return;
            }

            // Search for steamapps folder in parent directories
            string steamAppsDir = null;
            var dir = new DirectoryInfo(gamePath);
            while (dir != null)
            {
                var candidate = Path.Combine(dir.FullName, "steamapps");
                if (Directory.Exists(candidate))
                {
                    steamAppsDir = candidate;
                    break;
                }
                dir = dir.Parent;
            }
            AppLogger.Log($"Found steamapps directory: {steamAppsDir}");
            if (steamAppsDir == null)
            {
                AppLogger.Log("Could not find a parent steamapps directory for this game folder.");
                MessageBox.Show("Could not find a parent steamapps directory for this game folder.");
                return;
            }

            // Find matching ACF
            var acfFiles = Directory.GetFiles(steamAppsDir, "appmanifest_*.acf");
            AppLogger.Log($"Found {acfFiles.Length} ACF files in {steamAppsDir}");
            string foundAcf = null;
            string foundGameName = null;
            string foundInstallDir = null;
            foreach (var acf in acfFiles)
            {
                AppLogger.Log($"Checking ACF: {acf}");
                var lines = File.ReadAllLines(acf);
                string installdir = null;
                string name = null;
                foreach (var line in lines)
                {
                    var parts = line.Split(new[] { '"' }, StringSplitOptions.RemoveEmptyEntries);
                    if (line.Trim().StartsWith("\"installdir\"") && parts.Length >= 4)
                        installdir = parts[3].Trim();
                    if (line.Trim().StartsWith("\"name\"") && parts.Length >= 4)
                        name = parts[3].Trim();
                }
                AppLogger.Log($"Parsed installdir: '{installdir}', name: '{name}'");
                if (!string.IsNullOrWhiteSpace(installdir) &&
                    string.Equals(installdir.Trim(), gameFolderName.Trim(), StringComparison.InvariantCultureIgnoreCase))
                {
                    foundAcf = acf.Replace("\\", "/");
                    foundGameName = name;
                    foundInstallDir = installdir;
                    AppLogger.Log($"Match found: {foundAcf} for game '{foundGameName}' (installdir: '{foundInstallDir}')");
                    break;
                }
            }
            if (foundAcf == null || foundGameName == null || foundInstallDir == null)
            {
                AppLogger.Log($"No matching ACF found for folder '{gameFolderName}' in {steamAppsDir}");
                MessageBox.Show($"Could not find a matching Steam ACF manifest for this folder.\nFolder: {gameFolderName}\nChecked steamapps: {steamAppsDir}");
                return;
            }
            // Check for duplicates (and path mismatch)
            if (SaveData.Games.TryGetValue(foundGameName, out var existingPath))
            {
                if (!string.Equals(existingPath, gamePath, StringComparison.InvariantCultureIgnoreCase))
                {
                    MessageBox.Show($"A game with the name '{foundGameName}' already exists but points to a different path. Aborting.");
                    return;
                }
                else
                {
                    MessageBox.Show($"{foundGameName} already exists.", "");
                    return;
                }
            }
            // Save installdir for this game
            SaveData.GamesInstallDir[foundGameName] = foundInstallDir;
            // Detect extra branches (not 'original')
            var gameBranchesDir = BranchHandler.GetGameBranchesPath(Path.Combine(Path.GetDirectoryName(gamePath), foundInstallDir), foundInstallDir);
            var extraBranches = new List<string>();
            if (Directory.Exists(gameBranchesDir))
            {
                foreach (var dirName in Directory.GetDirectories(gameBranchesDir))
                {
                    var branch = Path.GetFileName(dirName);
                    if (!string.IsNullOrWhiteSpace(branch) && branch != "original" && !extraBranches.Contains(branch))
                        extraBranches.Add(branch);
                }
            }
            // Use background worker to process original branch setup, pass extra branches
            AppLogger.Log($"Starting branch creation for {foundGameName} at {gamePath} with ACF {foundAcf} (installdir: {foundInstallDir})");
            var args = new Tuple<string, string, string, string, string, List<string>>(foundGameName, gamePath, foundAcf, foundInstallDir, "original", extraBranches);
            TryCreateBranchWorker.RunWorkerAsync(args);
        }

        private void TryCreateBranchWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            System.Diagnostics.Debug.Assert(sender != null, "BackgroundWorker sender should never be null in DoWork");
            var args = (Tuple<string, string, string, string, string, List<string>>)e.Argument;
            // args: (gameName, gamePath, acfPath, installDir, branchName, extraBranches)
            e.Result = new Tuple<object, object>(
                BranchHandler.TryCreateBranch(args.Item1, args.Item2, args.Item3, args.Item5, (System.ComponentModel.BackgroundWorker)sender),
                args.Item6 // pass extra branches
            );
        }

        private void TryCreateBranchWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                return;
            var tuple = (Tuple<object, object>)e.Result;
            var state = (Tuple<DialogResult, string, string, string, string>)tuple.Item1;
            DialogResult result = state.Item1;
            if (result != DialogResult.OK) return;
            var gameName = state.Item2;
            var gamePath = state.Item3;
            var acfPath = state.Item4;
            var installDir = state.Item5;
            // Only add if not already present, and check for path mismatch
            if (SaveData.Games.TryGetValue(gameName, out var existingPath))
            {
                if (!string.Equals(existingPath, gamePath, StringComparison.InvariantCultureIgnoreCase))
                {
                    AppLogger.Log($"[BRANCH-RESCAN] Game '{gameName}' already exists but points to a different path. Aborting.");
                    MessageBox.Show($"A game with the name '{gameName}' already exists but points to a different path. Aborting.");
                    return;
                }
            }
            else
            {
                AppLogger.Log($"[BRANCH-RESCAN] Adding new game '{gameName}' at '{gamePath}'");
                SaveData.Games.Add(gameName, gamePath);
                SaveData.GamesACFPath.Add(gameName, acfPath);
                SaveData.GamesInstallDir[gameName] = installDir;
            }
            // Always rescan the Branches directory for all branches
            var gameBranchesDir = BranchHandler.GetGameBranchesPath(Path.Combine(Path.GetDirectoryName(gamePath), installDir), installDir);
            AppLogger.Log($"[BRANCH-RESCAN] Scanning for branches in: {gameBranchesDir}");
            var allBranches = new List<string>();
            if (Directory.Exists(gameBranchesDir))
            {
                foreach (var dirName in Directory.GetDirectories(gameBranchesDir))
                {
                    var branch = Path.GetFileName(dirName);
                    AppLogger.Log($"[BRANCH-RESCAN] Found branch directory: {dirName} (branch: {branch})");
                    if (!string.IsNullOrWhiteSpace(branch) && !allBranches.Contains(branch))
                        allBranches.Add(branch);
                }
            }
            if (!allBranches.Contains("original"))
                allBranches.Insert(0, "original");
            AppLogger.Log($"[BRANCH-RESCAN] Final branch list for '{gameName}': {string.Join(", ", allBranches)}");
            SaveData.Branches[gameName] = allBranches;
            Save();
            RefreshLists(gameName);
        }

        private void DeleteGame_Click(object sender, EventArgs e)
        {
            var currentGame = Games.SelectedItem?.ToString() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(currentGame))
                return;

            if (MessageBox.Show(
                    @"Are you sure you want to unmanage this game?\nThe original files will be restored, but all branches will be preserved unless you choose to delete them.",
                    "", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            // Restore original files before removing from SaveData
            string gamePath = null, acfPath = null, installDir = null;
            if (SaveData.Games.TryGetValue(currentGame, out gamePath) &&
                SaveData.GamesACFPath.TryGetValue(currentGame, out acfPath) &&
                SaveData.GamesInstallDir.TryGetValue(currentGame, out installDir))
            {
                BranchHandler.RestoreOriginalFiles(gamePath, acfPath);
                // Check for extra branches
                var gameBranchesDir = BranchHandler.GetGameBranchesPath(Path.Combine(Path.GetDirectoryName(gamePath), installDir), installDir);
                if (Directory.Exists(gameBranchesDir))
                {
                    var extraBranches = Directory.GetDirectories(gameBranchesDir)
                        .Select(Path.GetFileName)
                        .Where(b => !string.IsNullOrWhiteSpace(b) && b != "original")
                        .ToList();
                    if (extraBranches.Count > 0)
                    {
                        var deleteBranches = MessageBox.Show(
                            $"Do you want to delete all extra branches for this game? (Choosing No will keep them on disk for future use.)",
                            "Delete Extra Branches?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (deleteBranches == DialogResult.Yes)
                        {
                            foreach (var branch in extraBranches)
                            {
                                var branchPath = Path.Combine(gameBranchesDir, branch);
                                try { Directory.Delete(branchPath, true); } catch { }
                            }
                        }
                    }
                    // --- Remove Branches/{installdir} if empty ---
                    if (Directory.Exists(gameBranchesDir) && Directory.GetDirectories(gameBranchesDir).Length == 0)
                    {
                        try { Directory.Delete(gameBranchesDir); } catch { }
                    }
                }
                // --- Remove Branches folder if empty ---
                var gameRoot = BranchHandler.GetGameRoot(gamePath);
                var branchesRoot = Path.Combine(gameRoot, "Branches");
                if (Directory.Exists(branchesRoot) && Directory.GetDirectories(branchesRoot).Length == 0)
                {
                    try { Directory.Delete(branchesRoot); } catch { }
                }
            }

            SaveData.Games.Remove(currentGame);
            SaveData.GamesACFPath.Remove(currentGame);
            SaveData.GamesInstallDir.Remove(currentGame);
            SaveData.Branches.Remove(currentGame);
            Save();
            RefreshLists();

            // --- Hide branches tab if no games are left ---
            if (Games.Items.Count == 0 && MainTabControl.TabPages.Contains(BranchesTabPage))
            {
                MainTabControl.TabPages.Remove(BranchesTabPage);
            }
        }

        // Add missing event handler stubs to resolve build errors
        private void AddBranchButton_Click(object sender, EventArgs e)
        {
            var branchName = BranchNameTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(branchName))
            {
                MessageBox.Show("Branch name must not be empty!");
                return;
            }
            const string pattern = @"[^0-9a-zA-Z ]+";
            if (System.Text.RegularExpressions.Regex.Match(branchName, pattern).Success)
            {
                MessageBox.Show("Names can only contain alphanumeric characters and spaces");
                return;
            }
            if (SaveData.Branches[SaveData.SelectedGame].Any(b => b.Equals(branchName, StringComparison.InvariantCultureIgnoreCase)))
            {
                MessageBox.Show($"{branchName} already exists.");
                return;
            }
            // Use background worker to process branch creation
            var args = new Tuple<string, string, string, string>(
                SaveData.SelectedGame,
                SaveData.Games[SaveData.SelectedGame],
                SaveData.GamesACFPath[SaveData.SelectedGame],
                branchName);
            using (var progressForm = new BranchProgressForm())
            {
                BranchCreateWorker.WorkerSupportsCancellation = true;
                BranchCreateWorker.ProgressChanged += (s, ev) =>
                {
                    if (ev.UserState is string fileName)
                        progressForm.SetProgress(ev.ProgressPercentage, fileName);
                };
                BranchCreateWorker.RunWorkerCompleted += (s, ev) =>
                {
                    progressForm.SetComplete();
                };
                BranchCreateWorker.DoWork += (s, ev) =>
                {
                    var tuple = (Tuple<string, string, string, string>)ev.Argument;
                    ev.Result = BranchHandler.TryCreateBranch(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, (BackgroundWorker)s);
                };
                BranchCreateWorker.RunWorkerAsync(args);
                progressForm.ShowDialog(this);
                if (progressForm.CancelRequested)
                {
                    // TODO: Delete the branch folder if cancelled
                }
                BranchCreateWorker.CancelAsync();
                BranchNameTextBox.Text = string.Empty;

                // --- Fix: Rescan branches and update UI after branch creation ---
                var gameName = SaveData.SelectedGame;
                var gamePath = SaveData.Games[gameName];
                var installDir = SaveData.GamesInstallDir[gameName];
                var gameBranchesDir = BranchHandler.GetGameBranchesPath(gamePath, installDir);
                var allBranches = new List<string>();
                if (Directory.Exists(gameBranchesDir))
                {
                    foreach (var dirName in Directory.GetDirectories(gameBranchesDir))
                    {
                        var branch = Path.GetFileName(dirName);
                        if (!string.IsNullOrWhiteSpace(branch) && !allBranches.Contains(branch))
                            allBranches.Add(branch);
                    }
                }
                if (!allBranches.Contains("original"))
                    allBranches.Insert(0, "original");
                SaveData.Branches[gameName] = allBranches;
                Save();
                LoadBranchesForSelectedGame();
                // --- End fix ---
            }
        }
        private void RenameBranchButton_Click(object sender, EventArgs e)
        {
            AppLogger.Log($"[MainForm] RenameBranchButton_Click called. SelectedGame: {SaveData.SelectedGame}, BranchesListBox.SelectedItem: {BranchesListBox.SelectedItem}, BranchNameTextBox: '{BranchNameTextBox.Text}'");
            // --- Real branch rename logic moved here from BranchesForm ---
            if (BranchesListBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a branch to rename.");
                AppLogger.Log("[MainForm] No branch selected.");
                return;
            }
            var currentBranch = BranchesListBox.SelectedItem.ToString();
            var newBranchName = BranchNameTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(newBranchName))
            {
                MessageBox.Show("Branch name must not be empty!");
                AppLogger.Log("[MainForm] New branch name is empty.");
                return;
            }
            if (newBranchName == currentBranch)
            {
                MessageBox.Show("The new branch name is the same as the current name.");
                AppLogger.Log("[MainForm] New branch name is the same as current.");
                return;
            }
            const string pattern = @"[^0-9a-zA-Z ]+";
            if (System.Text.RegularExpressions.Regex.Match(newBranchName, pattern).Success)
            {
                MessageBox.Show("Names can only contain alphanumeric characters and spaces");
                AppLogger.Log("[MainForm] New branch name contains invalid characters.");
                return;
            }
            if (SaveData.Branches[SaveData.SelectedGame].Any(b => b.Equals(newBranchName, StringComparison.InvariantCultureIgnoreCase)))
            {
                MessageBox.Show($"A branch named '{newBranchName}' already exists.");
                AppLogger.Log($"[MainForm] Branch '{newBranchName}' already exists.");
                return;
            }
            var branches = SaveData.Branches[SaveData.SelectedGame];
            var idx = branches.IndexOf(currentBranch);
            AppLogger.Log($"[MainForm] Index of current branch: {idx}");
            if (idx >= 0)
            {
                // Rename the branch folder on disk
                var gamePath = SaveData.Games[SaveData.SelectedGame];
                var installDir = SaveData.GamesInstallDir[SaveData.SelectedGame];
                var oldBranchPath = BranchHandler.GetBranchPath(gamePath, installDir, currentBranch);
                var newBranchPath = BranchHandler.GetBranchPath(gamePath, installDir, newBranchName);
                AppLogger.Log($"[MainForm] Renaming folder: '{oldBranchPath}' to '{newBranchPath}'");
                try
                {
                    if (Directory.Exists(oldBranchPath))
                    {
                        Directory.Move(oldBranchPath, newBranchPath);
                        AppLogger.Log($"[MainForm] Directory renamed successfully.");
                    }
                    else
                    {
                        AppLogger.Log($"[MainForm] Old branch path does not exist: {oldBranchPath}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to rename branch folder: {ex.Message}");
                    AppLogger.Log($"[MainForm] Exception: {ex}");
                    return;
                }
                branches[idx] = newBranchName;
                Save();
                LoadBranchesForSelectedGame();
                BranchNameTextBox.Text = string.Empty;
            }
            else
            {
                AppLogger.Log("[MainForm] Branch index not found in list.");
            }
        }
        private void DeleteBranchButton_Click(object sender, EventArgs e)
        {
            AppLogger.Log($"[MainForm] DeleteBranchButton_Click called. SelectedGame: {SaveData.SelectedGame}, BranchesListBox.SelectedItem: {BranchesListBox.SelectedItem}");
            if (BranchesListBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a branch to delete.");
                AppLogger.Log("[MainForm] No branch selected for deletion.");
                return;
            }
            var branchToDelete = BranchesListBox.SelectedItem.ToString();
            if (string.IsNullOrWhiteSpace(branchToDelete))
            {
                MessageBox.Show("Invalid branch selected.");
                AppLogger.Log("[MainForm] Selected branch name is empty or whitespace.");
                return;
            }
            if (branchToDelete == "original")
            {
                MessageBox.Show("You cannot delete the 'original' branch.");
                AppLogger.Log("[MainForm] Attempted to delete 'original' branch, which is not allowed.");
                return;
            }
            var confirm = MessageBox.Show($"Are you sure you want to delete the branch '{branchToDelete}'? This cannot be undone.", "Delete Branch", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes)
            {
                AppLogger.Log($"[MainForm] User cancelled deletion of branch '{branchToDelete}'.");
                return;
            }
            try
            {
                var gameName = SaveData.SelectedGame;
                var gamePath = SaveData.Games[gameName];
                var acfPath = SaveData.GamesACFPath[gameName];
                var installDir = SaveData.GamesInstallDir[gameName];
                AppLogger.Log($"[MainForm] Attempting to delete branch '{branchToDelete}' for game '{gameName}' at '{gamePath}' with acf '{acfPath}'.");
                // Use a background worker for deletion
                using (var worker = new BackgroundWorker())
                {
                    worker.WorkerReportsProgress = true;
                    worker.DoWork += (s, ev) =>
                    {
                        ev.Result = BranchHandler.TryDeleteBranch(gameName, gamePath, acfPath, branchToDelete, worker);
                    };
                    worker.RunWorkerCompleted += (s, ev) =>
                    {
                        if (ev.Error != null)
                        {
                            AppLogger.Log($"[MainForm] Exception during branch deletion: {ev.Error}");
                            MessageBox.Show($"Error deleting branch: {ev.Error.Message}");
                            return;
                        }
                        var resultTuple = (Tuple<DialogResult, string, string, string, string>)ev.Result;
                        AppLogger.Log($"[MainForm] Branch deletion result: {resultTuple.Item1}, branch: {resultTuple.Item5}");
                        if (resultTuple.Item1 == DialogResult.OK)
                        {
                            // Remove from SaveData and update UI
                            if (SaveData.Branches.TryGetValue(gameName, out var branchList))
                            {
                                branchList.Remove(branchToDelete);
                                Save();
                                LoadBranchesForSelectedGame();
                                AppLogger.Log($"[MainForm] Branch '{branchToDelete}' deleted and UI updated.");
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Failed to delete branch '{branchToDelete}'.");
                            AppLogger.Log($"[MainForm] Failed to delete branch '{branchToDelete}'.");
                        }
                    };
                    worker.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                AppLogger.Log($"[MainForm] Exception in DeleteBranchButton_Click: {ex}");
                MessageBox.Show($"Exception: {ex.Message}");
            }
        }        
    }
}