using System.Collections.Generic;
using WebBanHang2.Models;

namespace WebBanHang2.Repositories
{
    public interface ICategoryRepository
    {
        // Lấy danh sách tất cả các danh mục sản phẩm
        IEnumerable<Category> GetAllCategories();
    }
}
