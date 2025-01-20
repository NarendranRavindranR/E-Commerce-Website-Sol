using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Website.Models
{
    public class Admin
    {
        //[Key] Primary Key & Auto Increment
        [Key]
        public int admin_Id { get; set; }
        public String admin_Name { get; set; }
        public String admin_Email { get; set; }
        public String admin_Password { get; set; }
        public String admin_image {get ; set;}
    }
}
