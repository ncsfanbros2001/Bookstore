using Bookstore.Data.UnitOfWork;
using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _uow;

        public CoverTypeController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _uow.CoverType.GetAll();
            return View(objCoverTypeList);
        }

        public IActionResult Create() // GET
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType coverType) // POST
        {
            if (ModelState.IsValid)
            {
                _uow.CoverType.Add(coverType);
                _uow.Save();
                TempData["success"] = "Cover type created successfully";
                return RedirectToAction("Index");
            }
            return View(coverType);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var coverTypeFromDB = _uow.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (coverTypeFromDB == null)
            {
                return NotFound();
            }
            return View(coverTypeFromDB);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType coverType) // POST
        {
            if (ModelState.IsValid)
            {
                _uow.CoverType.Update(coverType);
                _uow.Save();
                TempData["success"] = "Cover type updated successfully";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id) // GET
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var coverTypeFromDB = _uow.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (coverTypeFromDB == null)
            {
                return NotFound();
            }
            return View(coverTypeFromDB);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id) // POST
        {
            var coverTypeFromDB = _uow.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (coverTypeFromDB == null)
            {
                return NotFound();
            }
            _uow.CoverType.Remove(coverTypeFromDB);
            _uow.Save();
            TempData["success"] = "Cover type deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
