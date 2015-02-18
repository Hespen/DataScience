using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashMap
{
    class Program
    {
        private static Dictionary<int, UserPreference> map;
        static void Main(string[] args)
        {
            RecommendationManager rManager = new RecommendationManager();
            rManager.startDataRead();
        }
    }
}
