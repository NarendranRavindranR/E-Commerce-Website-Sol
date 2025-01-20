
using System.ComponentModel.DataAnnotations;
namespace E_Commerce_Website.Models;

public class Customer
{
    //[Key] Primary Key & Auto Increment
    [Key]
    public int customer_id { get; set; }
    public String customer_name { get; set; }
    public String customer_phone { get; set; }
    public String customer_email { get; set; }
    public String customer_password { get; set; }
    public String customer_gender { get; set; }
    public String customer_country { get; set; }
    public String customer_city { get; set; }
    public String customer_address { get; set; }
    public String customer_image { get; set; }
}
