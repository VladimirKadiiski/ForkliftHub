using ForkliftHub.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Areas.User.Controllers
{
    [Area("User")]
    public class ProductsController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: /User/Products
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.MachineModel)
                .Include(p => p.Category)
                .Include(p => p.ProductType)
                .Include(p => p.Engine)
                .Include(p => p.MastType)
                .ToListAsync();

            return View(products);
        }

        // GET: /User/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.MachineModel)
                .Include(p => p.Category)
                .Include(p => p.ProductType)
                .Include(p => p.Engine)
                .Include(p => p.MastType)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }
    }
}
