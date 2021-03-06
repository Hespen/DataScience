﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HashMap.Strategy;

namespace HashMap
{
    internal class Calculator
    {
        private ICalculator calculator;
        private Dictionary<int, UserPreference> ratingsDataSet;
        private RatingPredictor ratingPredictor;
        private int _currentTarget;

        public Calculator()
        {
            ratingPredictor = new RatingPredictor();
        }

        public void SetCalculator(ICalculator calculator)
        {
            this.calculator = calculator;
        }

        public void PassDataSet(Dictionary<int, UserPreference> dataSet)
        {
            ratingsDataSet = dataSet;
        }

        public void ExecuteWithTarget(int i)
        {
            _currentTarget = i;
            if (calculator == null)
            {
                Console.WriteLine("Please specify a calculator");
            }
            else if (ratingsDataSet == null)
            {
                Console.WriteLine("Datasource needed");
            }
            else
            {
                SetUpRatingPredictor(calculator.Execute(ratingsDataSet, i));
            }
            Console.ReadKey();
        }

        private void SetUpRatingPredictor(Dictionary<int, double> nearestNeighbours)
        {
            ratingPredictor.NearestNeighbours = nearestNeighbours;
            var targetIds = getNotRatedArticleIds(nearestNeighbours);
            Dictionary<int,double> predictedRatings = new Dictionary<int, double>();
            foreach (var targetId in targetIds)
            {
                ratingPredictor.ArticleId = (int)targetId;
                double predictedRating = ratingPredictor.CalculateInfluenceWeight();
                if (predictedRating > -1)
                {
                    predictedRatings.Add((int) targetId, predictedRating);
                }
            }
            int n = 0;
            foreach (var neighbour in predictedRatings.OrderByDescending(key => key.Value))
            {
                if (neighbour.Value < 0.35 || n >= 8) continue;
                Console.WriteLine("Movie " + neighbour.Key + " will be rated " + neighbour.Value);
                n++;
            }
        }

        private ArrayList getNotRatedArticleIds(Dictionary<int, double> nearestNeighbours)
        {
            ArrayList ids = new ArrayList();

            UserPreference targetUser = RecommendationManager.UserPreferences[_currentTarget];
            foreach (var articleId in DataProcessor.ArticleIds)
            {
                if (targetUser.GetRating(articleId) == 0)
                {
                    ids.Add(articleId);
                }
            }
            return ids;
        }
    }
}