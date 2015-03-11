using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationsC
{
    class RecommendationManager
    {
        private DataTable ratingMatrix;

        public RecommendationManager()
        {
        
        }

        public void RunProgram()
        {
            DataProcessor dataProcessor = new DataProcessor();
            ratingMatrix = dataProcessor.ReadDataFromFile();

            Calculator c = new Calculator();
            c.PassDataSet(ratingMatrix);
            
            c.SetCalculator(new Percentage());
            c.Execute();

            c.SetCalculator(new Mean());
            c.Execute();
        }

    }
}
