using System.ComponentModel.DataAnnotations;

namespace MapMVCWebApp.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = "New Location";
        [Required(ErrorMessage = "Latitude value is required.")]
        public double Latitude { get; set; }
        [Required(ErrorMessage = "Longitude value is required.")]
        public double Longitude { get; set; }

        public Location()
        {

        }
    }
}
