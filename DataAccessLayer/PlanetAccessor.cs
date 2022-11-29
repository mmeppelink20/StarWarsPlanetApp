using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayerInterfaces;
using DataObjects;

namespace DataAccessLayer
{
    public class PlanetAccessor : IPlanetAccessor
    {
        public PlanetVM SelectPlanetVMByPlanetID(string planetID)
        {
            PlanetVM planet = new PlanetVM();

            var connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_select_planet_by_planetID";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PlanetID", SqlDbType.NVarChar, 50);

            cmd.Parameters["@PlanetID"].Value = planetID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    planet.PlanetID = reader.GetString(0);
                    planet.GridNumber = reader.GetString(1);
                    planet.PlanetArticleLink = reader.GetString(2);
                    planet.PlanetCoordinateX = reader.GetDecimal(3);
                    planet.PlanetCoordinateY = reader.GetDecimal(4);
                    planet.SystemID = reader.GetString(5);

                    planet.SystemArticleLink = reader.GetString(6);
                    planet.SectorID = reader.GetString(7);
                    planet.SectorArticleLink = reader.GetString(8);
                    planet.RegionID = reader.GetString(9);
                    planet.RegionArticleLink = reader.GetString(10);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return planet;
        }

        public List<PlanetVM> SelectPlanetsByPlanetID(string planetID)
        {
            List<PlanetVM> planets = new List<PlanetVM>();

            var connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_select_planet_by_planetID";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PlanetID", SqlDbType.NVarChar, 50);

            cmd.Parameters["@PlanetID"].Value = planetID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var planet = new PlanetVM();
                        planet.PlanetID = reader.GetString(0);
                        planet.GridNumber = reader.GetString(1);
                        planet.PlanetArticleLink = reader.GetString(2);
                        planet.PlanetCoordinateX = reader.GetDecimal(3);
                        planet.PlanetCoordinateY = reader.GetDecimal(4);
                        planet.SystemID = reader.GetString(5);

                        planet.SystemArticleLink = reader.GetString(6);
                        planet.SectorID = reader.GetString(7);
                        planet.SectorArticleLink = reader.GetString(8);
                        planet.RegionID = reader.GetString(9);
                        planet.RegionArticleLink = reader.GetString(10);

                        planets.Add(planet);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return planets;
        }

        public int SelectRegionByRegionID(string regionID)
        {
            int result = 0;

            var connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_select_region_by_regionID";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@RegionID", SqlDbType.NVarChar, 50);

            cmd.Parameters["@RegionID"].Value = regionID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result++;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
        public int InsertRegionRecord(string regionID, string regionArticleLink)
        {
            int result = 0;

            var connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_insert_region_record";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@RegionID", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@ArticleLink", SqlDbType.NVarChar, 250);

            cmd.Parameters["@RegionID"].Value = regionID;
            cmd.Parameters["@ArticleLink"].Value = regionArticleLink;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result++;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        public int InsertSectorRecord(string sectorID, string regionID, string sectorArticleLink)
        {
            int result = 0;

            var connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_insert_sector_record";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@SectorID", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@RegionID", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@ArticleLink", SqlDbType.NVarChar, 250);

            cmd.Parameters["@SectorID"].Value = sectorID;
            cmd.Parameters["@RegionID"].Value = regionID;
            cmd.Parameters["@ArticleLink"].Value = sectorArticleLink;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result++;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        public int InsertPlanetarySystemRecord(string systemID, string sectorID, string systemArticleLink)
        {
            int result = 0;

            var connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_insert_system_record";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@SystemID", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@SectorID", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@ArticleLink", SqlDbType.NVarChar, 250);

            cmd.Parameters["@SystemID"].Value = systemID;
            cmd.Parameters["@SectorID"].Value = sectorID;
            cmd.Parameters["@ArticleLink"].Value = systemArticleLink;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result++;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        public int InsertPlanetRecord(Planet planet)
        {
            int result = 0;

            var connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_insert_planet_record";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PlanetID", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@SystemID", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@GridNumber", SqlDbType.NVarChar, 5);
            cmd.Parameters.Add("@ArticleLink", SqlDbType.NVarChar, 250);
            cmd.Parameters.Add("@PlanetCoordinateX", SqlDbType.Decimal);
            cmd.Parameters.Add("@PlanetCoordinateY", SqlDbType.Decimal);

            cmd.Parameters["@PlanetID"].Value = planet.PlanetID;
            cmd.Parameters["@SystemID"].Value = planet.SystemID;
            cmd.Parameters["@GridNumber"].Value = planet.GridNumber;
            cmd.Parameters["@ArticleLink"].Value = planet.PlanetArticleLink;
            cmd.Parameters["@PlanetCoordinateX"].Value = planet.PlanetCoordinateX;
            cmd.Parameters["@PlanetCoordinateY"].Value = planet.PlanetCoordinateY;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result++;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        public int SelectSectorBySectorID(string sectorID)
        {
            int result = 0;

            var connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_select_sector_by_sectorID";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@SectorID", SqlDbType.NVarChar, 50);

            cmd.Parameters["@SectorID"].Value = sectorID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result++;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
        public int SelectPlanetarySystemBySystemID(string systemID)
        {
            int result = 0;

            var connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_select_system_by_systemID";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@SystemID", SqlDbType.NVarChar, 50);

            cmd.Parameters["@SystemID"].Value = systemID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result++;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public int SelectPlanetByPlanetID(string planetID)
        {
            int result = 0;

            var connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_select_planet_by_planetID";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PlanetID", SqlDbType.NVarChar, 50);

            cmd.Parameters["@PlanetID"].Value = planetID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result++;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public int DeletePlanetByPlanetID(string planetID)
        {
            int result = 0;

            var connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_delete_planet_record";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PlanetID", SqlDbType.NVarChar, 50);

            cmd.Parameters["@PlanetID"].Value = planetID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result++;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return result;

        }

        public int UpdatePlanetCoordinateByPlanetID(string planetID, double xCord, double yCord)
        {
            int result = 0;

            var connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_update_planet_coordinates";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PlanetID", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@PlanetCoordinateX", SqlDbType.Decimal);
            cmd.Parameters.Add("@PlanetCoordinateY", SqlDbType.Decimal);

            cmd.Parameters["@PlanetID"].Value = planetID;
            cmd.Parameters["@PlanetCoordinateX"].Value = xCord;
            cmd.Parameters["@PlanetCoordinateY"].Value = yCord;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result++;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
    }
}
