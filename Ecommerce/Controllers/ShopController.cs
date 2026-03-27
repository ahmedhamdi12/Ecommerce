using Ecommerce.Data;
using Ecommerce.Data.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context){
            _context = context;
        }
        public IActionResult Index(string search, int? categoryId, int page=1)
        {
            int pageSize = 6;
            var products = _context.Products.Include(p => p.Category).AsQueryable();

            //search
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search));
            }

            //filter
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId);
            }

            //pagination
            int totalItems = products.Count();

            var pagedProducts = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages= (int)Math.Ceiling((double)totalItems / pageSize);

            ViewBag.Categories = _context.Categories.ToList();
            return View(pagedProducts);
        }

        public IActionResult Details(int id)
        {
            var product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            return View(product);
        }
    }
}
