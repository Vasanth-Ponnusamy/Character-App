namespace CharacterApp.DataSync.Model
{
    public class CharacterResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Species { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }
        public PlaceInfo Origin { get; set; }
        public PlaceInfo Location { get; set; }
        public string Image { get; set; }
        public List<string> Episode { get; set; }
        public string Url { get; set; }
        public DateTime Created { get; set; }
    }

    public class PlaceInfo
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
