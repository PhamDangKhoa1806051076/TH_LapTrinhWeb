using System.Collections.Generic;
using PhamDangKhoa_W345_C2.Models;

namespace PhamDangKhoa_W345_C2.Repositories
{
    public interface IProductRepository
    {
    
        IEnumerable<Product> GetAll();
       
        Product GetById(int id);
     
        void Add(Product product);
      
        void Update(Product product);
        
        void Delete(int id);
    }
}
