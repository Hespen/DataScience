using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HashMap
{
    interface ICalculator
    {
        void Execute(Dictionary<int, UserPreference> userRatings, int target);
    }
}
