using AutoMapper;
using CharacterApp.Data.Model;
using CharacterApp.DataSync.Model;

namespace CharacterApp.DataSync
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Character, CharacterResponse>();
        }
    }

}
