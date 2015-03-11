using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationsC
{
    class DataProcessor
    {
        private readonly String filePath = @"../../RatingMatrix.csv";
        private DataTable ratingMatrix = new DataTable();

        public DataTable ReadDataFromFile()
        {
            try
            {
                using (var sr = new StreamReader(filePath))
                {
                    String line;

                    // Add columns first
                    line = sr.ReadLine();
                    line = line.Replace("\"", "");
                    var columns = line.Split(';');
                    foreach (string column in columns)
                    {
                        ratingMatrix.Columns.Add(column);
                    }

                    while ((line = sr.ReadLine()) != null)
                    {
                        ProcessLine(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }

            return ratingMatrix;
        }

        private void ProcessLine(string line)
        {
            DataRow dr = ratingMatrix.NewRow();

            line = line.Replace("\"", "");
            var values = line.Split(';');
            for (int i=0; i < values.Length; i++)
            {
                dr[i] = values[i];
            }

            ratingMatrix.Rows.Add(dr);
        }

    }
}
