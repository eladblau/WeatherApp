using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApp.Classes
{
    public class Classes
    {
    }

    public class Location
    {
        public int Version { get; set; }
        public string Key { get; set; }
        public string Type { get; set; }
        public int Rank { get; set; }
        public string LocalizedName { get; set; }
        public Country Country { get; set; }
        public Country AdministrativeArea { get; set; }
    }

    public class Country
    {
        public string ID { get; set; }
        public string LocalizedName { get; set; }
    }

    public class Weather
    {
        public DateTime LocalObservationDateTime { get; set; }
        public string WeatherText { get; set; }
        public int WeatherIcon { get; set; }
        public bool IsDayTime { get; set; }
        public Temperature Temperature { get; set; }
        public bool isFavorite { get; set; }

    }

    public class Temperature
    {
        public TemperatureValues Metric { get; set; }
        public TemperatureValues Imperial { get; set; }
    }

    public class TemperatureValues
    {
        public double Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
    }

    public class Favorite
    {
        public string LocationKey { get; set; }
        public string LocalizedName { get; set; }
    }
}

