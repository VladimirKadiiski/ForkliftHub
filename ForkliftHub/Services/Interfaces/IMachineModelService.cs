using ForkliftHub.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ForkliftHub.Services.Interfaces
{
    public interface IMachineModelService
    {
        Task<IEnumerable<MachineModelViewModel>> GetAllMachineModelsAsync();
        Task<IEnumerable<SelectListItem>> GetBrandsSelectListAsync();
        Task<bool> MachineModelExistsAsync(string name, int? excludeId = null);
        Task CreateMachineModelAsync(MachineModelViewModel vm);
        Task<MachineModelViewModel?> GetMachineModelByIdAsync(int id);
        Task UpdateMachineModelAsync(MachineModelViewModel vm);
        Task<MachineModelDeleteViewModel?> GetMachineModelDeleteViewModelAsync(int id);
        Task DeleteMachineModelAsync(int id);
    }
}
