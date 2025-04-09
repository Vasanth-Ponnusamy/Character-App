using CharacterApp.Data.Model;
using CharacterApp.Data.Repository;
using CharacterApp.Models;
using CharacterApp.Services;
using Moq;

namespace CharacterApp.Test.Services
{
    public class CharacterServiceTest
    {
        private readonly Mock<ICharacterRepository> _mockCharecterRepository = new();
        private readonly CharacterService _characterService;
        public CharacterServiceTest()
        {
            _characterService = new CharacterService(_mockCharecterRepository.Object);
        }

        [Fact]
        [Trait(nameof(CharacterService), nameof(CharacterService.GetCharactersAsync))]
        public async Task GetCharactersAsyncShouldReturnsCharecters()
        {
            // Arrange
            var locationInfo = new LocationInfoViewModel()
            {
                Id = 1,
                Name = "location-1",
                Url = "test-url"
            };
            var locationInfoRepoResponse = new LocationInfo()
            {
                Id = 1,
                Name = "location-1",
                Url = "test-url"
            };
            var originInfo = new LocationInfoViewModel()
            {
                Id = 2,
                Name = "origin-1",
                Url = "test-url"
            };
            var originInfoRepoResponse = new LocationInfo()
            {
                Id = 2,
                Name = "origin-1",
                Url = "test-url"
            };
            var response = new List<CharacterViewModel>
            {
                new()
                {
                    Name = "test",
                    Location = locationInfo,
                    Origin = originInfo,
                    Episodes = new List<string>(){ "1","2"}
                }
            };
            var repoResponse = new List<Character>
            {
                new()
                {
                    Name = "test",
                    LocationId = 1,
                    OriginId = 2,
                    Episodes = "1,2",
                }
            };

            var episodes = new List<string>() { "url-1", "url-2" };

            _mockCharecterRepository.Setup(x => x.GetCharactersAsync()).ReturnsAsync(repoResponse).Verifiable(Times.Once);

            _mockCharecterRepository.Setup(x => x.GetLocationInfoById(1)).ReturnsAsync(locationInfoRepoResponse).Verifiable(Times.Once);
            _mockCharecterRepository.Setup(x => x.GetLocationInfoById(2)).ReturnsAsync(originInfoRepoResponse).Verifiable(Times.Once);
            _mockCharecterRepository.Setup(x => x.GetEpisodesByIds(new List<int>() { 1, 2 }))
                .ReturnsAsync(episodes).Verifiable(Times.Once);

            // Act
            var result = await _characterService.GetCharactersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<List<CharacterViewModel>>(result);
            Assert.Single(result);
            Assert.Equal(response[0].Name, result[0].Name);
            Assert.Equal(response[0].Location.Name, result[0].Location.Name);

            _mockCharecterRepository.Verify();
            _mockCharecterRepository.VerifyNoOtherCalls();
        }

        [Fact]
        [Trait(nameof(CharacterService), nameof(CharacterService.InsertCharactersAsync))]
        public async Task InsertCharactersAsyncShouldInsertCharecters()
        {
            /// Arrange
            var mockRepo = new Mock<ICharacterRepository>();
            var service = new CharacterService(mockRepo.Object);

            var newCharacter = new Character
            {
                Id = 1,
                Name = "Jon Snow"
            };

            // Act
            await service.InsertCharactersAsync(newCharacter);

            // Assert
            mockRepo.Verify(repo => repo.InsertCharacterAsync(newCharacter), Times.Once);
        }

        [Fact]
        [Trait(nameof(CharacterService), nameof(CharacterService.GetLocationInfos))]
        public async Task GetLocationInfosReturnsExpectedLocations()
        {
            // Arrange
            var expectedLocations = new List<LocationInfo>
        {
            new LocationInfo { Id = 1, Name = "Winterfell" },
            new LocationInfo { Id = 2, Name = "King's Landing" }
        };

            var mockRepo = new Mock<ICharacterRepository>();
            mockRepo.Setup(r => r.GetLocations()).ReturnsAsync(expectedLocations);

            var service = new CharacterService(mockRepo.Object);

            // Act
            var result = await service.GetLocationInfos();

            // Assert
            Assert.Equal(expectedLocations.Count, result.Count);
            Assert.Equal(expectedLocations[0].Name, result[0].Name);
            Assert.Equal(expectedLocations[1].Name, result[1].Name);

            mockRepo.Verify(r => r.GetLocations(), Times.Once);
        }

        [Fact]
        [Trait(nameof(CharacterService), nameof(CharacterService.GetEpisodes))]
        public async Task GetEpisodesReturnsExpectedEpisodes()
        {
            // Arrange
            var expectedEpisodes = new List<Episode>
        {
            new Episode { Id = 1, Url = "url-1" },
            new Episode { Id = 2, Url = "url-2" }
        };

            var mockRepo = new Mock<ICharacterRepository>();
            mockRepo.Setup(r => r.GetEpisodes()).ReturnsAsync(expectedEpisodes);

            var service = new CharacterService(mockRepo.Object);

            // Act
            var result = await service.GetEpisodes();

            // Assert
            Assert.Equal(expectedEpisodes.Count, result.Count);
            Assert.Equal(expectedEpisodes[0].Url, result[0].Url);
            Assert.Equal(expectedEpisodes[1].Url, result[1].Url);

            mockRepo.Verify(r => r.GetEpisodes(), Times.Once);
        }
    }
}
