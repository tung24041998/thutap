using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trainee.Models;
using Trainee.Models.DAO;
using PagedList;

namespace Trainee.Controllers
{
    public class HomeController : Controller
    {
        db_Trainee db_Trainee = new db_Trainee();
        DAO DAO = new DAO();
        //public ActionResult Index(int page = 1, int pageSize = 6)
        //{
        //    var model = DAO.ListAllPaping(page, pageSize);
        //    return View(model);
        //}
        //[HttpPost
        public ActionResult Index(string cat, string q, int page = 1, int pageSize = 9)
        {
            ViewBag.Right = Session["Right"];
            ViewBag.Username = Session["Username"];
            ViewBag.cat = cat;
            ViewBag.q = q;

            var imagesQueryable = db_Trainee.Images.AsQueryable();
            if (!string.IsNullOrEmpty(cat)) imagesQueryable = imagesQueryable.Where(p => p.Theme.Contains(cat));
            if (!string.IsNullOrEmpty(q)) {
                var parts = q.Split(' ').Where(p => !string.IsNullOrWhiteSpace(p));
                imagesQueryable = imagesQueryable
                    .Where(x => parts.Any(p => x.Imagename.Contains(p)));

            }
            var m = imagesQueryable.OrderByDescending(x => x.Idimg).ToPagedList(page, pageSize);
            return View(m);

        }
        public ActionResult Header()
        {
            return PartialView(db_Trainee.ThemeImages);
        }
        public ActionResult AddTheme()
        {
            if (Session["Username"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }
        [HttpPost]
        public ActionResult AddTheme(ThemeImage ThemeImage)
        {
            if (Session["Username"] != null)
            {

                if (ThemeImage.Theme == null)
                {
                    ViewBag.ex = "Bạn chưa nhập chủ đề";
                    return View();
                }
                else
                {
                    var theme1 = db_Trainee.ThemeImages.FirstOrDefault(x => x.Theme == ThemeImage.Theme);
                    if (theme1 == null)
                    {
                        db_Trainee.ThemeImages.Add(ThemeImage);
                        db_Trainee.SaveChanges();

                        return RedirectToAction("Index");

                    }
                    else
                    {
                        ViewBag.ex = "Đã có chủ đề này !";
                        return View();
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }

        }
        public ActionResult uploadImage()
        {
            if (Session["Username"] != null)
            {
                return View(db_Trainee.ThemeImages);
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }
        [HttpPost]
        public ActionResult uploadImage(HttpPostedFileBase file, Image image)
        {
            if (Session["Username"] != null)
            {

                if (ModelState.IsValid)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string path = Path.Combine(Server.MapPath("~/Content/Image"),
                                                   Path.GetFileName(file.FileName));


                        //if (image.Theme == null || image.Link == null)
                        //{
                        //    ViewBag.ex = "Vui lòng nhập đầy đủ !";
                        //    return View();
                        //}
                        //else
                        //{
                        file.SaveAs(path);
                        image.Link = "/Content/Image/" + file.FileName;
                        image.Imagename = file.FileName;
                        image.censor = false;
                        ViewBag.name = Session["Username"];
                        image.FromImg = ViewBag.name;
                        db_Trainee.Images.Add(image);
                        db_Trainee.SaveChanges();
                        return RedirectToAction("Index");
                        //}
                    }
                    else
                    {
                        ViewBag.Message = "Bạn chưa chọn ảnh !";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "Vui lòng nhập đầy đủ !";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }

        }
        public ActionResult DeleteImage(int? id)
        {
            if (Session["Username"] != null)
            {
                var kt = db_Trainee.Images.FirstOrDefault(x => x.Idimg == id);
                if (kt != null)
                {
                    db_Trainee.Images.Remove(kt);
                    db_Trainee.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ex = "Xóa ảnh không thành công";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }

        }
        public ActionResult Theme()
        {
            if (Session["Username"] != null)
            {
                return View(db_Trainee.ThemeImages);
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }
        public ActionResult DeleteTheme(String theme)
        {
            if (Session["Username"] != null)
            {
                var themetest = db_Trainee.ThemeImages.FirstOrDefault(x => x.Theme == theme);
                if (themetest != null)
                {
                    db_Trainee.ThemeImages.Remove(themetest);
                    db_Trainee.SaveChanges();
                    return RedirectToAction("Theme");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }

        }
        public FileResult Download(String ImageName)
        {
            var FileVirtualPath = "/Content/Image/" + ImageName;
            return File(FileVirtualPath, "application/force- dowload", Path.GetFileName(FileVirtualPath));
        }
        public ActionResult Censor(int page = 1, int pageSize = 9)
        {
            return View(db_Trainee.Images.Where(x=>x.censor == false).OrderByDescending(x => x.Idimg).ToPagedList(page, pageSize));
        }
        [HttpPost]
        public ActionResult Censor(Image image, int page = 1, int pageSize = 9)
        {
            var ts = db_Trainee.Images.FirstOrDefault(x => x.Idimg == image.Idimg);
            ts.censor = true;
            db_Trainee.Entry(ts).State = System.Data.Entity.EntityState.Modified;

            db_Trainee.SaveChanges();
            return View(db_Trainee.Images.Where(x=>x.censor == false).OrderByDescending(x => x.Idimg).ToPagedList(page, pageSize));
        }
    }
}