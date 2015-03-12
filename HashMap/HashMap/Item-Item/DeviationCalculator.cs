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

    }
}
