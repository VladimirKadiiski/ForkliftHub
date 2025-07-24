using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.Services;
using ForkliftHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Tests.Services
{
    public class MachineModelServiceTests
    {
        private static ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateAsync_AddsMachineModel()
        {
            var context = GetDbContext();
            var service = new MachineModelService(context);
            var vm = new MachineModelViewModel { Name = "E35" };

            await service.CreateMachineModelAsync(vm);

            Assert.Single(context.MachineModels);
            Assert.Equal("E35", context.MachineModels.First().Name);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllModels()
        {
            var context = GetDbContext();
            context.MachineModels.AddRange(
                new MachineModel { Name = "RX60" },
                new MachineModel { Name = "H30" });
            await context.SaveChangesAsync();

            var service = new MachineModelService(context);
            var result = (await service.GetAllMachineModelsAsync()).ToList();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectModel()
        {
            var context = GetDbContext();
            var model = new MachineModel { Name = "E20" };
            context.MachineModels.Add(model);
            await context.SaveChangesAsync();

            var service = new MachineModelService(context);
            var result = await service.GetMachineModelByIdAsync(model.Id);

            Assert.NotNull(result);
            Assert.Equal("E20", result!.Name);
        }

        [Fact]
        public async Task ExistsByNameAsync_ReturnsTrue_IfExists()
        {
            var context = GetDbContext();
            context.MachineModels.Add(new MachineModel { Name = "T16" });
            await context.SaveChangesAsync();

            var service = new MachineModelService(context);
            var exists = await service.MachineModelExistsAsync("T16");

            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsByNameAsync_ReturnsFalse_IfNotExists()
        {
            var context = GetDbContext();
            var service = new MachineModelService(context);

            var exists = await service.MachineModelExistsAsync("NonExisting");

            Assert.False(exists);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesMachineModel()
        {
            var context = GetDbContext();
            var model = new MachineModel { Name = "OldName" };
            context.MachineModels.Add(model);
            await context.SaveChangesAsync();

            var service = new MachineModelService(context);
            var vm = new MachineModelViewModel { Id = model.Id, Name = "NewName" };

            await service.UpdateMachineModelAsync(vm);

            var updated = await context.MachineModels.FindAsync(model.Id);
            Assert.Equal("NewName", updated!.Name);
        }

        [Fact]
        public async Task DeleteAsync_RemovesMachineModel()
        {
            var context = GetDbContext();
            var model = new MachineModel { Name = "TempModel" };
            context.MachineModels.Add(model);
            await context.SaveChangesAsync();

            var service = new MachineModelService(context);
            await service.DeleteMachineModelAsync(model.Id);

            Assert.Empty(context.MachineModels);
        }
    }
}
