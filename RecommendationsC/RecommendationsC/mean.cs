using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationsC
{
    class Mean : ICalculator
    {
        private List<Tuple<string, double>> means = new List<Tuple<string, double>>();  
 
        public Mean()
        {
        }

        /// <summary>
        /// This execute methods calculates the average ratings for every movie.
        /// </summary>
        /// <param name="ratingMatrix">The DataTable which contains all the ratings for each movie.</param>
        /// <returns>A List of tuples. Each tuple contains a movie name with its mean. Only the top 5 means are returned</returns>
        public List<Tuple<string, double>> Execute(DataTable ratingMatrix)
        {
            Console.WriteLine("Mean: \n");

            for (int i=1; i < ratingMatrix.Columns.Count; i++)
            {
                string header = ratingMatrix.Columns[i].ColumnName;

                // Select all values for column `i`
                string[] ratings = ratingMatrix.AsEnumerable().Select(s => s.Field<string>(i)).ToArray<string>(); 

                double totalRating = 0;
                int numberOfRaters = 0;
                foreach (var rating in ratings)
                {
                    if (!rating.Equals(""))
                    {
                        totalRating += Convert.ToInt32(rating);
                        numberOfRaters++;
                    }
                }
                var mean = totalRating / numberOfRaters;
                
                means.Add(new Tuple<string, double>(header, mean));
            }
            means = means.OrderByDescending(tuple => tuple.Item2).ToList().Take(5).ToList();
            return means;
        }
    }
}
