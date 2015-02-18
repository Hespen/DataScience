using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashMap
{
    class Eucledian : ICalculator
    {   
        /// <summary>
        /// Distance of users vs the target users
        /// </summary>
        private List<KeyValuePair<int, double>> distances;

        public Eucledian()
        {

        }


        public void Execute(Dictionary<int, UserPreference> userRatings, int target)
        {  
            UserPreference targetRatings = userRatings[target];
            userRatings.Remove(target);

            // Loop through all users except for the target user
            foreach(KeyValuePair<int, UserPreference> userRating in userRatings){
                double similarity = calculateSimilarities(targetRatings, userRating);
            }
        }

        /// <summary>
        /// Calculates similarities for all similar products for two users
        /// </summary>
        /// <param name="targetRatings">Ratings of the target user that needs similarity data</param>
        /// <param name="userRating">Ratings of the user tho whom the target users ratings are compared</param>
        /// <returns>Similarity value</returns>
        private double calculateSimilarities(UserPreference targetRatings, KeyValuePair<int, UserPreference> userRating)
        {
            double userDifference = 0;

            // Loop through all product ratings of the target user
            foreach (KeyValuePair<int, float> targetRating in targetRatings.GetRatings())
            {
                // Get the rating of the other user
                float rating = userRating.Value.GetRating(targetRating.Key);

                if (rating > 0)
                {
                    userDifference += Math.Pow(targetRating.Value - rating, 2);
                }
            }
            double distance = Math.Sqrt(userDifference);
            return 1 / (1 + distance);
        }

    }
}
