namespace CharacterApp.DataSync.Model
{
    internal class ApiResponse
    {
        public PageInfo Info { get; set; }
        public List<CharacterResponse> Results { get; set; }
    }
}
