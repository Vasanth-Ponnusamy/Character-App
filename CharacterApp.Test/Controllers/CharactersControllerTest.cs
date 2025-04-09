using CharacterApp.Controllers;
using CharacterApp.Data.Model;
using CharacterApp.Models;
using CharacterApp.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using X.PagedList;

namespace CharacterApp.Test.Controllers
{
    public class CharactersControllerTest
    {
        private readonly Mock<ICharacterService> _characterServiceMock = new();
        private readonly CharactersController _characterController;
        public CharactersControllerTest()
        {
            var mockCache = new MemoryCache(new MemoryCacheOptions());
            _characterController = new CharactersController(_characterServiceMock.Object, mockCache);
            _characterController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

        }
        [Fact]
        public async Task IndexReturnsPagedCharactersAndSetsHeader()
        {
            // Arrange

            var characters = Enumerable.Range(1, 20).Select(i => new CharacterViewModel { Id = i, Name = $"Character {i}" }).ToList();

            _characterServiceMock.Setup(s => s.GetCharactersAsync()).ReturnsAsync(characters);

            // Act
            var result = await _characterController.Index(1) as ViewResult;

            // Assert
            Assert.NotNull(result);

            var pagedList = Assert.IsAssignableFrom<IPagedList<CharacterViewModel>>(result.Model);

            Assert.Equal(10, pagedList.Count); 
            var fromDbHeader = _characterController.Response.Headers["from-database"];
            Assert.Equal("true", fromDbHeader);
        }

        [Fact]
        public async Task IndexUsesCacheWhenFresh()
        {
            // Arrange
            var mockService = new Mock<ICharacterService>();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var characters = Enumerable.Range(1, 20).Select(i => new CharacterViewModel { Id = i, Name = $"Char {i}" }).ToList();

            memoryCache.Set("CharacterListCache", characters, TimeSpan.FromMinutes(10));
            memoryCache.Set("CharacterListCacheTime", DateTime.UtcNow, TimeSpan.FromMinutes(10));

            var controller = new CharactersController(mockService.Object, memoryCache);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = await controller.Index(1) as ViewResult;

            // Assert
            mockService.Verify(s => s.GetCharactersAsync(), Times.Never);
            Assert.Equal("false", controller.Response.Headers["from-database"]);
        }

        [Fact]
        public async Task Create_ReturnsViewWithViewBagsPopulated()
        {
            // Arrange

            var locationList = new List<LocationInfo>
            {
                new LocationInfo { Id = 1, Name = "Earth" },
                new LocationInfo { Id = 2, Name = "Mars" }
            };

                    var episodeList = new List<Episode>
            {
                new Episode { Id = 1, Url = "episode/1" },
                new Episode { Id = 2, Url = "episode/2" }
            };

            _characterServiceMock.Setup(s => s.GetLocationInfos()).ReturnsAsync(locationList);
            _characterServiceMock.Setup(s => s.GetEpisodes()).ReturnsAsync(episodeList);


            // Act
            var result = await _characterController.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<CharacterViewModel>(viewResult.Model);

            var locationSelectList = Assert.IsType<SelectList>(_characterController.ViewBag.Locations);
            var episodeSelectList = Assert.IsType<SelectList>(_characterController.ViewBag.Episodes);

            Assert.NotNull(locationSelectList);
            Assert.NotNull(episodeSelectList);
        }

        [Fact]
        public async Task Create_Post_ValidModel_InsertsCharacterAndRedirects()
        {
            // Arrange
            var model = new CharacterViewModel
            {
                Name = "Rick",
                Species = "Human",
                Gender = "Male",
                LocationId = 1,
                OriginId = 2,
                Episodes = new List<string> { "ep1", "ep2" }
            };

            // Act
            var result = await _characterController.Create(model);

            // Assert
            _characterServiceMock.Verify(s => s.InsertCharactersAsync(It.Is<Character>(c =>
                c.Name == model.Name &&
                c.Species == model.Species &&
                c.Gender == model.Gender &&
                c.LocationId == model.LocationId &&
                c.OriginId == model.OriginId &&
                c.Episodes == "ep1, ep2"
            )), Times.Once);

            // Check redirect
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

    }
}
