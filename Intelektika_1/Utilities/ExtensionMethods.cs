using Intelektika_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelektika_1.Utilities
{
    static class ExtensionMethods
    {
        public static List<string> ExtractDistancePosition(this List<DataSetDistanceModel> DataSets)
        {
            var list = new List<string>();
            foreach (var dataSet in DataSets)
            {
                list.Add(dataSet.Position);
            }
            return list;
        }
    }
}
