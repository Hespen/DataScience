using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashMap
{
    class RecommendationManager
    {

        private ICalculator calculator;
        private Dictionary<int, UserPreference> ratingsDataSet; 

        public RecommendationManager(){}

        public void startDataRead()
        {
            DataProcessor processor = new DataProcessor();
            Calculator calculator = new Calculator();
            calculator.PassDataSet(processor.readDataFromFile());

            calculator.SetCalculator(new Pearson());
            calculator.ExecuteWithTarget(7);


            calculator.SetCalculator(new Eucledian());
            calculator.ExecuteWithTarget(7);
        }

        
    }
}
