using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
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

        public MainForm()
        {
            InitializeComponent();
            RefreshLists(SaveData.SelectedGame);
        }

        private void RefreshLists(string selectedItem = "")
        {
            LoadSaveData();
            GamesLabel.Text = @"Games";
            BranchesLabel.Text = @"Branches";
            Games.Items.Clear();
            Branches.Items.Clear();
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

        private static void Save()
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

        #region ValueChanges

        private void GamesBox_SelectedValueChanged(object sender, EventArgs e)
        {
            Branches.Items.Clear();
            if (Games.SelectedItem == null)
            {
                GamesLabel.Text = @"Games";
                SaveData.SelectedGame = string.Empty;
                Games.ClearSelected();
                return;
            }

            NameTextBox.Text = Games.SelectedItem.ToString();
            GamesLabel.Text = $@"Selected: {Games.SelectedItem}";
            SaveData.SelectedGame = Games.SelectedItem.ToString();
            if (!SaveData.Branches.TryGetValue(SaveData.SelectedGame, out var branches)) return;

            foreach (var branch in branches.Where(branch => branch != "original")) Branches.Items.Add(branch);

            BranchesLabel.Text = $@"Branches: {Branches.Items.Count}";
            if(SaveData.SelectedBranches.TryGetValue(SaveData.SelectedGame, out var b) && !string.IsNullOrWhiteSpace(b))
                for (var i = 0; i < Branches.Items.Count; i++)
                    if (b == Branches.Items[i].ToString())
                    {
                        BranchesLabel.Text = $@"Selected: {b}";
                        Branches.ItemCheck -= Branches_ItemCheck;
                        Branches.SetItemChecked(i, true);
                        Branches.ItemCheck += Branches_ItemCheck;
                        break;
                    }
            
            Save();
        }

        private void Branches_SelectedValueChanged(object sender, EventArgs e)
        {
            Branches.ClearSelected();
        }
        
        private void Branches_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked && Branches.CheckedItems.Count > 0)
            {
                Branches.ItemCheck -= Branches_ItemCheck;
                Branches.SetItemChecked(Branches.CheckedIndices[0], false);
                Branches.ItemCheck += Branches_ItemCheck;
            }


            var gamePath = SaveData.Games[SaveData.SelectedGame];
            var acfPath = SaveData.GamesACFPath[SaveData.SelectedGame];
            BranchHandler.SetActiveBranch(gamePath, acfPath,
                e.NewValue == CheckState.Checked ? Branches.SelectedItem.ToString() : "original");


            SaveData.SelectedBranches[SaveData.SelectedGame] =
                e.NewValue == CheckState.Checked ? Branches.SelectedItem.ToString(): string.Empty;
            Save();
            RefreshLists(SaveData.SelectedGame);
        }

        #endregion

        #region Buttons

        private void AddGameButton_Click(object sender, EventArgs e)
        {
            if (Working) return;

            var gameName = NameTextBox.Text;
            if (string.IsNullOrWhiteSpace(gameName))
            {
                MessageBox.Show(@"Name must not be empty!");
                return;
            }

            const string pattern = @"[^0-9a-zA-Z ]+";
            if (Regex.Match(gameName, pattern).Success)
            {
                MessageBox.Show(@"Names can only contain alphanumeric characters and spaces");
                return;
            }

            if (SaveData.Games.Keys.Any(game => game.ToString().ToLowerInvariant() == gameName.ToLowerInvariant()))
            {
                MessageBox.Show($@"{gameName} already exists.", "");
                return;
            }

            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();

            const string steamDefault = "C:/Program Files (x86)/Steam/steamapps/common/";
            if (Directory.Exists(steamDefault))
                dialog.SelectedPath = steamDefault;

            if (!(dialog.ShowDialog() ?? false)) return;

            var gamePath = dialog.SelectedPath.Replace(@"\", "/");

            VistaOpenFileDialog fileBrowser = new VistaOpenFileDialog
            {
                Filter = @"acf files (*.acf)|*.acf",
                Multiselect = false,
                Title = @"Please Select this games .acf file from the steamapps folder.",
                CheckFileExists = true
            };

            const string steamAppsDefault = "C:/Program Files (x86)/Steam/steamapps/";
            if (Directory.Exists(steamAppsDefault))
                fileBrowser.FileName = steamAppsDefault;

            if (!(fileBrowser.ShowDialog() ?? false)) return;

            var acfPath = fileBrowser.FileName.Replace(@"\", "/");


            FilesCopiedLabel.Text = string.Empty;
            var args = new Tuple<string, string, string, string>(gameName, gamePath, acfPath, "original");
            TryCreateBranchWorker.RunWorkerAsync(args);

            NameTextBox.Text = string.Empty;
        }

        private void DeleteGame_Click(object sender, EventArgs e)
        {
            var currentGame = Games.SelectedItem?.ToString() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(currentGame))
                return;

            if (MessageBox.Show(
                    @"Are you sure?\nThis will delete all branches and put the original files back in there default location!",
                    "", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            if (!SaveData.Games.TryGetValue(currentGame, out var gamePath) ||
                !SaveData.GamesACFPath.TryGetValue(currentGame, out var acfPath) ||
                !SaveData.Branches.TryGetValue(currentGame, out var branches)) return;

            DeleteGameWorker.RunWorkerAsync(
                new Tuple<string, string, string, string[]>(currentGame, gamePath, acfPath, branches.ToArray()));
        }

        private void RenameGame_Click(object sender, EventArgs e)
        {
            var currentGame = SaveData.SelectedGame ?? string.Empty;
            var newGameName = NameTextBox.Text;

            const string pattern = @"[^0-9a-zA-Z ]+";
            if (Regex.Match(newGameName, pattern).Success)
            {
                MessageBox.Show(@"Names can only contain alphanumeric characters and spaces");
                return;
            }

            NameTextBox.Text = string.Empty;
            if (string.IsNullOrWhiteSpace(currentGame) ||
                string.IsNullOrWhiteSpace(newGameName) ||
                currentGame == newGameName) return;

            if (!string.Equals(currentGame, newGameName, StringComparison.InvariantCultureIgnoreCase))
                if (SaveData.Games.Keys.Any(game =>
                        string.Equals(game.ToString(), newGameName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    MessageBox.Show($@"{newGameName} already exists.", "");
                    return;
                }

            if (!SaveData.Games.TryGetValue(currentGame, out var gamePath))
                throw new Exception($"Game Path not found for {currentGame}");

            if (!SaveData.Branches.TryGetValue(currentGame, out var branches))
                throw new Exception($"Game Path not found for {currentGame}");

            if (!SaveData.GamesACFPath.TryGetValue(currentGame, out var acfPath))
                throw new Exception($"Game Path not found for {currentGame}");


            SaveData.Games.Add(newGameName, gamePath);
            SaveData.Branches.Add(newGameName, branches);
            SaveData.GamesACFPath.Add(newGameName, acfPath);
            SaveData.Games.Remove(currentGame);
            SaveData.Branches.Remove(currentGame);
            SaveData.GamesACFPath.Remove(currentGame);

            Save();
            RefreshLists(newGameName);
        }

        private void CreateBranch_Click(object sender, EventArgs e)
        {
            if (Working) return;


            const string pattern = @"[^0-9a-zA-Z ]+";
            if (Regex.Match(NameTextBox.Text, pattern).Success)
            {
                MessageBox.Show(@"Names can only contain alphanumeric characters and spaces");
                return;
            }

            var currentGame = SaveData.SelectedGame;
            var branchName = NameTextBox.Text;
            if (branchName == string.Empty)
                return;

            if (!SaveData.Branches.TryGetValue(currentGame, out var branches))
                branches = SaveData.Branches[currentGame] = new List<string>();

            
            if (branches.Any(branch => string.Equals(branch, branchName, StringComparison.InvariantCultureIgnoreCase)))
            {
                MessageBox.Show($@"{branchName} already exists.", "");
                return;
            }

            if (!SaveData.Games.TryGetValue(currentGame, out var gamePath) ||
                !Directory.Exists($"{gamePath}Branches/original"))
            {
                MessageBox.Show(@"Failed to find original game copy to clone!", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (!SaveData.GamesACFPath.TryGetValue(currentGame, out var acfPath) ||
                !File.Exists($"{gamePath}Branches/original/{Path.GetFileName(acfPath)}"))
            {
                MessageBox.Show(@"Failed to find original acf copy to clone!", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            FilesCopiedLabel.Text = string.Empty;
            var args = new Tuple<string, string, string, string>(currentGame, gamePath, acfPath, branchName);
            TryCreateBranchWorker.RunWorkerAsync(args);
            NameTextBox.Text = string.Empty;
        }

        private void DeleteBranch_Click(object sender, EventArgs e)
        {
            var currentGame = SaveData.SelectedGame ?? string.Empty;
            var currentBranch = SaveData.SelectedBranches[currentGame] ?? string.Empty;
            if (string.IsNullOrWhiteSpace(currentGame) || string.IsNullOrWhiteSpace(currentBranch))
                return;

            if (MessageBox.Show($@"Are you sure you want to delete {currentBranch} from {currentGame}?", "",
                    MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            if (!SaveData.Games.TryGetValue(currentGame, out var gamePath) ||
                !SaveData.GamesACFPath.TryGetValue(currentGame, out var acfPath)) return;

            DeleteBranchWorker.RunWorkerAsync(
                new Tuple<string, string, string, string>(currentGame, gamePath, acfPath, currentBranch));
        }

        private void RenameBranch_Click(object sender, EventArgs e)
        {
            var currentGame = SaveData.SelectedGame ?? string.Empty;
            var currentBranch = SaveData.SelectedBranches[currentGame] ?? string.Empty;

            var newBranchName = NameTextBox.Text;

            NameTextBox.Text = string.Empty;
            if (string.IsNullOrWhiteSpace(currentGame) ||
                string.IsNullOrWhiteSpace(currentBranch) ||
                string.IsNullOrWhiteSpace(newBranchName) ||
                newBranchName == currentBranch) return;

            const string pattern = @"[^0-9a-zA-Z ]+";
            if (Regex.Match(newBranchName, pattern).Success)
            {
                MessageBox.Show(@"Names can only contain alphanumeric characters and spaces");
                return;
            }

            if (!SaveData.Games.TryGetValue(currentGame, out var gamePath))
                throw new Exception($"Game Path not found for {currentGame}");

            if (!string.Equals(currentBranch, newBranchName, StringComparison.InvariantCultureIgnoreCase))
            {
                if (Directory.Exists($"{gamePath}Branches/{newBranchName}"))
                    throw new Exception($"{gamePath}Branches/{newBranchName}\nFolder already exists!");
                Directory.Move($"{gamePath}Branches/{currentBranch}", $"{gamePath}Branches/{newBranchName}");
            }
            else
            {
                Directory.Move($"{gamePath}Branches/{currentBranch}", $"{gamePath}Branches/{newBranchName}1");
                Directory.Move($"{gamePath}Branches/{newBranchName}1", $"{gamePath}Branches/{newBranchName}");
            }


            SaveData.Branches[currentGame].Remove(currentBranch);
            SaveData.Branches[currentGame].Add(newBranchName);

            Save();
            RefreshLists(currentGame);
        }

        #endregion

        #region Workers

        internal static bool Working;

        #region TryCreateBranchWorker

        private void TryCreateBranchWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Working = true;
            var args = (Tuple<string, string, string, string>) e.Argument;
            BackgroundWorker worker = sender as BackgroundWorker;

            e.Result = BranchHandler.TryCreateBranch(args.Item1, args.Item2, args.Item3, args.Item4, worker);
        }

        private void TryCreateBranchWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FilesCopiedLabel.Text = e.UserState.ToString();
        }

        private void TryCreateBranchWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Working = false;
            if (e.Error != null)
                return;

            //string gameName, string gamePath, string acfPath, string branchName
            var state = (Tuple<DialogResult, string, string, string, string>) e.Result;

            DialogResult result = state.Item1;
            if (result != DialogResult.OK) return;

            var gameName = state.Item2;
            var gamePath = state.Item3;
            var acfPath = state.Item4;
            var branchName = state.Item5;

            if (branchName == "original")
            {
                SaveData.Games.Add(gameName, gamePath);
                SaveData.GamesACFPath.Add(gameName, acfPath);
                SaveData.Branches.Add(gameName, new List<string> {"original"});
            }
            else
            {
                SaveData.Branches[gameName].Add(branchName);
            }

            Save();
            RefreshLists(gameName);
        }

        #endregion

        #region DeleteBranchWorker

        private void DeleteBranchWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Working = true;
            var args = (Tuple<string, string, string, string>) e.Argument;
            BackgroundWorker worker = sender as BackgroundWorker;

            var gameName = args.Item1;
            var gamePath = args.Item2;
            var acfPath = args.Item3;
            var branchName = args.Item4;

            e.Result = BranchHandler.TryDeleteBranch(gameName, gamePath, acfPath, branchName, worker);
        }

        private void DeleteBranchWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FilesCopiedLabel.Text = e.UserState.ToString();
        }

        private void DeleteBranchWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Working = false;
            //string gameName, string gamePath, string acfPath, string branchName
            var state = (Tuple<DialogResult, string, string, string, string>) e.Result;

            DialogResult result = state.Item1;
            if (result != DialogResult.OK) return;

            var gameName = state.Item2;
            var branchName = state.Item5;

            SaveData.Branches[gameName].Remove(branchName);
            Branches.Items.Remove(branchName);

            Save();
            RefreshLists(gameName);
        }

        #endregion

        #region DeleteGameWorker

        private void DeleteGameWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Working = true;
            var args = (Tuple<string, string, string, string[]>) e.Argument;
            BackgroundWorker worker = sender as BackgroundWorker;

            var gameName = args.Item1;
            var gamePath = args.Item2;
            var acfPath = args.Item3;
            var branches = args.Item4;

            e.Result = BranchHandler.TryDeleteGame(gameName, gamePath, acfPath, branches, worker);
        }

        private void DeleteGameWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FilesCopiedLabel.Text = e.UserState.ToString();
        }

        private void DeleteGameWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Working = false;
            //string gameName, string gamePath, string acfPath, string branchName
            var state = (Tuple<DialogResult, string, string, string, string[]>) e.Result;

            DialogResult result = state.Item1;
            if (result != DialogResult.OK) return;

            var gameName = state.Item2;
            SaveData.Games.Remove(gameName);
            SaveData.GamesACFPath.Remove(gameName);
            SaveData.Branches.Remove(gameName);

            Branches.Items.Clear();
            Save();
            RefreshLists();
        }

        #endregion

        #endregion

    }
}