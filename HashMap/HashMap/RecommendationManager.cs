using System;
using System.Collections.Generic;
using System.Data;
using HashMap.Strategy;

namespace HashMap
{
    internal class RecommendationManager
    {

        public static Dictionary<int, UserPreference> UserPreferences;
        private DataTable _userRatings;
        private DataTable deviations;

        public void StartDataRead()
        {
//            var processor = new DataProcessor();
//            var calculator = new Calculator();

//            processor.setFilePath(Constants.ml);
//            UserPreferences = processor.ReadDataFromFile();
//
//            calculator.PassDataSet(UserPreferences);
//
//            calculator.SetCalculator(new Pearson());
//            calculator.ExecuteWithTarget(186);
//
//            calculator.SetCalculator(new Eucledian());
//            calculator.ExecuteWithTarget(186);
//
//            calculator.SetCalculator(new Cosine());
//            calculator.ExecuteWithTarget(186);

            DataTableProcessor dtProcessor = new DataTableProcessor();
            _userRatings = dtProcessor.ReadDataFromFile();

            DeviationCalculator dc = new DeviationCalculator();
            deviations = dc.Execute();
//            dc.InsertRating(3, 105, 4.5);

            ItemItemRatingPredictor iirp = new ItemItemRatingPredictor(deviations);

            iirp.SetUser(186);
            iirp.Execute();

//            iirp.SetUser(3);
//            iirp.Execute();

            Console.ReadKey();

        }
    }
}