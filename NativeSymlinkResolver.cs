using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Steam_Games_Branch_Manager
{
    internal static class NativeSymlinkResolver
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern uint GetFinalPathNameByHandle(
            IntPtr hFile,
            [Out] char[] lpszFilePath,
            uint cchFilePath,
            uint dwFlags);

        [DllImport("kernel32.dll")] 
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        private const uint FILE_READ_EA = 0x0008;
        private const uint FILE_SHARE_READ = 0x00000001;
        private const uint FILE_SHARE_WRITE = 0x00000002;
        private const uint OPEN_EXISTING = 3;
        private const uint FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;
        private const uint VOLUME_NAME_DOS = 0x0;

        public static string ResolveSymlink(string path)
        {
            IntPtr h = CreateFile(path, FILE_READ_EA, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_FLAG_BACKUP_SEMANTICS, IntPtr.Zero);
            if (h == IntPtr.Zero || h == new IntPtr(-1))
                return null;
            try
            {
                char[] buffer = new char[512];
                uint result = GetFinalPathNameByHandle(h, buffer, (uint)buffer.Length, VOLUME_NAME_DOS);
                if (result == 0)
                    return null;
                var finalPath = new string(buffer, 0, (int)result);
                // Remove \\?\ prefix if present
                if (finalPath.StartsWith(@"\\?\"))
                    finalPath = finalPath.Substring(4);
                return finalPath;
            }
            finally
            {
                CloseHandle(h);
            }
        }
    }
}
