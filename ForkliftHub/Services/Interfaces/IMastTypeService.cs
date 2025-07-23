using ForkliftHub.ViewModels;

namespace ForkliftHub.Services.Interfaces
{
    public interface IMastTypeService
    {
        Task<List<MastTypeViewModel>> GetAllMastTypesAsync();
        Task<bool> MastTypeExistsAsync(string name, int? excludeId = null);
        Task CreateMastTypeAsync(MastTypeViewModel vm);
        Task<MastTypeViewModel?> GetMastTypeByIdAsync(int id);
        Task UpdateMastTypeAsync(MastTypeViewModel vm);
        Task DeleteMastTypeAsync(int id);
    }
}
