using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Website.Models
{
    public class Cart
    {
        //[Key] Primary Key & Auto Increment
        [Key]
        public int cart_id { get; set; }
        public int prod_id { get; set; }
        public int cust_id { get; set; }
        public int product_quantity { get; set; }
        public int cart_status { get; set; }
    }
}
