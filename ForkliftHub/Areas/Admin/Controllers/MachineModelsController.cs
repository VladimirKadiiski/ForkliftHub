using ForkliftHub.Services.Interfaces;
using ForkliftHub.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ForkliftHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MachineModelsController(IMachineModelService machineModelService) : Controller
    {
        private readonly IMachineModelService _machineModelService = machineModelService;

        public async Task<IActionResult> Index()
        {
            var models = await _machineModelService.GetAllMachineModelsAsync();
            return View(models);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new MachineModelViewModel
            {
                Brands = (await _machineModelService.GetBrandsSelectListAsync()).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MachineModelViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Brands = (await _machineModelService.GetBrandsSelectListAsync()).ToList();
                return View(vm);
            }

            if (await _machineModelService.MachineModelExistsAsync(vm.Name))
            {
                ModelState.AddModelError("Name", "A model with this name already exists.");
                vm.Brands = (await _machineModelService.GetBrandsSelectListAsync()).ToList();
                return View(vm);
            }

            await _machineModelService.CreateMachineModelAsync(vm);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var vm = await _machineModelService.GetMachineModelByIdAsync(id.Value);
            if (vm == null) return NotFound();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MachineModelViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                vm.Brands = (await _machineModelService.GetBrandsSelectListAsync()).ToList();
                return View(vm);
            }

            if (await _machineModelService.MachineModelExistsAsync(vm.Name, vm.Id))
            {
                ModelState.AddModelError("Name", "A model with this name already exists.");
                vm.Brands = (await _machineModelService.GetBrandsSelectListAsync()).ToList();
                return View(vm);
            }

            await _machineModelService.UpdateMachineModelAsync(vm);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var vm = await _machineModelService.GetMachineModelDeleteViewModelAsync(id.Value);
            if (vm == null) return NotFound();

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _machineModelService.DeleteMachineModelAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
