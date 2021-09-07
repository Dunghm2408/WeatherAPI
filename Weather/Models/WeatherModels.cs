using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{

    public class WeatherResponseModels
    {
        public List<ListModels> List { get; set; }
    }

    public class ListModels
    {
        public List<WeatherModels> Weather { get; set; }
        public MainModels Main { get; set; }
        public String Name { get; set; }
        public string Id { get; set; }
    }

    public class WeatherModels
    {
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    public class MainModels
    {
        public double? Temp { get; set; }
        public double? Humidity { get; set; }
    }
}
