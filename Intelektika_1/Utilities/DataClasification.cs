using Intelektika_1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelektika_1.Utilities
{
    class DataClasification
    {
        public static List<DataSetDistanceModel> GetClosestSets(List<DataSetModel> dataSetModels, DataSetModel comparison)
        {
            Trace.WriteLine("GetClosestSets");
            var list = new List<DataSetDistanceModel>();
            for (int i = 0; i < dataSetModels.Count; i++)
            {
                if(comparison.Sport != dataSetModels[i].Sport)
                {
                    continue;
                }

                double distance = Math.Sqrt(Math.Pow(dataSetModels[i].Height - comparison.Height, 2) + Math.Pow(dataSetModels[i].Weight - comparison.Weight, 2));
                distance = Math.Round(distance, 2);
                int index = list.FindIndex(x => x.Position == dataSetModels[i].Position);
                if (index == -1)
                {
                    list.Add(new DataSetDistanceModel(dataSetModels[i], distance));
                }
                else if (list[index].Distance > distance)
                {
                    list[index] = new DataSetDistanceModel(dataSetModels[i], distance);
                }

            }
            return list;
        }
    }
}
