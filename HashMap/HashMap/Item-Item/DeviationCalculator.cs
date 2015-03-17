using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashMap
{
    class DeviationCalculator
    {
        private DataTable _deviations = new DataTable();

        public DeviationCalculator()
        {
        }

        /// <summary>
        /// Formula for calculating deviations: 𝒅𝒆𝒗𝒊,𝒋 = 𝑢∈𝑆𝑖,𝑗 (𝑢𝑖 − 𝑢𝑗) / card(𝑆𝑖,𝑗)
        /// </summary>
        /// <returns>A DataTable which contains all deviations between articles</returns>
        public DataTable Execute()
        {
            AddColumns();

            // The articleId used in this loop represents the left column (u∈𝑆i)
            foreach (var articleId in DataTableProcessor.ArticleIds)
            {
                List<string> leftColumn = DataTableProcessor.UserRatings.AsEnumerable().Select(s => s.Field<string>(articleId.ToString())).ToList();
                DataRow dr = _deviations.NewRow();
                dr[0] = articleId;

                // The articleId used in this loop represents the right column (u∈𝑆j)
                foreach (var id in DataTableProcessor.ArticleIds)
                {
                    double deviation = 0;

                    if (articleId == id) continue;
                    List<string> rightColumn = DataTableProcessor.UserRatings.AsEnumerable().Select(s => s.Field<string>(id.ToString())).ToList();

                    dr[id.ToString()] = CalculateDeviation(leftColumn, rightColumn);
                }
                _deviations.Rows.Add(dr);
            }
            return _deviations;
        }

        private void AddColumns()
        {
            _deviations.Columns.Add("Deviations");
            foreach (var articleId in DataTableProcessor.ArticleIds)
            {
                _deviations.Columns.Add(articleId.ToString());
            }
        }

        // Loop through all elements in both lists. If both elements have a value/rating, 
        // subtract both values and add it to the numerator.
        private string CalculateDeviation(List<string> leftColumn, List<string> rightColumn)
        {
            double numerator = 0;
            int denominator = 0;

            for (int i = 0; i < leftColumn.Count; i++)
            {
                if (leftColumn[i] != null && rightColumn[i] != null)
                {
                    float ui = float.Parse(leftColumn[i], CultureInfo.InvariantCulture.NumberFormat);
                    float uj = float.Parse(rightColumn[i], CultureInfo.InvariantCulture.NumberFormat);

                    numerator += (ui - uj);
                    denominator ++;
                }
            }
            double deviation = numerator/denominator;

            // Both deviation AND denominator need to be saved in the cell.
            return deviation + ";" + denominator;
        }

        public void InsertRating(int userId, int articleId, double rating)
        {
            DataRow userRatingsRow = DataTableProcessor.UserRatings.AsEnumerable().Where(s => s.Field<string>(0) == userId.ToString()).ToList()[0];
            userRatingsRow[articleId.ToString()] = rating.ToString();

            UpdateDeviations(articleId, rating, userRatingsRow);
        }

        /// <summary>
        /// formula for updating deviation : 𝑑𝑒𝑣𝐴,𝐵′ = (𝑑𝑒𝑣𝐴,𝐵 × 𝑛) + (𝑟𝐴 − 𝑟𝐵) / 𝑛 + 1
        /// </summary>
        /// <param name="articleId"></param>
        private void UpdateDeviations(int articleId, double rating, DataRow userRatingsRow)
        {
            DataRow articleDeviationsRow = _deviations.AsEnumerable().Where(s => s.Field<string>(0) == articleId.ToString()).ToList()[0];

            for (int i = 1; i < _deviations.Columns.Count; i++)
            {
                double numerator;
                double newDeviation;
                int oldDenominator;

                Console.WriteLine(i);

                string selectedDeviation = articleDeviationsRow[i].ToString();
                if (selectedDeviation.Equals("") || userRatingsRow[i].ToString().Equals("")) continue;
                string[] deviationAndDenominator = selectedDeviation.Split(';');

                oldDenominator = Convert.ToInt32(deviationAndDenominator[1]);

                // devABN = (𝑑𝑒𝑣𝐴,𝐵 × 𝑛)
                double devABN = Convert.ToDouble(deviationAndDenominator[0])*oldDenominator;

                float targetRating = float.Parse(userRatingsRow[i].ToString(), CultureInfo.InvariantCulture.NumberFormat);

                // devABN + (𝑟𝐴 − 𝑟𝐵)
                numerator = devABN + (rating - targetRating);

                newDeviation = numerator/(oldDenominator + 1);

                articleDeviationsRow[i] = newDeviation + ";" + (oldDenominator + 1);

                _deviations.Rows[i-1][articleId.ToString()] = (-1 * newDeviation) + ";" + (oldDenominator + 1);
            }
        }

    }
}
