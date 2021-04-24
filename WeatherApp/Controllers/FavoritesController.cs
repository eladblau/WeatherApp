using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Classes;
using WeatherApp.Helpers;

namespace WeatherApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {

        [HttpGet("GetFavorites")]
        public async Task<IActionResult> GetFavorites()
        {
            try
            {
                List<Favorite> favorites = await DBHelper.GetUserFavorites();
                List<Location> locations = new List<Location>();

                foreach (Favorite f in favorites)
                {
                    locations.Add(new Location
                    {
                        Key = f.LocationKey,
                        LocalizedName = f.LocalizedName
                    });
                }
                return Ok(locations);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("AddToFavorites")]
        public async Task<IActionResult> AddToFavorites(string locationKey, string localizedName)
        {
            //Add to db
            try
            {
                Favorite favorite = new Favorite()
                {
                    LocalizedName = localizedName,
                    LocationKey = locationKey
                };

                await DBHelper.InsertFavorite(favorite);

                return Ok(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("DeleteFavorite")]
        public async Task<IActionResult> DeleteFavorite(string locationKey)
        {
            //Delete from db
            try
            {
                await DBHelper.DeleteFavorite(locationKey);

                return Ok(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
