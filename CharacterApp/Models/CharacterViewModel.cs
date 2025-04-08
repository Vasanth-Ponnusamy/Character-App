using CharacterApp.Data.Model;
using System.ComponentModel.DataAnnotations;

namespace CharacterApp.Models
{
    public class CharacterViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Species { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }

        public int OriginId { get; set; }
        public LocationInfoViewModel Origin { get; set; }

        public int LocationId { get; set; }
        public LocationInfoViewModel Location { get; set; }

        public string Image { get; set; }

        public List<string> Episodes { get; set; }

        public string Url { get; set; }
        public DateTime Created { get; set; }
    }
}
