using System.ComponentModel.DataAnnotations;

namespace CharacterApp.Data.Model
{
    public class Episode
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
    }
}
