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
        private static readonly string CacheTimeKey = "CharacterListCacheTime";
        private static DateTime _lastFetchTime = DateTime.MinValue;

        public CharactersController(ICharacterService characterService, IMemoryCache memoryCache)
        {
            _characterService = characterService;
            _cache = memoryCache;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            bool fromDb = false;
            List<CharacterViewModel> characters;

            if (!_cache.TryGetValue(CacheKey, out characters) ||
                 !_cache.TryGetValue(CacheTimeKey, out DateTime lastFetchTime) ||
                    DateTime.UtcNow - lastFetchTime > TimeSpan.FromMinutes(5))
            {
                characters = await _characterService.GetCharactersAsync();
                _cache.Set(CacheKey, characters);
                _lastFetchTime = DateTime.UtcNow;
                fromDb = true;
            }

            Response.Headers["from-database"] = fromDb.ToString().ToLower();

            int pageSize = 10;
            int pageNumber = page ?? 1;
            var pagedCharacters = characters.ToPagedList(pageNumber, pageSize);

            return View(pagedCharacters);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var locations = await _characterService.GetLocationInfos();
            var episodes = await _characterService.GetEpisodes();
            ViewBag.Locations = new SelectList(locations, "Id", "Name");
            ViewBag.Episodes = new SelectList(episodes, "Id", "Url");

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

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> FromPlanet(string name)
        {
            var characters = await _characterService.GetCharactersByPlanetAsync(name);
            ViewBag.PlanetName = name;
            return View(characters); 
        }

    }

}
