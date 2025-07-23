using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.Services.Interfaces;
using ForkliftHub.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForkliftHub.Services
{
    public class MachineModelService(ApplicationDbContext context) : IMachineModelService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<MachineModelViewModel>> GetAllMachineModelsAsync()
        {
            return await _context.MachineModels
                .Include(m => m.Brand)
                .Select(m => new MachineModelViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    BrandId = m.BrandId,
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetBrandsSelectListAsync()
        {
            return await _context.Brands
                .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name })
                .ToListAsync();
        }

        public async Task<bool> MachineModelExistsAsync(string name, int? excludeId = null)
        {
            return await _context.MachineModels
                .AnyAsync(m => m.Name == name && (!excludeId.HasValue || m.Id != excludeId.Value));
        }

        public async Task CreateMachineModelAsync(MachineModelViewModel vm)
        {
            var model = new MachineModel
            {
                Name = vm.Name,
                BrandId = vm.BrandId
            };
            _context.MachineModels.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task<MachineModelViewModel?> GetMachineModelByIdAsync(int id)
        {
            var model = await _context.MachineModels.FindAsync(id);
            if (model == null) return null;

            var brands = await GetBrandsSelectListAsync();

            return new MachineModelViewModel
            {
                Id = model.Id,
                Name = model.Name,
                BrandId = model.BrandId,
                Brands = brands.ToList()
            };
        }

        public async Task UpdateMachineModelAsync(MachineModelViewModel vm)
        {
            var model = await _context.MachineModels.FindAsync(vm.Id);
            if (model == null) return;

            model.Name = vm.Name;
            model.BrandId = vm.BrandId;

            _context.MachineModels.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task<MachineModelDeleteViewModel?> GetMachineModelDeleteViewModelAsync(int id)
        {
            var model = await _context.MachineModels
                .Include(m => m.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null) return null;

            return new MachineModelDeleteViewModel
            {
                Id = model.Id,
                Name = model.Name,
                BrandName = model.Brand.Name
            };
        }

        public async Task DeleteMachineModelAsync(int id)
        {
            var model = await _context.MachineModels.FindAsync(id);
            if (model == null) return;

            _context.MachineModels.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}
