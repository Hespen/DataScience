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
        private readonly DataTable _userRatings;
        private readonly UserPreference _targetRatings;
        private DataTable _deviations = new DataTable();

        public DeviationCalculator(DataTable userRatings)
        {
            _userRatings = userRatings;
        }

        /// <summary>
        /// Formula for calculating deviations: 𝒅𝒆𝒗𝒊,𝒋 = 𝑢∈𝑆𝑖,𝑗 (𝑢𝑖 − 𝑢𝑗) / card(𝑆𝑖,𝑗)
        /// </summary>
        public void Execute()
        {
            AddColumns();

            // The articleId used in this loop represents the left column (u∈𝑆i)
            foreach (var articleId in DataTableProcessor.ArticleIds)
            {
                List<string> leftColumn = _userRatings.AsEnumerable().Select(s => s.Field<string>(articleId.ToString())).ToList();
                DataRow dr = _deviations.NewRow();
                dr[0] = articleId;

                // The articleId used in this loop represents the right column (u∈𝑆j)
                foreach (var id in DataTableProcessor.ArticleIds)
                {
                    double deviation = 0;

                    if (articleId == id) continue;
                    List<string> rightColumn = _userRatings.AsEnumerable().Select(s => s.Field<string>(id.ToString())).ToList();

                    dr[id.ToString()] = CalculateDeviation(leftColumn, rightColumn);
                }
                _deviations.Rows.Add(dr);
            }
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
        private Tuple<string, string> CalculateDeviation(List<string> leftColumn, List<string> rightColumn)
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
            return new Tuple<string, string>(deviation.ToString(), denominator.ToString());
        }

        /// <summary>
        /// This method checks what articles the target user has not yet rated.
        /// </summary>
        /// <returns>A list with article ids the target user has not yet rated.</returns>
        private List<int> GetNotRatedArticles()
        {
            // This HashSet contains all articles present in the data set. After execution of this method, it 
            // contains only article ids the target user has not yet rated.
            HashSet<int> articleIds = DataTableProcessor.ArticleIds;

            var ratedArticles = _targetRatings.GetRatings().Keys.ToList();

            foreach (var ratedArticle in ratedArticles)
            {
                articleIds.Remove(ratedArticle);
            }

            return articleIds.ToList();
        }

    }
}
