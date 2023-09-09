using Bulky.DataAccess.Reposotory.IReposotory;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        #region Constracture

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        #endregion

        #region Product List

        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            return View(objProductList);
        }

        #endregion Product List

        #region Upsert Product

        public IActionResult Upsert(int? id)
        {
            #region Send List for productVM

            productVM productVm = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().
                    Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),
                Product = new Product()
            };

            #endregion

            #region Insert View
            if (id == 0 || id == null)
            {
                return View(productVm);
            }
            #endregion

            else

            #region Update View
            {
                productVm.Product = _unitOfWork.Product.Get(c => c.Id == id);
                return View(productVm);
            }

            #endregion

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(productVM productVm, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                #region Insert && Update

                string wwwRoorPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRoorPath, @"images/Product/");
                    if (!string.IsNullOrEmpty(productVm.Product.ImageUrl))
                    {
                        //delete old image
                        var oldImage = Path.Combine(
                            wwwRoorPath, productVm.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }

                    using (var fileStrem = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStrem);
                    }

                    productVm.Product.ImageUrl = @"/images/Product/" + fileName;
                }

                if (productVm.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVm.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVm.Product);
                }
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");


                #endregion
            }
            else
            {
                #region return view after error
                TempData["error"] = "product created not succeed";
                productVm = new()
                {
                    CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),
                    Product = new Product()
                };
                return View(productVm);

                #endregion
            }
        }

        #endregion Upsert Product

        #region Delete Product

        public IActionResult Delete(int? id)
        {
            Product product = _unitOfWork.Product.Get(c => c.Id == id);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Remove(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product delete successfully";
                return RedirectToAction("Index");
            }
            TempData["success"] = "Product delete not succeed";
            return Redirect("Index");
        }
        #endregion


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }


        #endregion

    }
}
