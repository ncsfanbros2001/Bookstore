using Bookstore.Data.UnitOfWork;
using Bookstore.Models;
using Bookstore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookstore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork uow, IWebHostEnvironment webHostEnvironment)
        {
            _uow = uow;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id) // GET
        {
            ProductVM productVM = new()
            {
                product = new(),
                categoryList = _uow.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                coverTypeList = _uow.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null || id == 0) // Create product
            {
                return View(productVM);
            }
            else // Update product
            {
                productVM.product = _uow.Product.GetFirstOrDefault(u => u.Id == id);
                return View(productVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file) // POST
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images/products");
                    var extension = Path.GetExtension(file.FileName);

                    if (productVM.product.ImageURL != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.product.ImageURL.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension),
                        FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    productVM.product.ImageURL = @"/images/products/" + fileName + extension;
                }

                if (productVM.product.Id == 0)
                {
                    _uow.Product.Add(productVM.product);
                    TempData["success"] = "Product created successfully";
                }
                else
                {
                    _uow.Product.Update(productVM.product);
                    TempData["success"] = "Product updated successfully";
                }

                _uow.Save();
                return RedirectToAction("Index");
            }
            return View(productVM);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _uow.Product.GetAll(includeProperties: "Category,CoverType");
            return Json( new { data = productList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id) // POST
        {
            var productFromDB = _uow.Product.GetFirstOrDefault(u => u.Id == id);
            if (productFromDB == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productFromDB.ImageURL.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _uow.Product.Remove(productFromDB);
            _uow.Save();
            return Json(new { success = true, message = "Delete Successfully" });
        }

        #endregion

    }
}
