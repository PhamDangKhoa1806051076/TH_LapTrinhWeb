using System.Collections.Generic;
using System.Linq;
using PhamDangKhoa_W345_C2.Models;

namespace PhamDangKhoa_W345_C2.Repositories
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public EFCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }
    }
}
