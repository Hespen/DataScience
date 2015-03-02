using System.Collections.Generic;

namespace HashMap
{
    internal class Program
    {
        private static Dictionary<int, UserPreference> map;

        private static void Main(string[] args)
        {
            var rManager = new RecommendationManager();
            rManager.StartDataRead();
        }
    }
}