using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashMap
{
    class DataTableProcessor
    {
        private readonly String filePath = @"../../movielens.data";
        private readonly Dictionary<int, UserPreference> _map;
        public static HashSet<int> ArticleIds;
        private DataTable _userRatingsTemp;
        public static DataTable UserRatings;

        public DataTableProcessor()
        {
            _map = new Dictionary<int, UserPreference>();
            ArticleIds = new HashSet<int>();

            _userRatingsTemp = new DataTable();
            _userRatingsTemp.Columns.Add("UserId");
            _userRatingsTemp.Columns.Add("ArticleId");
            _userRatingsTemp.Columns.Add("Rating");
        }

        public DataTable ReadDataFromFile()
        {
            try
            {
                using (var sr = new StreamReader(filePath))
                {
                    String line;
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

            return ConvertDataTable();
        }

        private void ProcessLine(String line)
        {
            string[] lines = line.Split('\t');
            DataRow dr = _userRatingsTemp.NewRow();
            dr[0] = lines[0];
            dr[1] = lines[1];
            dr[2] = lines[2];
            _userRatingsTemp.Rows.Add(dr);
        }

        private DataTable ConvertDataTable()
        {
            UserRatings = new DataTable();
            UserRatings.Columns.Add("UserId");

            // Get all distinct article ids, put them in a list and sort them.
            List<string> articleIds= _userRatingsTemp.AsEnumerable().Select(s => s.Field<string>(1)).ToArray<string>().Distinct().ToList();
            articleIds.Sort();

            foreach (var articleId in articleIds)
            {
                UserRatings.Columns.Add(articleId);
                ArticleIds.Add(Convert.ToInt32(articleId));
            }

            List<string> userIds = _userRatingsTemp.AsEnumerable().Select(s => s.Field<string>(0)).ToArray<string>().Distinct().ToList();
            userIds.Sort();

            foreach (var userId in userIds)
            {
                List<DataRow> rows = _userRatingsTemp.AsEnumerable().Where(s => s.Field<string>(0) == userId).ToList();
                DataRow dr = UserRatings.NewRow();

                dr[0] = userId;

                foreach (var dataRow in rows)
                {
                    dr[dataRow[1].ToString()] = dataRow[2];
                }

                UserRatings.Rows.Add(dr);
            }
            
            return UserRatings;

        }
    }
}