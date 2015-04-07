using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashMap
{
    class Constants
    {
        public static int NearestNeighbours = 25;

        //Constants to determine dataSet
        public const int ds1 = 0;
        public const int ds2 = 1;
        public const int ml = 2;

        //DataSet paths
        public static String dataSet1 = @"../../userItem.data";
        public static String dataSet2 = @"../../userItem2.data";
        public static String movieLensDataSet = @"../../movielens.data";

        //DataSet Split Chars
        public static Char dataSplit = ',';
        public static Char movieLensSplit = '\t';
    }
}
