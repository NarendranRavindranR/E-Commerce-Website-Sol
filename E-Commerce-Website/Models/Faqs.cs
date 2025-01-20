using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Website.Models
{
    public class Faqs
    {
        //[Key] Primary Key & Auto Increment
        [Key]
        public int faq_id { get; set; }
        public String faq_question{ get; set; }
        public String faq_answer { get; set; }
    }
}
