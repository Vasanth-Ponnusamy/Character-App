using CharacterApp.Data.Model;
using CharacterApp.Services.Interface;
using Microsoft.AspNetCore.Mvc;

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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Character character)
        {
            return null;
            //if (ModelState.IsValid)
            //{
            //    _context.Add(character);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(character);
        }
    }

}
