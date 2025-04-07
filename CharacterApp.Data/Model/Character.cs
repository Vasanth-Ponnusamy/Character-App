using System.ComponentModel.DataAnnotations;

namespace CharacterApp.Data.Model
{
    public class Character
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Species { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }

        public LocationInfo Origin { get; set; }
        public LocationInfo Location { get; set; }

        public string Image { get; set; }

        public List<int> Episodes { get; set; }

        public string Url { get; set; }
        public DateTime Created { get; set; }
    }
}
