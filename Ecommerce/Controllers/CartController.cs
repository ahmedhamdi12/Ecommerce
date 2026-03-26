using System.Text.Json;
using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }
        private List<CartItem> GetCart()
        {
            var sessionData = HttpContext.Session.GetString("Cart");

            if (sessionData == null)
                return new List<CartItem>();

            return JsonSerializer.Deserialize<List<CartItem>>(sessionData);
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
        }

        public IActionResult AddToCart(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            var cart = GetCart();

            var existingItem = cart.FirstOrDefault(c => c.ProductId == id);

            if (product == null)
            {
                return NotFound(); 
            }

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 1,
                    ImageUrl = product.ImageUrl,

                });
            }

            SaveCart(cart);
            return RedirectToAction("Index", "Shop");
        }
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        public IActionResult Remove(int id)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(c => c.ProductId == id);

            if (item != null)
            {
                cart.Remove(item);
            }
            SaveCart(cart);

            return RedirectToAction("Index");
        }

        public IActionResult Increase(int id) { 
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == id);
            if (item != null)
            {
                item.Quantity++;
            }
            SaveCart(cart);
            return RedirectToAction("Index");
        }
        public IActionResult Decrease(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == id);
            if (item != null)
            {
                item.Quantity--;
                if (item.Quantity <= 0)
                {
                    cart.Remove(item);
                }
            }
            SaveCart(cart);
            return RedirectToAction("Index");
        }
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");
        }
    }
}
