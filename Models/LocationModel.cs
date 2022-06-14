﻿namespace MapMVCWebApp.Models
{
    public class LocationModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public LocationModel()
        {

        }
    }
}
