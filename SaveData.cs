using System.Collections.Generic;

namespace Steam_Games_Branch_Manager
{
    internal class SaveData
    {
        public string SelectedGame = string.Empty;
        public Dictionary<string, string> SelectedBranches = new Dictionary<string, string>();
        public Dictionary<string, string> Games { get; } = new Dictionary<string, string>();
        public Dictionary<string, string> GamesACFPath { get; } = new Dictionary<string, string>();
        public Dictionary<string, string> GamesInstallDir { get; } = new Dictionary<string, string>();
        public Dictionary<string, List<string>> Branches { get; } = new Dictionary<string, List<string>>();
    }
}