using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Steam_Games_Branch_Manager
{
    internal static class BranchHandler
    {
        // Branches root is now relative to the game root (e.g., E:/SteamLibrary/steamapps/common/Branches/GameName/BranchName)
        public static string GetGameRoot(string gamePath)
        {
            // gamePath: E:/SteamLibrary/steamapps/common/Jedi Fallen Order
            // returns: E:/SteamLibrary/steamapps/common
            return Directory.GetParent(gamePath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)).FullName;
        }
        public static string GetGameBranchesPath(string gamePath, string installdir)
        {
            return Path.Combine(GetGameRoot(gamePath), "Branches", installdir);
        }
        public static string GetBranchPath(string gamePath, string installdir, string branchName)
        {
            return Path.Combine(GetGameBranchesPath(gamePath, installdir), branchName);
        }

        internal static Tuple<DialogResult, string, string, string, string> TryCreateBranch(string gameName,
            string gamePath, string acfPath, string branchName, BackgroundWorker worker, int depth = 0)
        {
            if (worker == null)
                throw new ArgumentNullException(nameof(worker), "BackgroundWorker must not be null when creating a branch. Check all call sites.");

            // Use installdir for all file/folder operations
            var installdir = SaveDataInstance?.GamesInstallDir != null && SaveDataInstance.GamesInstallDir.ContainsKey(gameName)
                ? SaveDataInstance.GamesInstallDir[gameName]
                : Path.GetFileName(gamePath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));

            if (depth == 5)
            {
                MessageBox.Show(@"Too many failed attempts. Aborting.");
                return new Tuple<DialogResult, string, string, string, string>(DialogResult.Abort, gameName, gamePath,
                    acfPath, branchName);
            }

            DialogResult result = DialogResult.OK;
            try
            {
                if (!CreateGameBranch(installdir, gamePath, acfPath, branchName, worker))
                    throw new Exception("CreateBranch returned false");
            }
            catch (Exception exception)
            {
                result = MessageBox.Show($@"Failed to Create Backup of {gameName} from {gamePath}: 
                    {exception}", @"ERROR",
                    MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                switch (result)
                {
                    case DialogResult.Abort:
                        result = DialogResult.Abort;
                        break;
                    case DialogResult.Ignore:
                        result = DialogResult.OK;
                        break;
                    case DialogResult.Retry:
                        result = TryCreateBranch(gameName, gamePath, acfPath, branchName, worker, depth + 1).Item1;
                        break;
                }
            }

            if (worker != null)
                worker.ReportProgress(100, result == DialogResult.OK ? $"Creating {branchName} Complete" : "Failed");

            return new Tuple<DialogResult, string, string, string, string>(result, gameName, gamePath, acfPath,
                branchName);
        }

        [DllImport("kernel32.dll")]
        private static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName,
            SymbolicLink dwFlags);

        private static bool CreateGameBranch(string installdir, string gamePath, string acfPath, string branchName,
            BackgroundWorker worker)
        {
            var gameBranchesPath = GetGameBranchesPath(gamePath, installdir);
            var branchPath = GetBranchPath(gamePath, installdir, branchName);
            var originalPath = GetBranchPath(gamePath, installdir, "original");
            Directory.CreateDirectory(gameBranchesPath);

            if (Directory.Exists(branchPath))
                throw new Exception($"{branchPath}\nAlready Exists!\nCannot overwrite existing folders!");

            if (branchName != "original" && !Directory.Exists(originalPath))
                throw new Exception($"Tried to create a branch but could not find \n{originalPath}");

            if (branchName != "original" && !File.Exists(Path.Combine(originalPath, Path.GetFileName(acfPath))))
                throw new Exception($"original Acf file not found at {Path.Combine(originalPath, Path.GetFileName(acfPath))}");

            if (branchName == "original")
            {
                Directory.Move(gamePath, originalPath);
                File.Move(acfPath, Path.Combine(originalPath, Path.GetFileName(acfPath)));

                CreateSymbolicLink(gamePath, originalPath, SymbolicLink.Directory);
                CreateSymbolicLink(acfPath, Path.Combine(originalPath, Path.GetFileName(acfPath)), SymbolicLink.File);

                FileInfo pathInfo = new FileInfo(acfPath);
                var success = pathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint);

                if (success)
                    return true;

                if (File.Exists(Path.Combine(originalPath, Path.GetFileName(acfPath))))
                    File.Move(Path.Combine(originalPath, Path.GetFileName(acfPath)), acfPath);
                if (Directory.Exists(originalPath))
                    Directory.Move(originalPath, gamePath);

                return false;
            }

            CopyDirectory(originalPath, branchPath, true, false, worker);
            return Directory.Exists(branchPath) &&
                   File.Exists(Path.Combine(branchPath, Path.GetFileName(acfPath)));
        }

        internal static void SetActiveBranch(string gamePath, string acfPath, string branchName)
        {
            var gameName = Path.GetFileName(gamePath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
            var branchPath = GetBranchPath(gamePath, gameName, branchName);
            var acfTarget = Path.Combine(branchPath, Path.GetFileName(acfPath));
            FileInfo acfPathInfo = new FileInfo(acfPath);
            if (File.Exists(acfTarget))
            {
                try
                {
                    acfPathInfo.Delete();
                }
                catch { }
                CreateSymbolicLink(acfPath, acfTarget, SymbolicLink.File);
            }
            DirectoryInfo gamePathInfo = new DirectoryInfo(gamePath);
            if (Directory.Exists(branchPath))
            {
                try
                {
                    gamePathInfo.Delete();
                }
                catch { }
                CreateSymbolicLink(gamePath, branchPath, SymbolicLink.Directory);
            }
        }

        internal static void RestoreOriginalFiles(string gamePath, string acfPath)
        {
            var gameName = Path.GetFileName(gamePath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
            var originalBranchPath = GetBranchPath(gamePath, gameName, "original");
            var originalAcfPath = Path.Combine(originalBranchPath, Path.GetFileName(acfPath));

            // Remove symlink at gamePath if it exists
            if (Directory.Exists(gamePath))
            {
                var dirInfo = new DirectoryInfo(gamePath);
                if (dirInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                {
                    dirInfo.Delete();
                }
            }
            // Remove symlink at acfPath if it exists
            if (File.Exists(acfPath))
            {
                var fileInfo = new FileInfo(acfPath);
                if (fileInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                {
                    fileInfo.Delete();
                }
            }
            // Move ACF file first
            if (File.Exists(originalAcfPath))
            {
                File.Move(originalAcfPath, acfPath);
            }
            // Then move original files/folder back
            if (Directory.Exists(originalBranchPath))
            {
                Directory.Move(originalBranchPath, gamePath);
            }
            // Clean up Branches/gameName if empty
            var gameBranchesPath = GetGameBranchesPath(gamePath, gameName);
            if (Directory.Exists(gameBranchesPath) && Directory.GetDirectories(gameBranchesPath).Length == 0)
            {
                Directory.Delete(gameBranchesPath);
            }
        }

        private static void CopyDirectory(string sourceDir, string destinationDir, bool recursive, bool deep,
            BackgroundWorker worker)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");
            var dirs = dir.GetDirectories();
            Directory.CreateDirectory(destinationDir);
            if (recursive && deep)
                foreach (DirectoryInfo subDir in dirs)
                {
                    var newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true, true, worker);
                }
            foreach (FileInfo file in dir.GetFiles())
            {
                var targetFilePath = Path.Combine(destinationDir, file.Name);
                worker.ReportProgress(0, file.Name);
                file.CopyTo(targetFilePath);
            }
            if (recursive && !deep)
                foreach (DirectoryInfo subDir in dirs)
                {
                    var newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true, false, worker);
                }
        }

        public static Tuple<DialogResult, string, string, string, string> TryDeleteBranch(string gameName,
            string gamePath, string acfPath, string branchName, BackgroundWorker worker, int depth = 0)
        {
            if (depth == 5)
            {
                MessageBox.Show(@"Too many failed attempts. Aborting.");
                return new Tuple<DialogResult, string, string, string, string>(DialogResult.Abort, gameName, gamePath,
                    acfPath, branchName);
            }
            DialogResult result = DialogResult.OK;
            try
            {
                if (!DeleteGameBranch(gameName, gamePath, acfPath, branchName, worker))
                    throw new Exception("CreateBranch returned false");
            }
            catch (Exception exception)
            {
                result = MessageBox.Show(
                    $@"Failed to delete branch {branchName} of {gameName} from {GetBranchPath(gamePath, gameName, branchName)}: 
                    {exception}", @"ERROR",
                    MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
                switch (result)
                {
                    case DialogResult.Abort:
                        result = DialogResult.Abort;
                        break;
                    case DialogResult.Ignore:
                        result = DialogResult.OK;
                        break;
                    case DialogResult.Retry:
                        result = TryDeleteBranch(gameName, gamePath, acfPath, branchName, worker, depth + 1).Item1;
                        break;
                }
            }
            worker.ReportProgress(100, result == DialogResult.OK ? $"Deleting {branchName} Complete" : "Failed");
            return new Tuple<DialogResult, string, string, string, string>(result, gameName, gamePath, acfPath,
                branchName);
        }

        public static Tuple<DialogResult, string, string, string, string[]> TryDeleteGame(string gameName,
            string gamePath, string acfPath, string[] branches, BackgroundWorker worker, int depth = 0)
        {
            DialogResult result = DialogResult.OK;
            foreach (var branch in branches)
            {
                result = TryDeleteBranch(gameName, gamePath, acfPath, branch, worker).Item1;
                if (result != DialogResult.OK)
                    break;
            }
            var gameBranchesPath = GetGameBranchesPath(gamePath, gameName);
            DirectoryInfo branchFolderInfo = new DirectoryInfo(gameBranchesPath);
            if (result == DialogResult.OK && branchFolderInfo.Exists && branchFolderInfo.GetDirectories().Length == 0)
                branchFolderInfo.Delete();
            worker.ReportProgress(100, result == DialogResult.OK ? $"Deleting {gameName} Complete" : "Failed");
            return new Tuple<DialogResult, string, string, string, string[]>(result, gameName, gamePath, acfPath,
                branches);
        }

        private static bool DeleteGameBranch(string gameName, string gamePath, string acfPath, string branchName,
            BackgroundWorker worker)
        {
            var branchPath = GetBranchPath(gamePath, gameName, branchName);
            if (!Directory.Exists(branchPath))
                return true;
            if (branchName == "original")
            {
                FileInfo acfPathInfo = new FileInfo(acfPath);
                var acfTarget = Path.Combine(branchPath, Path.GetFileName(acfPath));
                if (File.Exists(acfTarget) &&
                    (acfPathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint) || acfPathInfo.Exists))
                {
                    acfPathInfo.Delete();
                    File.Move(acfTarget, acfPath);
                }
                DirectoryInfo gamePathInfo = new DirectoryInfo(Path.GetDirectoryName(branchPath));
                if (Directory.Exists(branchPath) &&
                    gamePathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                {
                    gamePathInfo.Delete();
                    Directory.Move(branchPath, Path.GetDirectoryName(branchPath));
                }
            }
            else
            {
                DeleteDirectory(branchPath, worker);
            }
            return !Directory.Exists(branchPath);
        }

        private static void DeleteDirectory(string sourceDir, BackgroundWorker worker)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists)
                return;
            var dirs = dir.GetDirectories();
            foreach (DirectoryInfo subDir in dirs) DeleteDirectory(subDir.FullName, worker);
            foreach (FileInfo file in dir.GetFiles())
            {
                worker.ReportProgress(0, file.Name);
                file.Delete();
            }
            dir.Delete();
        }

        private enum SymbolicLink
        {
            File = 0,
            Directory = 1
        }

        // Add a static reference to SaveData for lookup
        public static SaveData SaveDataInstance;
    }
}