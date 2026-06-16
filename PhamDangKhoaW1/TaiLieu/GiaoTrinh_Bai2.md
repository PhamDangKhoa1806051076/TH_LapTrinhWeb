# TÀI LIỆU GIÁO TRÌNH - BÀI 2: XÂY DỰNG ỨNG DỤNG WEBSITE CƠ BẢN VỚI ASP.NET CORE MVC

Tài liệu này tổng hợp toàn bộ nội dung, hình ảnh và mã nguồn chi tiết từ các trang giáo trình **Trang 17 đến Trang 40** do người dùng cung cấp.

---

## 📌 PHẦN 1: MỤC TIÊU BÀI HỌC (Trang 17)
Sau khi hoàn thành Bài 2, sinh viên cần đạt được:
- Hiểu rõ cấu trúc nội bộ của ứng dụng ASP.NET Core MVC (sự phân chia và tương tác giữa các lớp và modules).
- Hiểu cơ chế truyền tải và nhận dữ liệu trong mô hình MVC (binding và routing).
- Nắm vững khái niệm và quản lý 'view', sử dụng Razor syntax.
- Hiểu sâu về cấu trúc và chức năng của 'model' trong MVC (cách thức tương tác và xử lý dữ liệu).
- Hiểu rõ vai trò của 'controller' (quản lý luồng dữ liệu, xử lý yêu cầu và phản hồi).
- Hiểu rõ cách truyền 'model' giữa các thành phần.
- Thực hành kỹ thuật xử lý và lưu trữ hình ảnh.

---

## 💻 PHẦN 2: KHỞI TẠO VÀ CẤU TRÚC DỰ ÁN (Trang 18 - 21)

### 2.1 Khởi tạo dự án bằng Visual Studio 2022
- **Loại Project:** `ASP.NET Core Web App (Model-View-Controller)`
- **Tên Project:** `WebsiteBanHang`
- **Framework:** `.NET 6.0 (Long Term Support)`
- **Authentication Type:** `None`
- **Configure for HTTPS:** `Checked`
- **Docker Support:** `Linux`

### 2.2 Cấu trúc thư mục dự án ASP.NET Core MVC (Trang 20 - 21)
1. **Dependencies:** Gói phụ thuộc cần thiết để chạy ứng dụng.
2. **Properties:** Chứa tệp `launchSettings.json` cấu hình môi trường phát triển.
3. **wwwroot:** Thư mục gốc chứa tệp tĩnh (`.css`, `.js`, bootstrap, v.v.).
4. **Controllers:** Chứa các lớp điều khiển xử lý nghiệp vụ.
5. **Models:** Chứa các lớp Model để thao tác dữ liệu.
6. **Views:** Chứa các tệp giao diện Razor `.cshtml` (HTML động).
7. **Shared:** Chứa bố cục chung `_Layout.cshtml`.
8. **appsettings.json:** Cấu hình chung hệ thống (chuỗi kết nối, biến toàn cục).
9. **Program.cs:** Thiết lập web server, đăng ký dịch vụ (DI) và Middleware.

---

## 📐 PHẦN 3: KIẾN TRÚC LAYOUT VÀ RAZOR ENGINE (Trang 21 - 23)

### 3.1 Cấu trúc file `_Layout.cshtml`
Mô hình Layout mặc định gồm các phần:
- **HEADER** (Đầu trang)
- **NAVIGATION** (Thanh thực đơn điều hướng)
- **BODY** (Nội dung động - chia cột: Trái, Giữa, Phải nếu có)
- **FOOTER** (Chân trang)

### 3.2 Cơ chế biên dịch và Render
- **Razor View Engine:** `HTML + Code C#` -> qua `View Engine` biên dịch -> tạo ra `Pure HTML` gửi về trình duyệt của khách hàng.
- **RenderBody():** Nạp nội dung động của từng View cụ thể vào vùng trung tâm của Layout.
- **RenderSectionAsync:** Nạp các Script hoặc Style riêng biệt của từng View cụ thể.
  * *Khai báo tại _Layout:* `@await RenderSectionAsync("Scripts", required: false)`
  * *Khai báo tại View:* `@section Scripts { ... }`

---

## 🛠️ PHẦN 4: YÊU CẦU BÀI THỰC HÀNH CRUD (Trang 23 - 26)

### 4.1 Mục đích
Xây dựng ứng dụng Web bán hàng công nghệ hỗ trợ đầy đủ các thao tác Thêm, Đọc, Xóa, Sửa (CRUD) dựa trên mô hình ASP.NET Core MVC, sử dụng **Repository Pattern** kết hợp với **Mock Data** trong bộ nhớ.

### 4.2 Đặc tả Models (Trang 25)

#### Lớp `Product` (`Models/Product.cs`)
```csharp
using System.ComponentModel.DataAnnotations;

public class Product
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 10000.00)]
    public decimal Price { get; set; }

    public string Description { get; set; } = string.Empty;

    public int CategoryId { get; set; }
}
```

#### Lớp `Category` (`Models/Category.cs`)
```csharp
using System.ComponentModel.DataAnnotations;

public class Category
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string Name { get; set; } = string.Empty;
}
```

---

## 💾 PHẦN 5: CHI TIẾT REPOSITORIES (Trang 26 - 27)

### 5.1 Interface `IProductRepository.cs`
```csharp
using System.Collections.Generic;
using Bai2.Models;

public interface IProductRepository
{
    IEnumerable<Product> GetAll();
    Product GetById(int id);
    void Add(Product product);
    void Update(Product product);
    void Delete(int id);
}
```

### 5.2 Lớp `MockProductRepository.cs` (Trang 26-27)
```csharp
using System.Collections.Generic;
using System.Linq;
using Bai2.Models;

public class MockProductRepository : IProductRepository
{
    private readonly List<Product> _products;
    
    public MockProductRepository()
    {
        _products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 1000, Description = "A high-end laptop", CategoryId = 1 },
            new Product { Id = 2, Name = "Desktop", Price = 800, Description = "A powerful desktop", CategoryId = 2 }
        };
    }
    
    public IEnumerable<Product> GetAll()
    {
        return _products;
    }
    
    public Product GetById(int id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }
    
    public void Add(Product product)
    {
        product.Id = _products.Max(p => p.Id) + 1;
        _products.Add(product);
    }
    
    public void Update(Product product)
    {
        var index = _products.FindIndex(p => p.Id == product.Id);
        if (index != -1)
        {
            _products[index] = product;
        }
    }
    
    public void Delete(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product != null)
        {
            _products.Remove(product);
        }
    }
}
```

### 5.3 Interface `ICategoryRepository.cs` (Trang 27)
```csharp
using System.Collections.Generic;
using Bai2.Models;

public interface ICategoryRepository
{
    IEnumerable<Category> GetAllCategories();
}
```

### 5.4 Lớp `MockCategoryRepository.cs` (Trang 27)
```csharp
using System.Collections.Generic;
using Bai2.Models;

public class MockCategoryRepository : ICategoryRepository
{
    private List<Category> _categoryList;
    
    public MockCategoryRepository()
    {
        _categoryList = new List<Category>
        {
            new Category { Id = 1, Name = "Laptop" },
            new Category { Id = 2, Name = "Desktop" }
        };
    }
    
    public IEnumerable<Category> GetAllCategories()
    {
        return _categoryList;
    }
}
```

---

## 🎛️ PHẦN 6: CHI TIẾT CONTROLLERS & PROGRAM.CS (Trang 28 - 30)

### 6.1 Lớp `ProductController.cs`
```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bai2.Models;
using Bai2.Repositories;

public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    
    public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }
    
    // 1. Hiển thị danh sách sản phẩm
    public IActionResult Index()
    {
        var products = _productRepository.GetAll();
        return View(products);
    }
    
    // 2. Hiển thị thông tin chi tiết một sản phẩm
    public IActionResult Display(int id)
    {
        var product = _productRepository.GetById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }
    
    // 3. Hiển thị form thêm mới sản phẩm (GET)
    public IActionResult Add()
    {
        var categories = _categoryRepository.GetAllCategories();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        return View();
    }
    
    // 4. Xử lý lưu sản phẩm mới (POST)
    [HttpPost]
    public IActionResult Add(Product product)
    {
        if (ModelState.IsValid)
        {
            _productRepository.Add(product);
            return RedirectToAction("Index");
        }
        return View(product);
    }
    
    // 5. Hiển thị form cập nhật thông tin sản phẩm (GET)
    public IActionResult Update(int id)
    {
        var product = _productRepository.GetById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }
    
    // 6. Xử lý cập nhật thông tin sản phẩm (POST)
    [HttpPost]
    public IActionResult Update(Product product)
    {
        if (ModelState.IsValid)
        {
            _productRepository.Update(product);
            return RedirectToAction("Index");
        }
        return View(product);
    }
    
    // 7. Hiển thị form xác nhận xóa sản phẩm (GET)
    public IActionResult Delete(int id)
    {
        var product = _productRepository.GetById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }
    
    // 8. Xử lý xóa sản phẩm (POST)
    [HttpPost, ActionName("DeleteConfirmed")]
    public IActionResult DeleteConfirmed(int id)
    {
        _productRepository.Delete(id);
        return RedirectToAction("Index");
    }
}
```

### 6.2 Cấu hình dịch vụ trong `Program.cs` (Trang 30)
```csharp
var builder = WebApplication.CreateBuilder(args);

// Đăng ký dịch vụ Controllers with Views
builder.Services.AddControllersWithViews();

// Đăng ký Dependency Injection cho Repositories
builder.Services.AddSingleton<IProductRepository, MockProductRepository>();
builder.Services.AddScoped<ICategoryRepository, MockCategoryRepository>();

var app = builder.Build();

// Cấu hình Middleware Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Định nghĩa Route mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

---

## 🎴 PHẦN 7: CHI TIẾT MÃ NGUỒN RAZOR VIEWS (Trang 31 - 34)

### 7.1 Cấu trúc thư mục Views
```text
Views/
  ├── Home/
  │     ├── Index.cshtml
  │     └── Privacy.cshtml
  ├── Product/
  │     ├── Add.cshtml
  │     ├── Delete.cshtml
  │     ├── Display.cshtml
  │     ├── Index.cshtml
  │     └── Update.cshtml
  └── Shared/
        ├── _Layout.cshtml
        └── _ViewStart.cshtml
```

### 7.2 Trang `Add.cshtml` (Thêm sản phẩm)
```html
@model YourNamespace.Models.Product
@using Microsoft.AspNetCore.Mvc.Rendering
@{
    ViewData["Title"] = "Add Product";
}
<h1>Add Product</h1>
<form asp-action="Add">
    <div asp-validation-summary="All" class="text-danger"></div>
    <div class="form-group">
        <label asp-for="Name"></label>
        <input asp-for="Name" class="form-control" />
        <span asp-validation-for="Name" class="text-danger"></span>
    </div>
    <div class="form-group">
        <label asp-for="Price"></label>
        <input asp-for="Price" class="form-control" />
        <span asp-validation-for="Price" class="text-danger"></span>
    </div>
    <div class="form-group">
        <label asp-for="Description"></label>
        <textarea asp-for="Description" class="form-control"></textarea>
        <span asp-validation-for="Description" class="text-danger"></span>
    </div>
    <div class="form-group">
        <label asp-for="CategoryId">Category</label>
        <select asp-for="CategoryId" asp-items="ViewBag.Categories" class="form-control"></select>
    </div>
    <button type="submit" class="btn btn-primary">Add</button>
</form>
```

### 7.3 Trang `Index.cshtml` (Danh sách sản phẩm)
```html
@model IEnumerable<YourNamespace.Models.Product>
<h2>Products</h2>
<table class="table">
    <thead>
        <tr>
            <th>Name</th>
            <th>Price</th>
            <th>Description</th>
            <th>Actions</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var product in Model)
        {
            <tr>
                <td>@product.Name</td>
                <td>@product.Price</td>
                <td>@product.Description</td>
                <td>
                    <a asp-action="Display" asp-route-id="@product.Id">View</a> |
                    <a asp-action="Update" asp-route-id="@product.Id">Edit</a> |
                    <a asp-action="Delete" asp-route-id="@product.Id">Delete</a>
                </td>
            </tr>
        }
    </tbody>
</table>
```

### 7.4 Trang `Display.cshtml` (Chi tiết sản phẩm)
```html
@model YourNamespace.Models.Product
<h2>Product Details</h2>
<div>
    <h4>Name: @Model.Name</h4>
    <h4>Price: @Model.Price</h4>
    <h4>Description: @Model.Description</h4>
</div>
<a asp-action="Index">Back to List</a>
```

### 7.5 Trang `Delete.cshtml` (Xác nhận xóa sản phẩm)
```html
@model YourNamespace.Models.Product
<h2>Are you sure you want to delete this?</h2>
<div>
    <h4>Name: @Model.Name</h4>
    <h4>Price: @Model.Price</h4>
    <h4>Description: @Model.Description</h4>
</div>
<form asp-action="DeleteConfirmed" method="post">
    <input type="hidden" asp-for="Id" />
    <input type="submit" value="Delete" class="btn btn-danger" /> |
    <a asp-action="Index">Cancel</a>
</form>
```

### 7.6 Trang `Update.cshtml` (Cập nhật sản phẩm)
```html
@model YourNamespace.Models.Product
<h2>Edit Product</h2>
<form asp-action="Update">
    <input type="hidden" asp-for="Id" />
    <div class="form-group">
        <label asp-for="Name"></label>
        <input asp-for="Name" class="form-control" />
        <span asp-validation-for="Name" class="text-danger"></span>
    </div>
    <div class="form-group">
        <label asp-for="Price"></label>
        <input asp-for="Price" class="form-control" />
        <span asp-validation-for="Price" class="text-danger"></span>
    </div>
    <div class="form-group">
        <label asp-for="Description"></label>
        <textarea asp-for="Description" class="form-control"></textarea>
        <span asp-validation-for="Description" class="text-danger"></span>
    </div>
    <button type="submit" class="btn btn-primary">Update</button>
</form>
```

---

## 📸 PHẦN 8: KẾT QUẢ CHẠY & TÍNH NĂNG UPLOAD ẢNH NÂNG CAO (Trang 35 - 40)

### 8.1 Kết quả chạy thực tế ban đầu (Trang 35)
Giao diện nạp thành công các giá trị sản phẩm ví dụ: `Iphone 15` (giá `35,000,000.00`) và `Iphone 16` (giá `50,000,000.00`) thuộc danh mục `1`.

### 8.2 Bổ sung tính năng upload file hình ảnh (Trang 36-38)

#### Mở rộng Model `Product` (Trang 36)
```csharp
public class Product
{
    // ...Các thuộc tính có sẵn
    
    public string? ImageUrl { get; set; } // Đường dẫn đến hình ảnh đại diện chính
    public List<string>? ImageUrls { get; set; } // Danh sách các hình ảnh phụ khác
}
```

#### Tạo thư mục `images` trong `wwwroot` (Trang 37)
Tạo thư mục `wwwroot/images` để lưu trữ các file ảnh được tải lên.

#### Cập nhật View `Add.cshtml` cho tính năng Upload (Trang 37)
- Thêm thuộc tính `enctype="multipart/form-data"` vào thẻ form:
  ```html
  <form asp-action="Add" enctype="multipart/form-data">
  ```
- Thêm trường chọn file cho ảnh đại diện (`ImageUrl`) và ảnh phụ (`ImageUrls`):
  ```html
  <div class="form-group">
      <label asp-for="ImageUrl">Product Image</label>
      <input type="file" name="ImageUrl" class="form-control" />
  </div>
  <div class="form-group">
      <label asp-for="ImageUrls">Additional Images</label>
      <input type="file" name="ImageUrls" class="form-control" multiple />
  </div>
  ```

#### Xử lý Upload trong `ProductController` (Trang 38)
```csharp
[HttpPost]
public async Task<IActionResult> Add(Product product, IFormFile imageUrl, List<IFormFile> imageUrls)
{
    if (ModelState.IsValid)
    {
        if (imageUrl != null)
        {
            // Lưu hình ảnh đại diện
            product.ImageUrl = await SaveImage(imageUrl);
        }
        
        if (imageUrls != null && imageUrls.Count > 0)
        {
            product.ImageUrls = new List<string>();
            foreach (var file in imageUrls)
            {
                // Lưu các hình ảnh phụ
                product.ImageUrls.Add(await SaveImage(file));
            }
        }
        
        _productRepository.Add(product);
        return RedirectToAction("Index");
    }
    return View(product);
}

private async Task<string> SaveImage(IFormFile image)
{
    // Đường dẫn lưu file tùy chọn trong cấu hình
    var savePath = Path.Combine("wwwroot/images", image.FileName);
    using (var fileStream = new FileStream(savePath, FileMode.Create))
    {
        await image.CopyToAsync(fileStream);
    }
    return "/images/" + image.FileName; // Trả về đường dẫn tương đối
}
```

#### Cấu hình `Program.cs` phục vụ file tĩnh
Đảm bảo gọi `app.UseStaticFiles()` để trình duyệt có thể truy cập file trong thư mục `wwwroot`.

### 8.3 Hiển thị hình ảnh trong các View (Trang 40)
Ví dụ trong View `Display.cshtml`:
```html
@model YourNamespace.Models.Product
<h2>Product Details</h2>
<div>
    <h4>Name: @Model.Name</h4>
    <h4>Price: @Model.Price</h4>
    <h4>Description: @Model.Description</h4>
    
    @if (!string.IsNullOrEmpty(Model.ImageUrl))
    {
        <img src="@Model.ImageUrl" alt="Product Image" style="width: 300px; height: auto;" />
    }
    
    @if (Model.ImageUrls != null)
    {
        foreach (var url in Model.ImageUrls)
        {
            <img src="@url" alt="Product Image" style="width: 300px; height: auto;" />
        }
    }
</div>
```

---

## ⚠️ LƯU Ý QUAN TRỌNG VỀ XỬ LÝ LỖI (Trang 40)
Sinh viên cần bổ sung kiểm tra và xử lý lỗi cho các tình huống phát sinh thực tế:
1. **Định dạng file không hợp lệ:** Người dùng chọn tải lên file không phải tệp ảnh (ví dụ tệp tin .exe, .pdf, v.v.).
2. **Dung lượng tệp tin quá lớn:** Tải lên tệp hình ảnh có kích thước vượt quá giới hạn hệ thống (ví dụ lớn hơn 2MB), có thể gây nghẽn băng thông và lỗi bộ nhớ đệm server.
