using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HashMap
{
    class ItemItemRatingPredictor
    {
        private int _targetUser;
        private readonly DataTable _deviations;
        private DataRow _targetUserRow;
        private string[] _columnNames;
        private List<Tuple<int, int, double>> _predictedRatings; 

        public ItemItemRatingPredictor(DataTable deviations)
        {
            _deviations = deviations;
        }

        public void Execute()
        {
            List<int> notRatedArticles = GetNotRatedArticles();
            _predictedRatings = new List<Tuple<int, int, double>>();

            foreach (var notRatedArticle in notRatedArticles)
            {
                double predictedRating = CalculatePredictedRating(notRatedArticle);

                if (Math.Abs(predictedRating) > 0)
                {
                    _predictedRatings.Add(new Tuple<int, int, double>(_targetUser, notRatedArticle, predictedRating));
                }
            }

            _predictedRatings = _predictedRatings.OrderByDescending(tuple => tuple.Item3).ToList();
            PrintResults(_predictedRatings);
        }

        public void SetUser(int targetUser)
        {
            _targetUser = targetUser;
        }

        /// <summary>
        /// Formula to predict rating = 𝒑(𝒖,𝒊) = 𝑗∈ratings(𝑢) (𝑢𝑗 + 𝑑𝑒𝑣𝑖,𝑗) * card (𝑆𝑖,𝑗) /  𝑗∈ratings(𝑢) card(𝑆𝑖,𝑗)
        /// </summary>
        /// <param name="articleId">The article to get a predicted rating for</param>
        /// <returns>The predicted rating for the given article id</returns>
        private double CalculatePredictedRating(int articleId)
        {
            DataRow targetDeviationRow = _deviations.AsEnumerable().Where(s => s.Field<string>(0) == articleId.ToString()).ToList()[0];
            double numerator = 0;
            double denominator = 0;
            double predictedRating = -1;

            // Loop over all ratings from the target user
            for (int i = 1; i < DataTableProcessor.UserRatings.Columns.Count; i++)
            {
                string rating = _targetUserRow[i].ToString();

                if (!rating.Equals(articleId.ToString()) && !rating.Equals(""))
                {
                    // Contains deviation AND denominator
                    string[] deviationAndDenominator = targetDeviationRow[i].ToString().Split(';');

                    double deviation = Convert.ToDouble(deviationAndDenominator[0]);
                    double deviationDenominator = Convert.ToDouble(deviationAndDenominator[1]);

                    //(𝑢𝑗 + 𝑑𝑒𝑣𝑖,𝑗)
                    numerator += (float.Parse(rating, CultureInfo.InvariantCulture.NumberFormat) + deviation) * deviationDenominator;
                    //card (𝑆𝑖,𝑗)
                    denominator += deviationDenominator;
                }
            }

            if (Math.Abs(denominator) > 0)
            {
                predictedRating = numerator/denominator;
            }

            return predictedRating;
        }

        /// <summary>
        /// This method checks what articles the target user has not yet rated.
        /// </summary>
        /// <returns>A list with article ids the target user has not yet rated.</returns>
        private List<int> GetNotRatedArticles()
        {
            List<int> notRatedArticles = new List<int>();

            List<DataRow> drList = DataTableProcessor.UserRatings.AsEnumerable().Where(s => s.Field<string>(0) == _targetUser.ToString()).ToList();
            _columnNames = (from dc in DataTableProcessor.UserRatings.Columns.Cast<DataColumn>()
                        select dc.ColumnName).ToArray();


            _targetUserRow = drList[0];
            foreach (var columnName in _columnNames)
            {
                string value = _targetUserRow[columnName].ToString();
                if (value.Equals(""))
                {
                    notRatedArticles.Add(Convert.ToInt32(columnName));
                }
            }
            
            return notRatedArticles;
        }

        private void PrintResults(List<Tuple<int, int, double>> predictedRatings)
        {
            Console.WriteLine("Predicted ratings: \n");
            foreach (var predictedRating in predictedRatings)
            {
                Console.WriteLine("User ID: " + predictedRating.Item1 + "\t - Article: " + predictedRating.Item2 + "\t - Predicted Rating: " + predictedRating.Item3);
            }
            Console.WriteLine("\n");
        }

    }
}
