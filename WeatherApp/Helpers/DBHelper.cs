using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Classes;

namespace WeatherApp.Helpers
{
    public static class DBHelper
    {
        private const string SQL_CONNECTIONS = "SqlConnections";
        private const string WeatherDatabase = "WeatherDatabase";

        //Weather
        private const string USP_INSERT_CURRENT_WEATHER = "usp_InsertCurrentWeather";
        private const string USP_GET_CURRENT_WEATHER = "usp_GetCurrentWeather";

        //Favorites
        private const string USP_GET_FAVORITES = "usp_GetFavorites";
        private const string USP_INSERT_FAVORITE = "usp_InsertFavorite";
        private const string USP_DELETE_FAVORITE = "usp_DeleteFavorite";


        private const string CELSIUS_UNIT = "C";
        private const int CELSIUS_UNIT_TYPE = 17;


        public static async Task<List<Favorite>> GetUserFavorites(string locationKey = null)
        {
            try
            {
                List<Favorite> favorites = new List<Favorite>();
                DataTable table = new DataTable();

                string connectionString = Startup.StaticConfig.GetSection(SQL_CONNECTIONS).GetSection(WeatherDatabase).Value;
                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(USP_GET_FAVORITES, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(locationKey)) cmd.Parameters.AddWithValue("@locationKey", locationKey);

                    con.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        table.Load(reader);
                    }
                }
                foreach (DataRow dr in table.Rows)
                {
                    favorites.Add(new Favorite()
                    {
                        LocationKey = dr["LocationKey"]?.ToString(),
                        LocalizedName = dr["LocalizedName"].ToString()
                    });
                }
                return favorites;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static async Task<string> InsertFavorite(Favorite favorite)
        {
            try
            {
                string sRes = string.Empty;
                string connectionString = Startup.StaticConfig.GetSection(SQL_CONNECTIONS).GetSection(WeatherDatabase).Value;
                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(USP_INSERT_FAVORITE, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await con.OpenAsync();
                    {
                        cmd.Parameters.AddWithValue("@LocationKey", favorite.LocationKey);
                        cmd.Parameters.AddWithValue("@LocalizedName", favorite.LocalizedName);

                        var res = await cmd.ExecuteNonQueryAsync();
                        sRes = res.ToString();
                    }
                    con.Close();

                    return sRes;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static async Task<string> DeleteFavorite(string locationKey)
        {
            try
            {
                string sRes = string.Empty;
                string connectionString = Startup.StaticConfig.GetSection(SQL_CONNECTIONS).GetSection(WeatherDatabase).Value;
                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(USP_DELETE_FAVORITE, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await con.OpenAsync();
                    {
                        cmd.Parameters.AddWithValue("@locationKey", locationKey);

                        var res = await cmd.ExecuteNonQueryAsync();
                        sRes = res.ToString();
                    }
                    con.Close();

                    return sRes;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<string> InsertCurrentWeather(Weather weather, string locationKey, DateTime lastModified)
        {
            try
            {
                string sRes = string.Empty;
                string connectionString = Startup.StaticConfig.GetSection(SQL_CONNECTIONS).GetSection(WeatherDatabase).Value;
                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(USP_INSERT_CURRENT_WEATHER, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await con.OpenAsync();
                    {
                        cmd.Parameters.AddWithValue("@Key", locationKey);
                        cmd.Parameters.AddWithValue("@LocalObservationDateTime", weather.LocalObservationDateTime);
                        cmd.Parameters.AddWithValue("@CelsiusValue", weather.Temperature.Metric.Value);
                        cmd.Parameters.AddWithValue("@WeatherText", weather.WeatherText);
                        cmd.Parameters.AddWithValue("@lastModified", lastModified);

                        var res = await cmd.ExecuteNonQueryAsync();
                        sRes = res.ToString();
                    }
                    con.Close();

                    return sRes;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<Weather> GetCurrentWeather(string locationKey)
        {
            try
            {
                Weather weather = null;
                DataTable table = new DataTable();

                string connectionString = Startup.StaticConfig.GetSection(SQL_CONNECTIONS).GetSection(WeatherDatabase).Value;
                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(USP_GET_CURRENT_WEATHER, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    cmd.Parameters.AddWithValue("@Key", locationKey);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        table.Load(reader);
                    }
                }
                foreach (DataRow dr in table.Rows)
                {
                    weather = new Weather()
                    {
                        LocalObservationDateTime = DateTime.Parse(dr["LocalObservationDateTime"].ToString()),
                        Temperature = new Temperature()
                        {
                            Metric = new TemperatureValues()
                            {
                                Unit = CELSIUS_UNIT,
                                UnitType = CELSIUS_UNIT_TYPE,
                                Value = double.Parse(dr["CelsiusValue"].ToString())
                            }
                        },
                        WeatherText = dr["WeatherText"].ToString()
                    };

                    //Return the first record
                    break;
                }
                return weather;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
