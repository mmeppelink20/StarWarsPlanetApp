using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Planet
    {
        public string PlanetID { get; set; }
        public string GridNumber { get; set; }
        public string PlanetArticleLink { get; set; }
       
        public decimal PlanetCoordinateX { get; set; }
        public decimal PlanetCoordinateY { get; set; }
        public string SystemID { get; set; }
    }

    public class PlanetVM : Planet
    {
        public string SystemArticleLink { get; set; }
        public string SectorID { get; set; }

        public string SectorArticleLink { get; set; }
        public string RegionID { get; set; }
        public string RegionArticleLink { get; set; }

    }
}
