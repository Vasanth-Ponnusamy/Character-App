using CharacterApp.Data.Model;
using CharacterApp.Models;
using CharacterApp.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CharacterApp.Controllers
{
    public class CharactersController : Controller
    {
        private readonly ICharacterService _characterService;

        public CharactersController(ICharacterService characterService)
        {
            _characterService = characterService;
        }


        public async Task<IActionResult> Index()
        {
            var characters = await _characterService.GetCharactersAsync();
            return View(characters);
        }

        public IActionResult Create()
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

            ViewBag.SpeciesList = new SelectList(speciesList);
            ViewBag.genderList = new SelectList(genderList);

            return View(new CharacterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CharacterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var character = new Character
                //{
                //    Name = model.Name,
                //    Species = model.Species,
                //    Gender = model.Gender,
                //    LocationId = model.LocationId,
                //    OriginId = model.OriginId
                //};
                //_characterService.InsertCharactersAsync();

                //_context.Characters.Add(character);
                //_context.SaveChanges();

                return RedirectToAction("Index");
            }

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
