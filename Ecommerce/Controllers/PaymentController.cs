using Ecommerce.Data;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Pay(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);

            if (order == null) return NotFound();

            return View(order);
        }

        [HttpPost]
        public IActionResult ConfirmPayment(int id, string paymentMethod)
        {
            var order = _context.Orders.FirstOrDefault(o =>o.Id == id);
            if (order == null) return NotFound();
            if (order != null)
            {
                order.PaymentStatus = "Paid";
                order.PaymentMethod = paymentMethod;
                order.Status = "Processing";

                _context.SaveChanges();

            }

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
