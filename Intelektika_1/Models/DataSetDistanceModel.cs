using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelektika_1.Models
{
    class DataSetDistanceModel : DataSetModel
    {
        private double distance;

        public double Distance { get => distance; set => distance = value; }

        public DataSetDistanceModel(DataSetModel dsm, double distance)
        {
            Id = dsm.Id;
            Height = dsm.Height;
            Weight = dsm.Weight;
            Sport = dsm.Sport;
            Position = dsm.Position;
            Distance = distance;
        }
    }
}
