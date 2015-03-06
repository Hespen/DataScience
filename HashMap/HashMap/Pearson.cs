using System;
using System.Collections.Generic;
using System.Linq;

namespace HashMap
{
    internal class Pearson : ICalculator
    {
        public Dictionary<int,double> Execute(Dictionary<int, UserPreference> userRatings, int target)
        {
            UserPreference targetUserPreferences = userRatings[target];
            var neighbours = new Dictionary<int, double>();
            var nearestNeighbours = new Dictionary<int, double>();
            int n = 0;

            Console.WriteLine("=======Pearson========");

            foreach (var userPreference in userRatings)
            {
                if (userPreference.Value == targetUserPreferences) continue;
                double similarity = CalculateSimilarities(targetUserPreferences, userPreference);
                neighbours.Add(userPreference.Key, similarity);
            }
            //Loop through similarities, ordered by descending value
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
        ///     Pearson formula is:
        ///     r = (∑XiYi - (∑Xi*∑Yi)/n) / ( SQRT(∑Xi^2 - ((∑Xi)^2) / n) * SQRT(∑Yi^2 - ((∑Yi)^2) / n) )
        /// </summary>
        /// <param name="targetRatings"></param>
        /// <param name="userRatings"></param>
        /// <returns>
        ///     The Pearson coefficient of the two users. Range of -1 to 1
        /// </returns>
        private double CalculateSimilarities(UserPreference targetRatings,
            KeyValuePair<int, UserPreference> userRatings)
        {
            //The final formula will be created using these values:
            //The sum of all values of the Target
            double Xi = 0;
            //The sum of all the values to the power of 2 of the Target
            double Xi2 = 0;
            //The sum of all values of the Other user
            double Yi = 0;
            //The sum of all the values to the power of 2 from the other user
            double Yi2 = 0;
            //The sum from all the ratings from the target and other user
            double XiYi = 0;
            //Amount of the same products rated by both users
            int n = 0;
            foreach (var targetRating in targetRatings.GetRatings())
            {
                //Target will be defined as Xi
                float tarRating = targetRating.Value;

                //Rating of other user for product X
                //userRating will be defined as Yi
                float userRating = userRatings.Value.GetRating(targetRating.Key);
                if (userRating > 0)
                {
                    //If the other user has rated the same product as the target, we will add the rating to all variables.
                    Xi += tarRating;
                    Yi += userRating;
                    Xi2 += Math.Pow(tarRating, 2);
                    Yi2 += Math.Pow(userRating, 2);
                    XiYi += tarRating*userRating;
                    n++;
                }
            }
            //Calculate the pc
            double pearsonCoefficient = (XiYi - ((Xi*Yi)/n))/
                                        (Math.Sqrt(Xi2 - (Math.Pow(Xi, 2))/n)*Math.Sqrt(Yi2 - (Math.Pow(Yi, 2))/n));
            Console.WriteLine(pearsonCoefficient);
            return pearsonCoefficient;
        }
    }
}