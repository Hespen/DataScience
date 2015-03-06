using System.Collections.Generic;

namespace HashMap
{
    internal class RecommendationManager
    {

        public static Dictionary<int, UserPreference> UserPreferences;

        public void StartDataRead()
        {
            var processor = new DataProcessor();
            var calculator = new Calculator();
            UserPreferences = processor.readDataFromFile();
            calculator.PassDataSet(UserPreferences);

            calculator.SetCalculator(new Pearson());
            calculator.ExecuteWithTarget(186);


            calculator.SetCalculator(new Eucledian());
            calculator.ExecuteWithTarget(186);

            calculator.SetCalculator(new Cosine());
            calculator.ExecuteWithTarget(186);
        }
    }
}