using ForkliftHub.ViewModels;

namespace ForkliftHub.Services.Interfaces
{
    public interface IEngineService
    {
        Task<List<EngineViewModel>> GetAllEnginesAsync();
        Task<bool> EngineExistsAsync(string name, int? excludeId = null);
        Task CreateEngineAsync(EngineViewModel vm);
        Task<EngineViewModel?> GetEngineByIdAsync(int id);
        Task UpdateEngineAsync(EngineViewModel vm);
        Task DeleteEngineAsync(int id);
    }
}
