using System;
using System.IO;
using System.Diagnostics;

namespace Steam_Games_Branch_Manager
{
    internal static class AppLogger
    {
        private static readonly string LogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.log");

        public static void Log(string message)
        {
            string logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            try
            {
                File.AppendAllText(LogPath, logLine + Environment.NewLine);
            }
            catch { }
            Debug.WriteLine(logLine);
        }
    }
}
