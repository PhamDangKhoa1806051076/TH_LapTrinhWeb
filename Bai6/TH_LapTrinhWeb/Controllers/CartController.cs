using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PhamDangKhoa_W345_C2.Extensions;
using PhamDangKhoa_W345_C2.Models;
using PhamDangKhoa_W345_C2.Repositories;
using System.Text.Json;

namespace PhamDangKhoa_W345_C2.Controllers
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

        // Khóa Session riêng theo từng user (tránh lẫn giỏ hàng giữa các tài khoản)
        private string? GetSessionKey()
        {
            var userId = _userManager.GetUserId(User);
            return string.IsNullOrEmpty(userId) ? null : $"Cart_{userId}";
        }

        // Lấy giỏ hàng từ Session (chỉ khi đã đăng nhập)
        private List<CartItem> GetCartItems()
        {
            var sessionKey = GetSessionKey();
            if (sessionKey == null) return new List<CartItem>(); // Chưa đăng nhập → giỏ hàng trống

            var cartJson = HttpContext.Session.GetString(sessionKey);
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItem>();
            }
            try
            {
                var cart = JsonSerializer.Deserialize<List<CartItem>>(cartJson);
                return cart ?? new List<CartItem>();
            }
            catch
            {
                return new List<CartItem>();
            }
        }

        // Lưu giỏ hàng vào Session (chỉ khi đã đăng nhập)
        private void SaveCartItems(List<CartItem> cart)
        {
            var sessionKey = GetSessionKey();
            if (sessionKey == null) return; // Chưa đăng nhập → không lưu

            HttpContext.Session.SetString(sessionKey, JsonSerializer.Serialize(cart));
        }

        [Authorize]
        public IActionResult Index()
        {
            var cart = GetCartItems();
            return View(cart);
        }

        [HttpPost]
        [Authorize]
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

            SaveCartItems(cart);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddToCartApi(int productId, int quantity = 1)
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng." });
            }

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

            SaveCartItems(cart);
            
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

        [Authorize]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCartItems();
            var cartItem = cart.Find(p => p.ProductId == productId);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
                SaveCartItems(cart);
                TempData["Success"] = "Đã xóa sản phẩm khỏi giỏ hàng.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
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
                SaveCartItems(cart);
                TempData["Success"] = "Đã cập nhật số lượng.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantityApi(int productId, int quantity)
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập để cập nhật giỏ hàng." });
            }

            var cart = GetCartItems();
            if (quantity <= 0)
            {
                var itemToRemove = cart.Find(p => p.ProductId == productId);
                if (itemToRemove != null)
                {
                    cart.Remove(itemToRemove);
                    SaveCartItems(cart);
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
                    SaveCartItems(cart);
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
            if (user == null) return RedirectToAction("Login", "Account");

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

            // Xóa giỏ hàng của user trong Session sau khi đặt hàng thành công
            var sessionKey = GetSessionKey();
            if (sessionKey != null)
                HttpContext.Session.Remove(sessionKey);

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
