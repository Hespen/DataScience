using System.Collections.Generic;

namespace HashMap
{
    internal class RecommendationManager
    {
        private ICalculator calculator;
        private Dictionary<int, UserPreference> ratingsDataSet;

        public void StartDataRead()
        {
            var processor = new DataProcessor();
            var calculator = new Calculator();
            calculator.PassDataSet(processor.readDataFromFile());

            calculator.SetCalculator(new Pearson());
            calculator.ExecuteWithTarget(7);


            calculator.SetCalculator(new Eucledian());
            calculator.ExecuteWithTarget(7);

            calculator.SetCalculator(new Cosine());
            calculator.ExecuteWithTarget(7);
        }
    }
}