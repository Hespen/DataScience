using System;
using System.Collections.Generic;
using System.Linq;

namespace HashMap.Strategy
{
    internal class Cosine : ICalculator
    {
        public Dictionary<int,double> Execute(Dictionary<int, UserPreference> userRatings, int target)
        {
            UserPreference targetUserPreferences = userRatings[target];
            var neighbours = new Dictionary<int, double>();
            var nearestNeighbours = new Dictionary<int, double>();
            int n = 0;


            Console.WriteLine("=======Cosine=========");

            foreach (var userPreference in userRatings)
            {
                if (userPreference.Value == targetUserPreferences) continue;
                double similarity = CalculateSimilarities(targetUserPreferences, userPreference);
                neighbours.Add(userPreference.Key, similarity);
            }
            //Loop through all similarities, order descending.
            foreach (var neighbour in neighbours.OrderByDescending(key => key.Value))
            {
                if (neighbour.Value < 0.35 || n >= Constants.NearestNeighbours) continue;
                nearestNeighbours.Add(neighbour.Key, neighbour.Value);
                Console.WriteLine("User " + neighbour.Key + " with a value of " + neighbour.Value);
                n++;
            }
            return nearestNeighbours;
        }
        /// <summary>
        /// cos(x,y) = (x.y)/||v||
        /// </summary>
        /// <param name="targetUserPreferences">User preference class of the user for whom we calculate similarties for</param>
        /// <param name="userPreference">User preference class of the user where we compare the target user to.</param>
        /// <returns></returns>
        private double CalculateSimilarities(UserPreference targetUserPreferences,
            KeyValuePair<int, UserPreference> userPreference)
        {
            double cosineSimilarity = 0;
            double Xi = 0;
            double Yi = 0;
            double XiYi = 0;
            
            //Check the ratings for all items, even if they are 0
            foreach (var articleId in DataProcessor.ArticleIds)
            {
                float tarRating = targetUserPreferences.GetRating(articleId);
                float userRating = userPreference.Value.GetRating(articleId);
                Xi += Math.Pow(tarRating, 2);
                Yi += Math.Pow(userRating, 2);
                XiYi += tarRating*userRating;
            }
            cosineSimilarity = XiYi/(Math.Sqrt(Xi)*Math.Sqrt(Yi));
            Console.WriteLine(cosineSimilarity);
            return cosineSimilarity;
        }
    }
}