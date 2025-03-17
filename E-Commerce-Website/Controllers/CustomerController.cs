using E_Commerce_Website.Models;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Website.Controllers
{
    public class CustomerController : Controller
    {
        private myContext _context;
        private IWebHostEnvironment _env;
        public CustomerController(myContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            var categories = _context.tbl_category.ToList();
            return View(categories);
        }
        public IActionResult ProductListPage(int pageNumber = 1, int pageSize = 12, int? categoryId = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            /*, string sortOrder = "Featured"*/
            // Retrieve categories and products from the database
            var categories = _context.tbl_category.ToList();

            // Build the query for retrieving products
            IQueryable<Product> productQuery = _context.tbl_product;

            // Apply category filter if a category is selected
            if (categoryId.HasValue)
            {
                productQuery = productQuery.Where(p => p.cat_id == categoryId.Value);
            }
            // Apply price range filter if minPrice and/or maxPrice are specified
            if (minPrice.HasValue)
            {
                productQuery = productQuery.Where(p => p.product_price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                productQuery = productQuery.Where(p => p.product_price <= maxPrice.Value);
            }
            // Apply sorting
            /*productQuery = sortOrder switch
            {
                "PriceLowToHigh" => productQuery.OrderBy(p => p.product_price),
                "PriceHighToLow" => productQuery.OrderByDescending(p => p.product_price),
                _ => productQuery // Default to "Featured" (no explicit sorting applied)
            };*/

            // Get the total number of products after filtering for pagination
            var totalProducts = productQuery.Count();

            // Retrieve products for the current page
            var products = productQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Create the view model with pagination and filter data
            var viewModel = new ProductListViewModel
            {
                Categories = categories,
                Products = products,
                CurrentPage = pageNumber,
                TotalPages = (int)System.Math.Ceiling((double)totalProducts / pageSize),
                /*SelectedSortOrder = sortOrder*/
            };

            return View(viewModel);
        }
        public IActionResult productDetailPage(int product_id) {

            // Fetch the product details
            var product = _context.tbl_product.FirstOrDefault(p => p.product_id == product_id);

            if (product == null)
            {
                return NotFound(); // 404 if product doesn't exist
            }

            // Check availability
            bool isAvailable = product.stock_quantity > 0;
            ViewBag.IsAvailable = isAvailable;

            return View(product);
        }
    }
}
