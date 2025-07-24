using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.Services;
using ForkliftHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Tests.Services
{
    public class EngineServiceTests
    {
        private static ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateAsync_AddsEngine()
        {
            var context = GetDbContext();
            var service = new EngineService(context);
            var vm = new EngineViewModel { Name = "Diesel" };

            await service.CreateEngineAsync(vm);

            Assert.Single(context.Engines);
            Assert.Equal("Diesel", context.Engines.First().Type);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllEngines()
        {
            var context = GetDbContext();
            context.Engines.AddRange(new Engine { Type = "Gas" }, new Engine { Type = "Electric" });
            await context.SaveChangesAsync();

            var service = new EngineService(context);
            var engines = (await service.GetAllEnginesAsync()).ToList();

            Assert.Equal(2, engines.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectEngine()
        {
            var context = GetDbContext();
            var engine = new Engine { Type = "Hydraulic" };
            context.Engines.Add(engine);
            await context.SaveChangesAsync();

            var service = new EngineService(context);
            var result = await service.GetEngineByIdAsync(engine.Id);

            Assert.NotNull(result);
            Assert.Equal("Hydraulic", result!.Name);
        }

        [Fact]
        public async Task ExistsByNameAsync_ReturnsTrue_IfExists()
        {
            var context = GetDbContext();
            context.Engines.Add(new Engine { Type = "LPG" });
            await context.SaveChangesAsync();

            var service = new EngineService(context);
            var exists = await service.EngineExistsAsync("LPG");

            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsByNameAsync_ReturnsFalse_IfNotExists()
        {
            var context = GetDbContext();
            var service = new EngineService(context);

            var exists = await service.EngineExistsAsync("Steam");

            Assert.False(exists);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesEngine()
        {
            var context = GetDbContext();
            var engine = new Engine { Type = "OldEngine" };
            context.Engines.Add(engine);
            await context.SaveChangesAsync();

            var service = new EngineService(context);
            var updatedVm = new EngineViewModel { Id = engine.Id, Name = "UpdatedEngine" };

            await service.UpdateEngineAsync(updatedVm);

            var updated = await context.Engines.FindAsync(engine.Id);
            Assert.Equal("UpdatedEngine", updated!.Type);
        }

        [Fact]
        public async Task DeleteAsync_RemovesEngine()
        {
            var context = GetDbContext();
            var engine = new Engine { Type = "ToRemove" };
            context.Engines.Add(engine);
            await context.SaveChangesAsync();

            var service = new EngineService(context);
            await service.DeleteEngineAsync(engine.Id);

            Assert.Empty(context.Engines);
        }
    }
}
