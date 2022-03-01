using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.Common.Tools;
using Project.ENTITIES.Models;
using Project.MVCUI.Areas.Admin.Data.AdminVMClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        ProductRepository _pRep;
        CategoryRepository _cRep;

        public ProductController()
        {
            _cRep = new CategoryRepository();
            _pRep = new ProductRepository();
        }

        // Asagıdaki Action'da parametre olarak istenen id aslında categoryID'sidir productID'si degildir...
        public ActionResult ProductList(int? id)
        {
            ProductVM pvm = new ProductVM
            {
                Products = id == null ? _pRep.GetActives() : _pRep.Where(x => x.CategoryID == id)
            };
            return View(pvm);
        }

        public ActionResult AddProduct()
        {
            ProductVM pvm = new ProductVM
            {
                Categories = _cRep.GetActives()
            };
            return View(pvm);
        }

        [HttpPost]
        public ActionResult AddProduct(Product product,HttpPostedFileBase resim)
        {
            product.ImagePath = ImageUploader.UploadImage("/Pictures/", resim);
            _pRep.Add(product);
            return RedirectToAction("ProductList");
        }

        public ActionResult UpdateProduct(int id)
        {
            ProductVM pvm = new ProductVM
            {
                Categories = _cRep.GetActives(),
                Product = _pRep.Find(id)
            };
            return View(pvm);
        }


        
        [HttpPost]
        public ActionResult UpdateProduct(Product product)
        {
            //resim = null(Özel bir algoritma yazmazsanız resim null gelecektir)
            _pRep.Update(product);
            return RedirectToAction("ProductList");
        }

        public ActionResult DeleteProduct(int id)
        {
            _pRep.Delete(_pRep.Find(id));
            return RedirectToAction("ProductList");
        }


    }
}