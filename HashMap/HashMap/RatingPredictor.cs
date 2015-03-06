﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HashMap
{
    class RatingPredictor
    {

        private Dictionary<int, double> nearestNeighbours;
        private int articleId;

        public RatingPredictor()
        {
            
        }

        public double CalculateInfluenceWeight()
        {
            if (nearestNeighbours == null || articleId == 0) return -1;

            double totalCoefficient = 0;
            var userRatings = new Dictionary<int, double>();

            foreach (var nearestNeighbour in nearestNeighbours)
            {
                
                if (RecommendationManager.UserPreferences.ContainsKey(nearestNeighbour.Key))
                {
                    //If Nearest Neighbour has rated the product, add the coefficient to the totalCoefficient value.
                    //Create new Dictionary with all neighbours who rated the product
                    UserPreference userPreference = RecommendationManager.UserPreferences[nearestNeighbour.Key];
                    float rating = userPreference.GetRating(articleId);
                    if (rating > -1)
                    {
                        totalCoefficient += nearestNeighbour.Value;
                        userRatings.Add(nearestNeighbour.Key,rating);
                    }
                }
            }
            
            return CalculatePredictedRating(userRatings,totalCoefficient);
        }

        private double CalculatePredictedRating(Dictionary<int, double> userRatings, double totalCoefficient)
        {
            double predictedRating = 0.0;
            //Calculate ratings
            foreach (var userRating in userRatings)
            {
                double coefficient = nearestNeighbours[userRating.Key];
                double influenceWeight = coefficient / totalCoefficient;
                predictedRating += influenceWeight * userRating.Value;
            }
            Console.WriteLine(predictedRating);
            return predictedRating;
        }

        public Dictionary<int, double> NearestNeighbours
        {
            get { return nearestNeighbours; }
            set { nearestNeighbours = value; }
        }

        public int ArticleId
        {
            get { return articleId; }
            set { articleId = value; }
        }


        

    }
}