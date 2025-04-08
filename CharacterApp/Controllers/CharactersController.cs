using CharacterApp.Data.Model;
using CharacterApp.Models;
using CharacterApp.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace CharacterApp.Controllers
{
    public class CharactersController : Controller
    {
        private readonly ICharacterService _characterService;

        public CharactersController(ICharacterService characterService)
        {
            _characterService = characterService;
        }


        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 50;  // records per page
            int pageNumber = page ?? 1;  // if null, default to 1

            var characters = await _characterService.GetCharactersAsync();

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

            //if (ModelState.IsValid)
            //{
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


                return RedirectToAction("Index");
            //}
            //if (!ModelState.IsValid)
            //{
            //    var errorDetails = ModelState
            //        .Where(x => x.Value.Errors.Count > 0)
            //        .Select(x => new { x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList() })
            //        .ToList();

            //    return BadRequest(errorDetails); // This will return a detailed list of the validation errors
            //}

            // Repopulate Dropdowns if validation fails
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
