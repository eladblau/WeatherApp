using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WeatherApp.Classes;

namespace WeatherApp.Helpers
{
    public static class WeatherApiHelper
    {
        private static string baseUrl = Startup.StaticConfig.GetSection("MyAppSettings").GetSection("WeatherBaseUrl").Value;
        private static string apiKey = Startup.StaticConfig.GetSection("MyAppSettings").GetSection("ApiKey").Value;

        public static async Task<List<Location>> SearchLocations(string searchedVal)
        {
            try
            {
                List<Location> locations = null;

                string apiUrl = Startup.StaticConfig.GetSection("MyAppSettings").GetSection("AutocompleteApiUrl").Value;
                apiUrl += $"?q={searchedVal}&apikey={apiKey}";

                using (var client = new HttpClient())
                {
                    //Passing service base url  
                    client.BaseAddress = new Uri(baseUrl);

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetDepartments using HttpClient  
                    HttpResponseMessage Res = await client.GetAsync(apiUrl);

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        var ObjResponse = Res.Content.ReadAsStringAsync().Result;
                        locations = JsonConvert.DeserializeObject<List<Location>>(ObjResponse);
                    }
                    return locations;
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

                string apiUrl = Startup.StaticConfig.GetSection("MyAppSettings").GetSection("CurrentWeatherApiUrl").Value;
                apiUrl += $"{locationKey}?apikey={apiKey}";

                using (var client = new HttpClient())
                {
                    //Passing service base url  
                    client.BaseAddress = new Uri(baseUrl);

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetDepartments using HttpClient  
                    HttpResponseMessage Res = await client.GetAsync(apiUrl);

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        var ObjResponse = Res.Content.ReadAsStringAsync().Result;
                        weather = JsonConvert.DeserializeObject<List<Weather>>(ObjResponse).FirstOrDefault();
                    }
                    return weather;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private static string GetDummyData(int type)
        {
            string res = string.Empty;
            switch (type)
            {
                case 1: //Locations
                    res = @" [
                          {
                        'Version': 1,
                            'Key': '215854',
                            'Type': 'City',
                            'Rank': 31,
                            'LocalizedName': 'Tel Aviv',
                            'Country': {
                            'ID': 'IL',
                              'LocalizedName': 'Israel'
                            },
                            'AdministrativeArea': {
                            'ID': 'TA',
                              'LocalizedName': 'Tel Aviv'
                            }
                    },
                          {
                        'Version': 1,
                            'Key': '3431644',
                            'Type': 'City',
                            'Rank': 45,
                            'LocalizedName': 'Telanaipura',
                            'Country': {
                            'ID': 'ID',
                              'LocalizedName': 'Indonesia'
                            },
                            'AdministrativeArea': {
                            'ID': 'JA',
                              'LocalizedName': 'Jambi'
                            }
                    }
                        ]";
                    break;
                case 2: //Weather
                    res = @"[
                          {
                            'LocalObservationDateTime': '2021-04-23T08:55:00+03:00',
                            'EpochTime': 1619157300,
                            'WeatherText': 'Hazy sunshine',
                            'WeatherIcon': 5,
                            'HasPrecipitation': false,
                            'PrecipitationType': null,
                            'IsDayTime': true,
                            'Temperature': {
                              'Metric': {
                                'Value': 16.7,
                                'Unit': 'C',
                                'UnitType': 17
                              },
                              'Imperial': {
                                'Value': 62,
                                'Unit': 'F',
                                'UnitType': 18
                              }
                            },
                            'MobileLink': 'http://m.accuweather.com/en/il/tel-aviv/215854/current-weather/215854?lang=en-us',
                            'Link': 'http://www.accuweather.com/en/il/tel-aviv/215854/current-weather/215854?lang=en-us'
                          }
                        ]";
                    break;
                default:
                    break;
            }
            return res;
        }
    }
}
