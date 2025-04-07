using System.ComponentModel.DataAnnotations;

namespace CharacterApp.Data.Model
{
    public class LocationInfo
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
