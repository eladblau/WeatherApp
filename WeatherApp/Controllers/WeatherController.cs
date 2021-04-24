using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Classes;
using WeatherApp.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeatherApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string searchedVal)
        {
            try
            {
                //Get locations from external api
                List<Location> locations = await WeatherApiHelper.SearchLocations(searchedVal);

                return Ok(locations);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("GetCurrentWeather")]
        public async Task<IActionResult> GetCurrentWeather(string locationKey)
        {
            try
            {
                Weather weather = null;
                //Check if this location weather exist in the db
                weather = await DBHelper.GetCurrentWeather(locationKey);

                if (weather == null)
                {
                    //Get current weather from external api
                    weather = await WeatherApiHelper.GetCurrentWeather(locationKey);

                    //Insert to db
                    await DBHelper.InsertCurrentWeather(weather, locationKey, DateTime.Now);
                }

                //Check if this location is favorite
                List<Favorite> favorite = await DBHelper.GetUserFavorites(locationKey);
                if (favorite.Count > 0)
                {
                    weather.isFavorite = true;
                }

                return Ok(weather);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
