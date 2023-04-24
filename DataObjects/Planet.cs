using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataObjects
{
    public class Planet
    {
        [Required(ErrorMessage = "What is the planet name")]
        public string PlanetID { get; set; }

        [RegularExpression(@"[a-zA-Z]-\d\d", ErrorMessage = "Grid number must follow the \"B-12\" format")]
        public string GridNumber { get; set; }

        [Url]
        public string PlanetArticleLink { get; set; }
       
        [Range(1, 740.64)]
        public decimal PlanetCoordinateX { get; set; }

        [Range(1, 653.55)]
        public decimal PlanetCoordinateY { get; set; }

        public string SystemID { get; set; }
    }

    public class PlanetVM : Planet
    {
        [Url]
        public string SystemArticleLink { get; set; }

        [Required(ErrorMessage = "What is the sector name")]
        public string SectorID { get; set; }

        [Url]
        public string SectorArticleLink { get; set; }

        [Required(ErrorMessage = "What is the region name")]
        public string RegionID { get; set; }

        [Url]
        public string RegionArticleLink { get; set; }

    }
}
