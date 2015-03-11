using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationsC
{
    class Percentage : ICalculator
    {
        private List<Tuple<string, double>> percentages = new List<Tuple<string, double>>();

        public List<Tuple<string, double>> Execute(System.Data.DataTable ratingMatrix)
        {
            Console.WriteLine("Percentages: \n");

            for (int i = 1; i < ratingMatrix.Columns.Count; i++)
            {
                string header = ratingMatrix.Columns[i].ColumnName;

                // Select all values for column `i`
                string[] ratings = ratingMatrix.AsEnumerable().Select(s => s.Field<string>(i)).ToArray<string>();

                double numberOfRaters = 0;
                double numberOfFourPlusRatings = 0;
                foreach (var rating in ratings)
                {
                    if (!rating.Equals(""))
                    {
                        int iRating = Convert.ToInt32(rating);
                        if (iRating > 4)
                        {
                            numberOfFourPlusRatings ++;
                        }

                        numberOfRaters++;
                    }
                }

                double percentage = (numberOfFourPlusRatings/numberOfRaters)*100;
                
                percentages.Add(new Tuple<string, double>(header, percentage));
                
            }
            percentages = percentages.OrderByDescending(tuple => tuple.Item2).ToList().Take(5).ToList();
            return percentages;
        }
    }
}