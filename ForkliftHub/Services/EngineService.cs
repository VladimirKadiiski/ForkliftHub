using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.Services.Interfaces;
using ForkliftHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Services
{
    public class EngineService(ApplicationDbContext context) : IEngineService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<EngineViewModel>> GetAllEnginesAsync()
        {
            return await _context.Engines
                .Select(e => new EngineViewModel
                {
                    Id = e.Id,
                    Name = e.Type
                }).ToListAsync();
        }

        public async Task<bool> EngineExistsAsync(string name, int? excludeId = null)
        {
            return await _context.Engines
                .AnyAsync(e => e.Type == name && (!excludeId.HasValue || e.Id != excludeId.Value));
        }

        public async Task CreateEngineAsync(EngineViewModel vm)
        {
            var engine = new Engine { Type = vm.Name };
            _context.Engines.Add(engine);
            await _context.SaveChangesAsync();
        }

        public async Task<EngineViewModel?> GetEngineByIdAsync(int id)
        {
            var engine = await _context.Engines.FindAsync(id);
            if (engine == null) return null;

            return new EngineViewModel
            {
                Id = engine.Id,
                Name = engine.Type
            };
        }

        public async Task UpdateEngineAsync(EngineViewModel vm)
        {
            var engine = await _context.Engines.FindAsync(vm.Id);
            if (engine == null) return;

            engine.Type = vm.Name;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEngineAsync(int id)
        {
            var engine = await _context.Engines.FindAsync(id);
            if (engine == null) return;

            _context.Engines.Remove(engine);
            await _context.SaveChangesAsync();
        }
    }
}
