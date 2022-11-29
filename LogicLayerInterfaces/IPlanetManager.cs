using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace LogicLayerInterfaces
{
    public interface IPlanetManager
    {
        List<PlanetVM> RetrievePlanetVMsByPlanetID(string planetID);

        PlanetVM RetrievePlanetVMByPlanetID(string planetID);

        int AddRegionRecord(string regionID, string regionArticleLink);
        int AddPlanetarySystemRecord(string systemID, string sectorID, string systemArticleLink);
        int AddSectorRecord(string sectorID, string regionID, string sectorArticleLink);
        int AddPlanetRecord(Planet planet);
        int RetrieveRegionByRegionID(string regionID);
        int RetrieveSectorBySectorID(string sectorID);
        int RetrievePlanetarySystemBySystemID(string systemID);
        int RetrievePlanetByPlanetID(string planetID);
        int UpdatePlanetCoordinates(string planetID, double xCord, double yCord);
        int DeletePlanetByPlanetID(string planetID);

    }
}
