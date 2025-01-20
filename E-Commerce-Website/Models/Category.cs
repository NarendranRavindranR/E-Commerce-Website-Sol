using System.ComponentModel.DataAnnotations;
namespace E_Commerce_Website.Models
{
    public class Category
    {
        //[Key] Primary Key & Auto Increment
        [Key]
        public int category_id { get; set; }
        public String category_name { get; set; }
        public string Image { get; set; } // Field for the original image
        public string Thumbnail_Image { get; set; } // Field for the thumbnail image
        public List<Product> Product { get; set; }
    }
}
