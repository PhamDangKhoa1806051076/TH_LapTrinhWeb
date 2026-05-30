using System.Collections.Generic;
using PhamDangKhoa_W345_C2.Models;

namespace PhamDangKhoa_W345_C2.Repositories
{
    public interface ICategoryRepository
    {
        // Lấy danh sách tất cả các danh mục sản phẩm
        IEnumerable<Category> GetAllCategories();
    }
}
