using System;
using System.Collections.Generic;
using System.Linq;

namespace HashMap
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
                if (neighbour.Value < 0.35 || n >= 3) continue;
                nearestNeighbours.Add(neighbour.Key, neighbour.Value);
                Console.WriteLine("User " + neighbour.Key + " with a value of " + neighbour.Value);
                n++;
            }
            return nearestNeighbours;
        }

        private double CalculateSimilarities(UserPreference targetUserPreferences,
            KeyValuePair<int, UserPreference> userPreference)
        {
            double cosineSimilarity = 0;
            double Xi = 0;
            double Yi = 0;
            double XiYi = 0;
            foreach (var rating in targetUserPreferences.GetRatings())
            {
                float tarRating = rating.Value;
                float userRating = userPreference.Value.GetRating(rating.Key);
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