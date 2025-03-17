using E_Commerce_Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;

namespace E_Commerce_Website.Controllers
{
    public class AdminController : Controller
    {
        private myContext _context;
        private IWebHostEnvironment _env;
        public AdminController(myContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]

        [HttpPost]
        public IActionResult Login(String adminEmail, String adminPassword)
        {
            var row = _context.tbl_admins.FirstOrDefault(a => a.admin_Email == adminEmail);
            if (row != null && row.admin_Password == adminPassword)
            {
                HttpContext.Session.SetString("Admin_Session", row.admin_Id.ToString());
                return RedirectToAction("index");
            }
            else
            {
                ViewBag.message = "Incorrect Username and Password";
            }
            //HttpContext.Session.SetString(SessionKeyName, "The Doctor");

            return View();
        }

        [HttpPost]
        public IActionResult Register(String adminFirstName, String adminLastName, String adminEmail, String adminPassword, String adminConfirmPassword)
        {
            if (adminPassword == adminConfirmPassword)
            {
                // Passwords match - process registration
                var admin = new Admin
                {
                    admin_Name = $"{adminFirstName} {adminLastName}", // Combine first and last names
                    admin_Email = adminEmail,
                    admin_Password = adminPassword
                };

                _context.tbl_admins.Add(admin); // Add the record to the database
                _context.SaveChanges(); // Save changes to the database

                ViewBag.message = "Correct Password! Admin account created successfully.";
            }

            else
            {
                // Passwords do not match - show error message
                ViewBag.message = "Incorrect Password! Please ensure both passwords match.";
            }

            return View();
        }
        public IActionResult Logout()
        {
            return RedirectToAction("login");
        }
        public IActionResult Profile()
        {
            var adminId = HttpContext.Session.GetString("Admin_Session");

            if (string.IsNullOrEmpty(adminId))
            {
                // If session is empty, redirect to login page
                return RedirectToAction("Login");
            }
            // Convert the string adminId to int
            var row = _context.tbl_admins.Where(a => a.admin_Id == int.Parse(adminId)).ToList();

            //If the Admin User is Logged in but suddenly if the manager decided to delete the User details from the Database, then again the user need to go back to Login Page.
            if (row == null)
            {
                // If admin not found, redirect to login page
                return RedirectToAction("Login");
            }
            return View(row);
        }
        [HttpPost]
        public IActionResult Profile(Admin admin)
        {
            _context.tbl_admins.Update(admin);
            _context.SaveChanges();
            return RedirectToAction("profile");
        }

        [HttpPost]
        public IActionResult ChangeProfileImage(IFormFile admin_image, Admin admin)
        {
            String ImagePath = Path.Combine(_env.WebRootPath, "admin_image", admin_image.FileName);
            FileStream fs = new FileStream(ImagePath, FileMode.Create);
            admin_image.CopyTo(fs);
            admin.admin_image = admin_image.FileName;
            _context.tbl_admins.Update(admin);
            _context.SaveChanges();
            return RedirectToAction("profile");
        }

        public IActionResult fetchCustomer()
        {
            var customer = _context.tbl_customer.ToList();
            return View(customer);
        }
        public IActionResult customerDetails(int id)
        {
            var result = _context.tbl_customer.Find(id);
            return View(result);
        }


        public IActionResult updateCustomer(int id)
        {
            var result = _context.tbl_customer.Find(id);
            return View(result);
        }
        [HttpPost]
        public IActionResult updateCustomer(Customer customer, IFormFile customer_image)
        {

            /*String ImagePath = Path.Combine(_env.WebRootPath, "customer_image", customer_image.FileName);
            FileStream fs = new FileStream(ImagePath, FileMode.Create);
            customer_image.CopyTo(fs);
            customer.customer_image = customer_image.FileName;
            _context.tbl_customer.Update(customer);
            return RedirectToAction("updateCustomer");*/
            if (customer_image != null)
            {
                String ImagePath = Path.Combine(_env.WebRootPath, "customer_image", customer_image.FileName);

                // Delete the file if it already exists
                if (System.IO.File.Exists(ImagePath))
                {
                    System.IO.File.Delete(ImagePath);
                }

                using (FileStream fs = new FileStream(ImagePath, FileMode.Create))
                {
                    customer_image.CopyTo(fs);
                }

                customer.customer_image = customer_image.FileName;
            }

            _context.tbl_customer.Update(customer);
            _context.SaveChanges();
            return RedirectToAction("fetchCustomer");
        }

        public IActionResult deletePermission(int id)
        {
            var result = _context.tbl_customer.FirstOrDefault(c => c.customer_id == id);
            return View(result);
        }
        public IActionResult deleteCustomer(int id)
        {
            // Find the customer record by ID
            var removeCustomer = _context.tbl_customer.Find(id);

            if (removeCustomer != null)
            {
                // Remove the customer from the database
                _context.tbl_customer.Remove(removeCustomer);
                _context.SaveChanges();
            }

            // Redirect to the customer listing page
            return RedirectToAction("fetchCustomer");
        }

        public IActionResult fetchCategory()
        {
            var category = _context.tbl_category.ToList();
            return View(category);
        }

        public IActionResult addCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult addCategory(Category cat, IFormFile Image)
        {
            if (Image != null)
            {
                string imagePath = Path.Combine(_env.WebRootPath, "CategoryUploads", Image.FileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }

                // Generate thumbnail
                string thumbnailPath = Path.Combine(_env.WebRootPath, "CategoryUploadsThumbnails", "thumb_" + Image.FileName);
                cat.Thumbnail_Image = GenerateThumbnail(imagePath, thumbnailPath);
                cat.Image = Image.FileName;
            }
            _context.tbl_category.Add(cat);
            _context.SaveChanges();
            return RedirectToAction("fetchCategory");
        }
        private string GenerateThumbnail(string imagePath, string thumbnailPath)
        {
            using (var image = Image.FromFile(imagePath))
            {
                // Set the desired thumbnail dimensions
                int width = 120;
                int height = 120;

                using (var thumbnail = new Bitmap(width, height))
                {
                    using (var graphics = Graphics.FromImage(thumbnail))
                    {
                        // Ensure high-quality rendering
                        graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                        // Draw the original image resized to thumbnail dimensions
                        graphics.DrawImage(image, 0, 0, width, height);
                    }

                    // Save the thumbnail to the specified path in PNG format
                    thumbnail.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Png);
                }
            }

            return Path.GetFileName(thumbnailPath); // Return the thumbnail file name
        }


        public IActionResult updateCategory(int id)
        {
            var result = _context.tbl_category.Find(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        [HttpPost]
        public IActionResult updateCategory(Category cat, IFormFile Image)
        {
            if (Image != null)
            {
                string imagePath = Path.Combine(_env.WebRootPath, "CategoryUploads", Image.FileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }

                // Generate thumbnail
                string thumbnailPath = Path.Combine(_env.WebRootPath, "CategoryUploadsThumbnails", "thumb_" + Image.FileName);
                cat.Thumbnail_Image = GenerateThumbnail(imagePath, thumbnailPath);
                cat.Image = Image.FileName;
            }
            _context.tbl_category.Update(cat);
            _context.SaveChanges();
            return RedirectToAction("fetchCategory");
        }
        public IActionResult deletePermissionCategory(int id)
        {
            var result = _context.tbl_category.FirstOrDefault(c => c.category_id == id);
            return View(result);
        }
        public IActionResult deleteCategory(int id)
        {
            var result = _context.tbl_category.Find(id);
            _context.tbl_category.Remove(result);
            _context.SaveChanges();
            return RedirectToAction("fetchCategory");
        }
        public IActionResult fetchProduct()
        {
            var product = _context.tbl_product.ToList();
            return View(product);
        }
        public IActionResult addProduct()
        {
            List<Category> categories = _context.tbl_category.ToList();
            ViewData["categories"] = categories;
            return View();
        }
        [HttpPost]
        public IActionResult addProduct(Product prod,IFormFile product_Image)
        {
            if (product_Image != null)
            {
                String imageName = Path.GetFileName(product_Image.FileName);
                String ImagePath = Path.Combine(_env.WebRootPath, "product_images", imageName);
                using (FileStream fs = new FileStream(ImagePath, FileMode.Create))
                {
                    product_Image.CopyTo(fs);
                }
                // Generate thumbnail
                string thumbnailPath = Path.Combine(_env.WebRootPath, "ThumbnailProduct_images", "thumb_" + product_Image.FileName);
                prod.Thumbnail_Image = GenerateThumbnail(ImagePath, thumbnailPath);
                prod.product_Image = product_Image.FileName;

            }

            _context.tbl_product.Add(prod);
            _context.SaveChanges();
            return RedirectToAction("fetchProduct");
        }
        public IActionResult updateProduct(int id)
        {
            List<Category> categories = _context.tbl_category.ToList();// Fetch the Category into List
            ViewData["categories"] = categories; //Store the categories in the Viewdata so that it will be in the HTML.Partial Page
            var product = _context.tbl_product.Find(id); // Fetch the product by ID
            ViewBag.SelectCategoryId = product.cat_id;

            return View(product); // Pass the product model to the view
        }
        [HttpPost]
        public IActionResult updateProduct(Product prod)
        {
            // Fetch the existing product from the database
            var existingProduct = _context.tbl_product.Find(prod.product_id);

            if (existingProduct != null)
            {
                // Retain the existing Thumbnail_Image if not explicitly modified
                if (string.IsNullOrEmpty(prod.Thumbnail_Image))
                {
                    prod.Thumbnail_Image = existingProduct.Thumbnail_Image;
                }
                /* _context.tbl_product.Update(prod);
                 _context.SaveChanges();*/

                // Update only the necessary fields
                existingProduct.product_name = prod.product_name;
                existingProduct.Rating = prod.Rating;
                existingProduct.product_price = prod.product_price;
                existingProduct.product_Description = prod.product_Description;
                existingProduct.cat_id = prod.cat_id;
                existingProduct.Thumbnail_Image = prod.Thumbnail_Image;
                existingProduct.stock_quantity = prod.stock_quantity;

                // Save changes
                _context.tbl_product.Update(existingProduct);
                _context.SaveChanges();
            }
            return RedirectToAction("fetchProduct");
        }

        [HttpPost]
        public IActionResult ChangeProductImage(IFormFile product_Image, Product product)
        {
            /*            String ImagePath = Path.Combine(_env.WebRootPath, "product_images", product_Image.FileName);
                        FileStream fs = new FileStream(ImagePath, FileMode.Create);
                        product_Image.CopyTo(fs);
                        product.product_Image = product_Image.FileName;*/
            if (product_Image != null)
            {
                String imageName = Path.GetFileName(product_Image.FileName);
                String ImagePath = Path.Combine(_env.WebRootPath, "product_images", imageName);
                using (FileStream fs = new FileStream(ImagePath, FileMode.Create))
                {
                    product_Image.CopyTo(fs);
                }
                // Generate thumbnail
                string thumbnailPath = Path.Combine(_env.WebRootPath, "ThumbnailProduct_images", "thumb_" + product_Image.FileName);
                product.Thumbnail_Image = GenerateThumbnail(ImagePath, thumbnailPath);
                product.product_Image = product_Image.FileName;

            }
            _context.tbl_product.Update(product);
            _context.SaveChanges();
            return RedirectToAction("fetchProduct");
        }

        public IActionResult deletePermissionProduct(int id)
        {
            var result = _context.tbl_product.FirstOrDefault(c => c.product_id == id);
            return View(result);
        }
        public IActionResult deleteProduct(int id)
        {
            var result = _context.tbl_product.Find(id);
            _context.tbl_product.Remove(result);
            _context.SaveChanges();
            return RedirectToAction("fetchProduct");
        }
    }
}
