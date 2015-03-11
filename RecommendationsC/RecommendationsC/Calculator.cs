using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationsC
{
    class Calculator
    {
        private ICalculator _calculator;
        private DataTable _ratingMatrix;

        public Calculator()
        {
        }

        public void SetCalculator(ICalculator calculator)
        {
            this._calculator = calculator;
        }

        public void PassDataSet(DataTable ratingMatrix)
        {
            _ratingMatrix = ratingMatrix;
        }

        public void Execute()
        {
            if (_calculator == null)
            {
                Console.WriteLine("Please specify a calculator");
            }
            else if (_ratingMatrix == null)
            {
                Console.WriteLine("Datasource needed");
            }
            else
            {
                var result = _calculator.Execute(_ratingMatrix);
                printResult(result);
            }
            Console.WriteLine("\n");
            Console.ReadKey();
        }

        private void printResult(List<Tuple<string, double>> result)
        {
            foreach (var tuple in result)
            {
                Console.WriteLine(tuple.Item1 + " \t : " + tuple.Item2);
            }
        }
    }
}
