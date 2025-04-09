using CharacterApp.Data.Model;
using CharacterApp.Models;
using CharacterApp.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using X.PagedList.Extensions;

namespace CharacterApp.Controllers
{
    public class CharactersController : Controller
    {
        private readonly ICharacterService _characterService;
        private readonly IMemoryCache _cache;
        private static readonly string CacheKey = "CharacterListCache";
        private static DateTime _lastFetchTime = DateTime.MinValue;

        public CharactersController(ICharacterService characterService, IMemoryCache memoryCache)
        {
            _characterService = characterService;
            _cache = memoryCache;
        }


        public async Task<IActionResult> Index(int? page)
        {
            bool fromDb = false;
            List<CharacterViewModel> characters;

            if (!_cache.TryGetValue(CacheKey, out characters) || DateTime.UtcNow - _lastFetchTime > TimeSpan.FromMinutes(5))
            {
                characters = await _characterService.GetCharactersAsync();
                _cache.Set(CacheKey, characters);
                _lastFetchTime = DateTime.UtcNow;
                fromDb = true;
            }

            // Add response header
            Response.Headers["from-database"] = fromDb.ToString().ToLower();

            int pageSize = 10;  
            int pageNumber = page ?? 1; 
            var pagedCharacters = characters.ToPagedList(pageNumber, pageSize);

            return View(pagedCharacters);
        }

        public async Task<IActionResult> Create()
        {
          var speciesList = new List<string>
            {
                "Alien",
                "Animal",
                "Cronenberg",
                "Human",
                "Humanoid",
                "Mythological Creature",
                "Poopybutthole",
                "Robot",
                "unknown"
            };
            var genderList = new List<string>
            {
                "Female",
                "Genderless",
                "Male",
                "unknown"

            };
            var locations = await _characterService.GetLocationInfos();
            var episodes = await _characterService.GetEpisodes();
            ViewBag.Locations = new SelectList(locations, "Id", "Name");
            ViewBag.Episodes = new SelectList(episodes, "Id", "Url");


            ViewBag.SpeciesList = new SelectList(speciesList);
            ViewBag.genderList = new SelectList(genderList);

            return View(new CharacterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CharacterViewModel model)
        {
                var character = new Character
                {
                    Name = model.Name,
                    Species = model.Species,
                    Gender = model.Gender,
                    LocationId = model.LocationId,
                    OriginId = model.OriginId,
                    Episodes = string.Join(", ", model.Episodes)
                };
               await _characterService.InsertCharactersAsync(character);
            _cache.Remove(CacheKey);
            _lastFetchTime = DateTime.MinValue;

            return RedirectToAction("Index");

            ViewBag.SpeciesList = new SelectList(new List<string>
             {
                "Alien", "Animal", "Cronenberg", "Human", "Humanoid",
                "Mythological Creature", "Poopybutthole", "Robot", "unknown"
             });

            ViewBag.GenderList = new SelectList(new List<string>
            {
                "Female", "Genderless", "Male", "unknown"
            });

            return View(model);
        }

    }

}
