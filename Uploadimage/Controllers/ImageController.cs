using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uploadimage.Models;

namespace Uploadimage.Controllers
{
    public class ImageController : Controller
    {
        private DbModels db = new DbModels();

        public ActionResult Index()
        {
            return View(db.Images.ToList());
        }
             
        // GET: Image
        public ActionResult Add()
        {
            var list = db.Products.ToList();
            ViewBag.ProducId = new SelectList(list, "ProducId", "Nombre");
            return View();
        }

        [HttpPost]
        public ActionResult Add(Image imageModel)
        {
            string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.ImagePath = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            imageModel.ImageFile.SaveAs(fileName);
            using (DbModels db = new DbModels())
            {
                db.Images.Add(imageModel);
                db.SaveChanges();
            }
            ModelState.Clear();
            return RedirectToAction("Index", "Image"); 
        }

        public ActionResult View(int id)
        {
            Image miImage = new Image();
            miImage = db.Images.Find(id);
            return View(miImage);
        }
    }
}