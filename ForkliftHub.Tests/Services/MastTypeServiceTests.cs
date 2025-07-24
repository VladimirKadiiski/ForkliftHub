using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.Services;
using ForkliftHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Tests.Services
{
    public class MastTypeServiceTests
    {
        private static ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateAsync_AddsMastType()
        {
            var context = GetDbContext();
            var service = new MastTypeService(context);
            var vm = new MastTypeViewModel { Name = "Triple" };

            await service.CreateMastTypeAsync(vm);

            Assert.Single(context.MastTypes);
            Assert.Equal("Triple", context.MastTypes.First().Name);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllMastTypes()
        {
            var context = GetDbContext();
            context.MastTypes.AddRange(
                new MastType { Name = "Simplex" },
                new MastType { Name = "Duplex" });
            await context.SaveChangesAsync();

            var service = new MastTypeService(context);
            var result = (await service.GetAllMastTypesAsync()).ToList();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectViewModel()
        {
            var context = GetDbContext();
            var mast = new MastType { Name = "Quad" };
            context.MastTypes.Add(mast);
            await context.SaveChangesAsync();

            var service = new MastTypeService(context);
            var result = await service.GetMastTypeByIdAsync(mast.Id);

            Assert.NotNull(result);
            Assert.Equal("Quad", result!.Name);
        }

        [Fact]
        public async Task ExistsByNameAsync_ReturnsTrue_IfExists()
        {
            var context = GetDbContext();
            context.MastTypes.Add(new MastType { Name = "Telescopic" });
            await context.SaveChangesAsync();

            var service = new MastTypeService(context);
            var exists = await service.MastTypeExistsAsync("Telescopic");

            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsByNameAsync_ReturnsFalse_IfNotExists()
        {
            var context = GetDbContext();
            var service = new MastTypeService(context);

            var exists = await service.MastTypeExistsAsync("NonExistent");

            Assert.False(exists);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesName()
        {
            var context = GetDbContext();
            var mast = new MastType { Name = "OldMast" };
            context.MastTypes.Add(mast);
            await context.SaveChangesAsync();

            var service = new MastTypeService(context);
            var vm = new MastTypeViewModel { Id = mast.Id, Name = "UpdatedMast" };

            await service.UpdateMastTypeAsync(vm);

            var updated = await context.MastTypes.FindAsync(mast.Id);
            Assert.Equal("UpdatedMast", updated!.Name);
        }

        [Fact]
        public async Task DeleteAsync_RemovesMastType()
        {
            var context = GetDbContext();
            var mast = new MastType { Name = "Temporary" };
            context.MastTypes.Add(mast);
            await context.SaveChangesAsync();

            var service = new MastTypeService(context);
            await service.DeleteMastTypeAsync(mast.Id);

            Assert.Empty(context.MastTypes);
        }
    }
}
