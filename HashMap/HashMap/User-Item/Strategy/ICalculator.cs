using System.Collections.Generic;

namespace HashMap.Strategy
{
    internal interface ICalculator
    {
        Dictionary<int,double> Execute(Dictionary<int, UserPreference> userRatings, int target);
    }
}