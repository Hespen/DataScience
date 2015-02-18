using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashMap
{
    class DataProcessor
    {

        private readonly String filePath = @"../../userItem.data";
        private Dictionary<int, UserPreference> map; 

        public DataProcessor()
        {
            map = new Dictionary<int, UserPreference>();
        }

        public Dictionary<int, UserPreference> readDataFromFile()
        {
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        processLine(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
            return map;
        }

        private void processLine(String line)
        {
            String[] data = line.Split(',');
            UserPreference currentPreference;
            int userID = Convert.ToInt16(data[0]);
            int articleID = Convert.ToInt16(data[1]);
            float rating = float.Parse(data[2], CultureInfo.InvariantCulture.NumberFormat);
            if (!map.ContainsKey(userID))
            {
                currentPreference = new UserPreference();
                map.Add(userID, currentPreference);
            }
            else
            {
                currentPreference = map[userID];
            }
            currentPreference.AddNewRecord(articleID, rating);
        } 
    }
}
