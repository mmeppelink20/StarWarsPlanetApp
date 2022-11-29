using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessLayerInterfaces;
using LogicLayerInterfaces;

namespace LogicLayer
{
    public class PlanetManager : IPlanetManager
    {
        private IPlanetAccessor _planetAccessor = null;

        public PlanetManager()
        {
            _planetAccessor = new DataAccessLayer.PlanetAccessor();
        }

        public List<PlanetVM> RetrievePlanetVMsByPlanetID(string planetID)
        {
            List<PlanetVM> planets = null;

            try
            {
                planets = _planetAccessor.SelectPlanetsByPlanetID(planetID);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data not found", ex);
            }

            return planets;
        }

        public PlanetVM RetrievePlanetVMByPlanetID(string planetID)
        {
            PlanetVM planet = null;

            try
            {
                planet = _planetAccessor.SelectPlanetVMByPlanetID(planetID);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data not found", ex);
            }

            return planet;
        }

        public int AddRegionRecord(String regionID, string regionArticleLink)
        {
            int result = 0;
            try
            {
                result = _planetAccessor.InsertRegionRecord(regionID, regionArticleLink);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("That region already exists", ex);
            }
            return result;
        }

        public int RetrieveRegionByRegionID(string regionID)
        {
            int result = 0;
            try
            {
                result = _planetAccessor.SelectRegionByRegionID(regionID);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("" + ex);
            }
            return result;
        }

        public int RetrieveSectorBySectorID(string sectorID)
        {
            int result = 0;
            try
            {
                result = _planetAccessor.SelectSectorBySectorID(sectorID);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("" + ex);
            }
            return result;
        }
        public int AddSectorRecord(string sectorID, string regionID, string sectorArticleLink)
        {
            int result = 0;
            try
            {
                result = _planetAccessor.InsertSectorRecord(sectorID, regionID, sectorArticleLink);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("That sector already exists", ex);
            }
            return result;
        }

        public int AddPlanetRecord(Planet planet)
        {
            int result = 0;
            try
            {
                result = _planetAccessor.InsertPlanetRecord(planet);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("That sector already exists", ex);
            }
            return result;
        }

        public int RetrievePlanetarySystemBySystemID(string systemID)
        {
            int result = 0;
            try
            {
                result = _planetAccessor.SelectPlanetarySystemBySystemID(systemID);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("" + ex);
            }
            return result;
        }

        public int AddPlanetarySystemRecord(string systemID, string sectorID, string systemArticleLink)
        {
            int result = 0;
            try
            {
                result = _planetAccessor.InsertPlanetarySystemRecord(systemID, sectorID, systemArticleLink);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("That system already exists", ex);
            }
            return result;
        }

        public int RetrievePlanetByPlanetID(string planetID)
        {
            int result = 0;
            try
            {
                result = _planetAccessor.SelectPlanetByPlanetID(planetID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("", ex);
            }
            return result;
        }

        public int DeletePlanetByPlanetID(string planetID)
        {
            int result = 0;
            try
            {
                result = _planetAccessor.DeletePlanetByPlanetID(planetID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("", ex);
            }
            return result;
        }

        public int UpdatePlanetCoordinates(string planetID, double xCord, double yCord)
        {
            int result = 0;
            try
            {
                result = _planetAccessor.UpdatePlanetCoordinateByPlanetID(planetID, xCord, yCord);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("", ex);
            }
            return result;
        }
    }
}
