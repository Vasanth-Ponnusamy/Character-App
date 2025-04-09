using CharacterApp.Data.Context;
using CharacterApp.Data.Model;
using CharacterApp.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace CharacterApp.Test.Data.Repository
{
    public class CharacterRepositoryTest
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task InsertCharacterAsync_SavesCharacterWithDefaults()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CharacterRepository(context);

            var character = new Character
            {
                Name = "Rick",
                Species = "Human",
                Gender = "Male",
                Episodes = "1,2"
            };

            // Act
            await repository.InsertCharacterAsync(character);

            // Assert
            var saved = await context.Characters.FirstOrDefaultAsync();
            Assert.NotNull(saved);
            Assert.Equal("Alive", saved.Status);
            Assert.True(saved.Created <= DateTime.UtcNow);
        }

        [Fact]
        public async Task ClearData_RemovesAllEntities()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Characters.Add(new Character { Name = "Test", Species = "test", Gender = "test", Episodes = "1" });
            context.Locations.Add(new LocationInfo { Name = "test", Url = "test" });
            context.Episodes.Add(new Episode { Url = "ep1" });
            await context.SaveChangesAsync();

            var repository = new CharacterRepository(context);

            // Act
            await repository.ClearData();

            // Assert
            Assert.Empty(context.Characters);
            Assert.Empty(context.Locations);
            Assert.Empty(context.Episodes);
        }

        [Fact]
        public async Task InsertAndGetLocationInfo_ShouldReturnExistingIfExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Locations.Add(new LocationInfo { Name = "Earth", Url = "url-earth" });
            await context.SaveChangesAsync();

            var repo = new CharacterRepository(context);

            // Act
            var result = await repo.InsertAndGetLocationInfo("Earth", "url-earth");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Earth", result.Name);
        }

        [Fact]
        public async Task InsertAndGetLocationInfo_ShouldInsertIfNotExists()
        {
            var context = GetInMemoryDbContext();
            var repo = new CharacterRepository(context);

            var result = await repo.InsertAndGetLocationInfo("Mars", "url-mars");

            Assert.NotNull(result);
            Assert.Equal("Mars", result.Name);
            Assert.Single(context.Locations);
        }
        [Fact]
        public async Task InsertAndGetEpisode_ShouldInsertAndReturnIds()
        {
            var context = GetInMemoryDbContext();
            var repo = new CharacterRepository(context);
            var urls = new List<string> { "ep1", "ep2" };

            var result = await repo.InsertAndGetEpisode(urls);

            Assert.Equal(2, result.Count);
            Assert.Equal(2, await context.Episodes.CountAsync());
        }

        [Fact]
        public async Task InsertAndGetEpisode_ShouldReturnExistingId_IfExists()
        {
            var context = GetInMemoryDbContext();
            context.Episodes.Add(new Episode { Url = "ep1" });
            await context.SaveChangesAsync();

            var repo = new CharacterRepository(context);
            var result = await repo.InsertAndGetEpisode(new List<string> { "ep1", "ep2" });

            Assert.Equal(2, result.Count);
            Assert.Equal(2, await context.Episodes.CountAsync());
        }
        [Fact]
        public async Task GetLocationInfoById_ReturnsCorrectLocation()
        {
            var context = GetInMemoryDbContext();
            context.Locations.Add(new LocationInfo { Id = 10, Name = "Earth", Url = "test" });
            await context.SaveChangesAsync();

            var repo = new CharacterRepository(context);
            var result = await repo.GetLocationInfoById(10);

            Assert.NotNull(result);
            Assert.Equal("Earth", result.Name);
        }
        [Fact]
        public async Task GetEpisodesByIds_ReturnsUrls()
        {
            var context = GetInMemoryDbContext();
            context.Episodes.AddRange(
                new Episode { Id = 1, Url = "ep1" },
                new Episode { Id = 2, Url = "ep2" }
            );
            await context.SaveChangesAsync();

            var repo = new CharacterRepository(context);
            var result = await repo.GetEpisodesByIds(new List<int> { 1 });

            Assert.Single(result);
            Assert.Equal("ep1", result.First());
        }
        [Fact]
        public async Task GetLocations_ReturnsAllLocations()
        {
            var context = GetInMemoryDbContext();
            context.Locations.Add(new LocationInfo { Name = "Earth", Url = "test" });
            await context.SaveChangesAsync();

            var repo = new CharacterRepository(context);
            var result = await repo.GetLocations();

            Assert.Single(result);
        }
        [Fact]
        public async Task GetEpisodes_ReturnsAllEpisodes()
        {
            var context = GetInMemoryDbContext();
            context.Episodes.Add(new Episode { Url = "ep1" });
            await context.SaveChangesAsync();

            var repo = new CharacterRepository(context);
            var result = await repo.GetEpisodes();

            Assert.Single(result);
        }

    }

}
