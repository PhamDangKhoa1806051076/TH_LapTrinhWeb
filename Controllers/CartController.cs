using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBanHang2.Extensions;
using WebBanHang2.Models;
using WebBanHang2.Repositories;
using System.Text.Json;

namespace WebBanHang2.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(IProductRepository productRepository, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
        }

        // Lấy giỏ hàng từ Cookie
        private List<CartItem> GetCartItems()
        {
            var cartCookie = HttpContext.Request.Cookies["CartCookie"];
            if (string.IsNullOrEmpty(cartCookie))
            {
                return new List<CartItem>();
            }
            try
            {
                var cart = JsonSerializer.Deserialize<List<CartItem>>(cartCookie);
                return cart ?? new List<CartItem>();
            }
            catch
            {
                return new List<CartItem>();
            }
        }

        // Lưu giỏ hàng vào Cookie
        private void SaveCartCookie(List<CartItem> ls)
        {
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30),
                HttpOnly = true,
                IsEssential = true
            };
            var json = JsonSerializer.Serialize(ls);
            HttpContext.Response.Cookies.Append("CartCookie", json, options);
        }

        public IActionResult Index()
        {
            var cart = GetCartItems();
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = _productRepository.GetById(productId);
            if (product == null)
            {
                return NotFound();
            }

            // Kiểm tra số lượng tồn kho
            if (product.Quantity < quantity)
            {
                TempData["Error"] = $"Sản phẩm {product.Name} chỉ còn {product.Quantity} cái trong kho.";
                return RedirectToAction("Display", "Product", new { id = productId });
            }

            var cart = GetCartItems();
            var cartItem = cart.Find(p => p.ProductId == productId);
            if (cartItem != null)
            {
                if (cartItem.Quantity + quantity > product.Quantity)
                {
                    TempData["Error"] = $"Không đủ số lượng trong kho.";
                }
                else
                {
                    cartItem.Quantity += quantity;
                    TempData["Success"] = "Đã cập nhật số lượng sản phẩm trong giỏ hàng.";
                }
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl ?? ""
                });
                TempData["Success"] = "Đã thêm sản phẩm vào giỏ hàng.";
            }

            SaveCartCookie(cart);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddToCartApi(int productId, int quantity = 1)
        {
            var product = _productRepository.GetById(productId);
            if (product == null)
            {
                return Json(new { success = false, message = "Không tìm thấy sản phẩm." });
            }

            // Kiểm tra số lượng tồn kho
            if (product.Quantity < quantity)
            {
                return Json(new { success = false, message = $"Sản phẩm {product.Name} chỉ còn {product.Quantity} cái trong kho." });
            }

            var cart = GetCartItems();
            var cartItem = cart.Find(p => p.ProductId == productId);
            if (cartItem != null)
            {
                if (cartItem.Quantity + quantity > product.Quantity)
                {
                    return Json(new { success = false, message = $"Không đủ số lượng trong kho." });
                }
                else
                {
                    cartItem.Quantity += quantity;
                }
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl ?? ""
                });
            }

            SaveCartCookie(cart);
            
            int cartCount = cart.Sum(x => x.Quantity);
            decimal cartTotal = cart.Sum(x => x.Price * x.Quantity);

            return Json(new { 
                success = true, 
                message = "Đã thêm vào giỏ hàng!",
                cartCount = cartCount,
                cartTotal = cartTotal,
                cartItems = cart // Return updated cart items to render the dropdown
            });
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCartItems();
            var cartItem = cart.Find(p => p.ProductId == productId);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
                SaveCartCookie(cart);
                TempData["Success"] = "Đã xóa sản phẩm khỏi giỏ hàng.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            if (quantity <= 0)
            {
                return RemoveFromCart(productId);
            }

            var product = _productRepository.GetById(productId);
            if (product == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm.";
                return RedirectToAction("Index");
            }

            if (product.Quantity < quantity)
            {
                TempData["Error"] = $"Sản phẩm {product.Name} chỉ còn {product.Quantity} cái trong kho.";
                return RedirectToAction("Index");
            }

            var cart = GetCartItems();
            var cartItem = cart.Find(p => p.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                SaveCartCookie(cart);
                TempData["Success"] = "Đã cập nhật số lượng.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantityApi(int productId, int quantity)
        {
            var cart = GetCartItems();
            if (quantity <= 0)
            {
                var itemToRemove = cart.Find(p => p.ProductId == productId);
                if (itemToRemove != null)
                {
                    cart.Remove(itemToRemove);
                    SaveCartCookie(cart);
                }
            }
            else
            {
                var product = _productRepository.GetById(productId);
                if (product == null) return Json(new { success = false, message = "Không tìm thấy sản phẩm." });

                if (product.Quantity < quantity)
                    return Json(new { success = false, message = $"Sản phẩm chỉ còn {product.Quantity} cái trong kho." });

                var cartItem = cart.Find(p => p.ProductId == productId);
                if (cartItem != null)
                {
                    cartItem.Quantity = quantity;
                    SaveCartCookie(cart);
                }
            }

            int cartCount = cart.Sum(x => x.Quantity);
            decimal cartTotal = cart.Sum(x => x.Price * x.Quantity);

            return Json(new { 
                success = true, 
                message = "Đã cập nhật giỏ hàng!",
                cartCount = cartCount,
                cartTotal = cartTotal,
                cartItems = cart 
            });
        }

        [Authorize]
        public IActionResult Checkout()
        {
            var cart = GetCartItems();
            if (cart.Count == 0)
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống.";
                return RedirectToAction("Index");
            }
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(string shippingAddress, string phoneNumber)
        {
            var cart = GetCartItems();
            if (cart.Count == 0)
            {
                return RedirectToAction("Index");
            }

            // Lấy thông tin user hiện tại
            var user = await _userManager.GetUserAsync(User);

            // Kiểm tra lại tồn kho một lần nữa trước khi tạo đơn
            foreach(var item in cart)
            {
                var product = _context.Products.Find(item.ProductId);
                if (product == null || product.Quantity < item.Quantity)
                {
                    TempData["Error"] = $"Sản phẩm {item.ProductName} hiện không đủ hàng (Kho: {product?.Quantity ?? 0}).";
                    return RedirectToAction("Index");
                }
            }

            // Tính tổng tiền
            decimal totalAmount = cart.Sum(i => i.Price * i.Quantity);

            // Tạo đơn hàng mới
            var order = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                ShippingAddress = shippingAddress,
                PhoneNumber = phoneNumber,
                Status = "Pending"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // Lưu để có OrderId

            // Tạo OrderDetail và trừ kho
            foreach (var item in cart)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                _context.OrderDetails.Add(orderDetail);

                // Trừ số lượng kho
                var product = _context.Products.Find(item.ProductId);
                if (product != null)
                {
                    product.Quantity -= item.Quantity;
                    _context.Products.Update(product);
                }
            }

            await _context.SaveChangesAsync();

            // Xóa giỏ hàng
            HttpContext.Response.Cookies.Delete("CartCookie");

            TempData["Success"] = "Đặt hàng thành công! Đơn hàng của bạn đang chờ duyệt.";
            return RedirectToAction("OrderSuccess", new { orderId = order.Id });
        }

        [Authorize]
        public IActionResult OrderSuccess(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }
    }
}
