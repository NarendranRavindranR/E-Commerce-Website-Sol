using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Website.Models
{
    public class Product
    {   
        //[Key] Primary Key & Auto Increment
        [Key]
        public int product_id { get; set; }
        public String product_name { get; set; }
        public String product_price { get; set; }
        public String product_Image { get; set; }
        public String product_Description { get; set; }
        public int cat_id { get; set; }
        public Category Category { get; set; }
      
    }
}
