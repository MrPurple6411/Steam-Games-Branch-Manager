using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Steam_Games_Branch_Manager
{
    internal static class BranchHandler
    {
        internal static Tuple<DialogResult, string, string, string, string> TryCreateBranch(string gameName,
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
                if (!CreateGameBranch(gamePath, acfPath, branchName, worker))
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

            worker.ReportProgress(100, result == DialogResult.OK ? $"Creating {branchName} Complete" : "Failed");
            return new Tuple<DialogResult, string, string, string, string>(result, gameName, gamePath, acfPath,
                branchName);
        }

        [DllImport("kernel32.dll")]
        private static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName,
            SymbolicLink dwFlags);


        
        
        private static bool CreateGameBranch(string gamePath, string acfPath, string branchName,
            BackgroundWorker worker)
        {
            if (!Directory.Exists($"{gamePath}Branches"))
                Directory.CreateDirectory($"{gamePath}Branches");

            if (Directory.Exists($"{gamePath}Branches/{branchName}"))
                throw new Exception(
                    $"{gamePath}Branches/{branchName}\nAlready Exists!\nCannot overwrite existing folders!");

            if (branchName != "original" && !Directory.Exists($"{gamePath}Branches/original"))
                throw new Exception($"Tried to create a branch but could not find \n{gamePath}Branches/original");

            if (branchName != "original" && !File.Exists($"{gamePath}Branches/original/{Path.GetFileName(acfPath)}"))
                throw new Exception(
                    $"original Acf file not found at {gamePath}Branches/original/{Path.GetFileName(acfPath)}.original");

            if (branchName == "original")
            {
                Directory.Move(gamePath, $"{gamePath}Branches/{branchName}");
                File.Move(acfPath, $"{gamePath}Branches/{branchName}/{Path.GetFileName(acfPath)}");

                CreateSymbolicLink(gamePath, $"{gamePath}Branches/{branchName}", SymbolicLink.Directory);
                CreateSymbolicLink(acfPath, $"{gamePath}Branches/{branchName}/{Path.GetFileName(acfPath)}",
                    SymbolicLink.File);

                FileInfo pathInfo = new FileInfo(acfPath);
                var success = pathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint);

                if (success)
                    return true;

                if (File.Exists($"{gamePath}Branches/{branchName}/{Path.GetFileName(acfPath)}"))
                    File.Move($"{gamePath}Branches/{branchName}/{Path.GetFileName(acfPath)}", acfPath);
                if (Directory.Exists($"{gamePath}Branches/{branchName}"))
                    Directory.Move($"{gamePath}Branches/{branchName}", gamePath);

                return false;
            }

            CopyDirectory($"{gamePath}Branches/original", $"{gamePath}Branches/{branchName}", true, false, worker);
            return Directory.Exists($"{gamePath}Branches/{branchName}") &&
                   File.Exists($"{gamePath}Branches/{branchName}/{Path.GetFileName(acfPath)}");
        }

        internal static void SetActiveBranch(string gamePath, string acfPath, string branchName)
        {
            FileInfo acfPathInfo = new FileInfo(acfPath);
            if (acfPathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                File.Delete(acfPath);
            
            DirectoryInfo gamePathInfo = new DirectoryInfo(gamePath);
            if (gamePathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                gamePathInfo.Delete();
            
            CreateSymbolicLink(acfPath, $"{gamePath}Branches/{branchName}/{Path.GetFileName(acfPath)}", SymbolicLink.File);
            CreateSymbolicLink(gamePath, $"{gamePath}Branches/{branchName}", SymbolicLink.Directory);
        }
        private static void CopyDirectory(string sourceDir, string destinationDir, bool recursive, bool deep,
            BackgroundWorker worker)
        {
            // Get information about the source directory
            DirectoryInfo dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            var dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            if (recursive && deep)
                // If recursive and copying subdirectories, directory structure fist then files
                foreach (DirectoryInfo subDir in dirs)
                {
                    var newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true, true, worker);
                }

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                var targetFilePath = Path.Combine(destinationDir, file.Name);
                worker.ReportProgress(0, file.Name);
                file.CopyTo(targetFilePath);
            }

            if (recursive && !deep)
                // If recursive and copying subdirectories, all files first then each sub directory
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
                if (!DeleteGameBranch(gamePath, acfPath, branchName, worker))
                    throw new Exception("CreateBranch returned false");
            }
            catch (Exception exception)
            {
                result = MessageBox.Show(
                    $@"Failed to delete branch {branchName} of {gameName} from {gamePath}Branches\{branchName}: 
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

            DirectoryInfo branchFolderInfo = new DirectoryInfo($"{gamePath}Branches");
            if (result == DialogResult.OK && branchFolderInfo.Exists && branchFolderInfo.GetDirectories().Length == 0)
                branchFolderInfo.Delete();

            worker.ReportProgress(100, result == DialogResult.OK ? $"Deleting {gameName} Complete" : "Failed");
            return new Tuple<DialogResult, string, string, string, string[]>(result, gameName, gamePath, acfPath,
                branches);
        }

        private static bool DeleteGameBranch(string gamePath, string acfPath, string branchName,
            BackgroundWorker worker)
        {
            if (!Directory.Exists($"{gamePath}Branches"))
                return true;

            if (!Directory.Exists($"{gamePath}Branches/{branchName}"))
                return true;

            if (branchName == "original")
            {
                FileInfo acfPathInfo = new FileInfo(acfPath);
                if (acfPathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                    File.Delete(acfPath);

                if (File.Exists($"{gamePath}Branches/{branchName}/{Path.GetFileName(acfPath)}"))
                    File.Move($"{gamePath}Branches/{branchName}/{Path.GetFileName(acfPath)}", acfPath);

                DirectoryInfo gamePathInfo = new DirectoryInfo(gamePath);
                if (gamePathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                    gamePathInfo.Delete();

                if (Directory.Exists($"{gamePath}Branches/{branchName}"))
                    Directory.Move($"{gamePath}Branches/{branchName}", gamePath);
            }
            else
            {
                DeleteDirectory($"{gamePath}Branches/{branchName}", worker);
            }

            return !Directory.Exists($"{gamePath}Branches/{branchName}");
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
    }
}