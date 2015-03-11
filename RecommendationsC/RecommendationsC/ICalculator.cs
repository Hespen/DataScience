using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationsC
{
    interface ICalculator
    {
        List<Tuple<string, double>> Execute(DataTable ratingMatrix);
    }
}
