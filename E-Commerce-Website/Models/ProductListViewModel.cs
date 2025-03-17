
namespace E_Commerce_Website.Models
{
    public class ProductListViewModel
    {
        public List<Category>? Categories { get; set; }
        public List<Product>? Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
       /* public string SelectedSortOrder { get; set; }*/
    }
}