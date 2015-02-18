using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashMap
{
    class Calculator
    {

        private ICalculator calculator;
        private Dictionary<int, UserPreference> ratingsDataSet; 

        public void SetCalculator(ICalculator calculator)
        {
            this.calculator = calculator;
        }

        public void PassDataSet(Dictionary<int, UserPreference> dataSet)
        {
            this.ratingsDataSet = dataSet;
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
                calculator.Execute(ratingsDataSet, i);
            }
            Console.ReadKey();
        }
    }
}
