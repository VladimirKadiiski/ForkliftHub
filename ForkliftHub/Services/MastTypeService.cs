using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.Services.Interfaces;
using ForkliftHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Services
{
    public class MastTypeService(ApplicationDbContext context) : IMastTypeService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<MastTypeViewModel>> GetAllMastTypesAsync()
        {
            return await _context.MastTypes
                .Select(mt => new MastTypeViewModel
                {
                    Id = mt.Id,
                    Name = mt.Name
                }).ToListAsync();
        }

        public async Task<bool> MastTypeExistsAsync(string name, int? excludeId = null)
        {
            return await _context.MastTypes
                .AnyAsync(mt => mt.Name == name && (!excludeId.HasValue || mt.Id != excludeId.Value));
        }

        public async Task CreateMastTypeAsync(MastTypeViewModel vm)
        {
            var mastType = new MastType { Name = vm.Name };
            _context.MastTypes.Add(mastType);
            await _context.SaveChangesAsync();
        }

        public async Task<MastTypeViewModel?> GetMastTypeByIdAsync(int id)
        {
            var mastType = await _context.MastTypes.FindAsync(id);
            if (mastType == null) return null;

            return new MastTypeViewModel
            {
                Id = mastType.Id,
                Name = mastType.Name
            };
        }

        public async Task UpdateMastTypeAsync(MastTypeViewModel vm)
        {
            var mastType = await _context.MastTypes.FindAsync(vm.Id);
            if (mastType == null) return;

            mastType.Name = vm.Name;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMastTypeAsync(int id)
        {
            var mastType = await _context.MastTypes.FindAsync(id);
            if (mastType == null) return;

            _context.MastTypes.Remove(mastType);
            await _context.SaveChangesAsync();
        }
    }
}
