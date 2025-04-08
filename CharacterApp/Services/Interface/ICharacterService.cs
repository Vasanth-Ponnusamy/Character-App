using CharacterApp.Data.Model;
using CharacterApp.Models;

namespace CharacterApp.Services.Interface
{
    public interface ICharacterService
    {
        Task<List<CharacterViewModel>> GetCharactersAsync();
        Task InsertCharactersAsync(List<CharacterViewModel> characters);
    }
}
