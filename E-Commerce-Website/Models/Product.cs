using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Website.Models
{
    public class Product
    {
        //[Key] Primary Key & Auto Increment
        [Key]
        public int product_id { get; set; }

        [Required]
        [StringLength(100)]
        public String product_name { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public Decimal product_price { get; set; }

        [Required]
        public String? product_Image { get; set; }

        [Range(0, 5)]
        public decimal? Rating { get; set; }

        public String? Thumbnail_Image { get; set; }

        [StringLength(500)]
        public String product_Description { get; set; }

        public int cat_id { get; set; }

        public Category Category { get; set; }

        // New Column
        public int stock_quantity { get; set; } // Stock availability

    }
}

