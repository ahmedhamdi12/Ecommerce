using System.Text.Json;
using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult CheckOut()
        {
            var cart = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cart)) return RedirectToAction("Index", "Cart");

            var cartItems = JsonSerializer.Deserialize<List<CartItem>>(cart);

            var order = new Order
            {
                UserId = _userManager.GetUserId(User),
                Total = cartItems.Sum(i => i.Price * i.Quantity),
                OrderItems = cartItems.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Name = i.Name,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    ImageUrl = i.ImageUrl,
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Details" , new {id= order.Id});
        }

        public IActionResult Details(int id)
        {
            var order = _context.Orders.Include(o => o.OrderItems)
                .FirstOrDefault(o => o.Id == id && o.UserId == _userManager.GetUserId(User));

            if (order == null) return NotFound();

            return View(order);

        }

        public IActionResult History()
        {
            var userId = _userManager.GetUserId(User);
            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId  == userId)
                .ToList();

            return View(orders);
        }
    }
}
