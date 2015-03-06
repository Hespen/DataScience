using System;
using System.Collections.Generic;

namespace HashMap
{
    internal class Calculator
    {
        private ICalculator calculator;
        private Dictionary<int, UserPreference> ratingsDataSet;
        private RatingPredictor ratingPredictor;

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
            int[] targetIds = new int[]{101,103,106};
            foreach (var targetId in targetIds)
            {
                ratingPredictor.ArticleId = targetId;
                ratingPredictor.CalculateInfluenceWeight();
            }
        }
    }
}