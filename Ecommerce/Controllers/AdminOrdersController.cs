using Ecommerce.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminOrdersController : Controller
    {

        private readonly ApplicationDbContext _context;

        public AdminOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var orders = _context.Orders.Include(o => o.OrderItems).ToList();
            return View(orders);
        }

        public IActionResult UpdateStatus(int id, string status)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);

            if (order != null)
            {
                order.Status = status;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
