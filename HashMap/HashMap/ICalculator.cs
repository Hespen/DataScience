using System.Collections.Generic;

namespace HashMap
{
    internal interface ICalculator
    {
        void Execute(Dictionary<int, UserPreference> userRatings, int target);
    }
}