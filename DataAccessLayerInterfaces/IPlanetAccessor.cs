using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayerInterfaces
{
    public interface IPlanetAccessor
    {
        List<PlanetVM> SelectPlanetsByPlanetID(string planetID);
        PlanetVM SelectPlanetVMByPlanetID(string planetID);
        int InsertRegionRecord(string regionID, string RegionArticleLink);
        int InsertSectorRecord(string sectorID, string regionID, string sectorArticleLink);
        int InsertPlanetarySystemRecord(string systemID, string sectorID, string systemArticleLink);
        int InsertPlanetRecord(Planet planet);
        int SelectRegionByRegionID(string regionID);
        int SelectSectorBySectorID(string sectorID);
        int SelectPlanetarySystemBySystemID(string systemID);
        int SelectPlanetByPlanetID(string planetID);
        int DeletePlanetByPlanetID(string planetID);
        int UpdatePlanetCoordinateByPlanetID(string planetID, double xCord, double yCord);
        List<PlanetVM> SelectPlanetsMVCByPlanetID(string planetID);
        PlanetVM SelectOnePlanetVMMVCByPlanetID(string planetID);
        int DeletePlanetMVCByPlanetID(string planetID);
        List<Region> SelectAllRegions();
        List<Sector> SelectAllSectors();
        List<PlanetarySystem> SelectAllPlanetarySystem();
        int InsertPlanetMVCRecord(Planet planet);
        int UpdatePlanetMVC(PlanetVM oldPlanet, PlanetVM newPlanet);

    }
}
