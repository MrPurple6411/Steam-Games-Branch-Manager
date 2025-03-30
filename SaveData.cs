using System.Collections.Generic;

namespace Steam_Games_Branch_Manager
{
    public class SaveData
    {
        public Dictionary<string, string> Games { get; set; } = new();
        public Dictionary<string, string> GamesACFPath { get; set; } = new();
        public Dictionary<string, List<string>> Branches { get; set; } = new();
        public Dictionary<string, string> SelectedBranches { get; set; } = new();
        public string SelectedGame { get; set; } = string.Empty;
    }
}
