using Bookstore.Data;
using Bookstore.Data.UnitOfWork;
using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _uow;

        public CategoryController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _uow.Category.GetAll();
            return View(objCategoryList);
        }

        public IActionResult Create() // GET
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category) // POST
        {
            if (ModelState.IsValid)
            {
                _uow.Category.Add(category);
                _uow.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Edit(int? id) // GET
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDB = _uow.Category.GetFirstOrDefault(u => u.Id == id);
            if (categoryFromDB == null)
            {
                return NotFound();
            }
            return View(categoryFromDB);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category) // POST
        {
            if (ModelState.IsValid)
            {
                _uow.Category.Update(category);
                _uow.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Delete(int? id) // GET
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDB = _uow.Category.GetFirstOrDefault(u => u.Id == id);
            if (categoryFromDB == null)
            {
                return NotFound();
            }
            return View(categoryFromDB);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id) // POST
        {
            var categoryFromDB = _uow.Category.GetFirstOrDefault(u => u.Id == id);
            if (categoryFromDB == null)
            {
                return NotFound();
            }
            _uow.Category.Remove(categoryFromDB);
            _uow.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
