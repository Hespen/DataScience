using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashMap
{
    class RecommendationManager
    {
        public RecommendationManager(){}

        public void startDataRead()
        {
            DataProcessor processor = new DataProcessor();
            Dictionary<int, UserPreference> result = processor.readDataFromFile();
            Console.ReadKey();
        }
    }
}
