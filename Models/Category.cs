using System.ComponentModel.DataAnnotations;

namespace PhamDangKhoa_W345_C2.Models
{
    public class Category
    {
        
        public int Id { get; set; }
        
        
        [Required, StringLength(50)]
        public string Name { get; set; }
    }
}
