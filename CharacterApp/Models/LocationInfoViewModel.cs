using System.ComponentModel.DataAnnotations;

namespace CharacterApp.Models
{
    public class LocationInfoViewModel
    {
        [Required(ErrorMessage = "Location is required")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
