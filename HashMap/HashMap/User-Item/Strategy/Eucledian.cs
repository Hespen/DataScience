using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HashMap.Strategy
{
    internal class Eucledian : ICalculator
    {
        /// <summary>
        ///     Distance of users vs the target users
        /// </summary>
        private List<KeyValuePair<int, double>> _distances;


        public Dictionary<int,double> Execute(Dictionary<int, UserPreference> userRatings, int target)
        {
            UserPreference targetRatings = userRatings[target];
            var neighbours = new Dictionary<int, double>();
            var nearestNeighbours = new Dictionary<int, double>();
            int n = 0;

            Console.WriteLine("=======Euclidian========");

            // Loop through all users except for the target user
            foreach (var userRating in userRatings)
            {
                if (userRating.Value == targetRatings) continue;
                double similarity = calculateSimilarities(targetRatings, userRating);
                Console.WriteLine(similarity);
                neighbours.Add(userRating.Key, similarity);
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
        ///     Calculates similarities for all similar products for two users
        /// </summary>
        /// <param name="targetRatings">Ratings of the target user that needs similarity data</param>
        /// <param name="userRating">Ratings of the user tho whom the target users ratings are compared</param>
        /// <returns>Similarity value</returns>
        private double calculateSimilarities(UserPreference targetRatings, KeyValuePair<int, UserPreference> userRating)
        {
            double userDifference = 0;

            // Loop through all product ratings of the target user
            foreach (var targetRating in targetRatings.GetRatings())
            {
                // Get the rating of the other user
                float rating = userRating.Value.GetRating(targetRating.Key);

                if (rating > 0)
                {
                    userDifference += Math.Pow(targetRating.Value - rating, 2);
                }
            }
            double distance = Math.Sqrt(userDifference);
            return 1/(1 + distance);
        }
    }
}