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
        public ActionResult Index(String name, int page = 1, int pageSize = 9)
        {
            ViewBag.Right = Session["Right"];
            ViewBag.Username = Session["Username"];
            
            if (name == null)
            {               
                var model = DAO.ListAllPaping(page, pageSize);
                return View(model);
            }
            else
            {
                //var nu = db_Trainee.Images.FirstOrDefault(x=>x.Theme == name);
                //if(nu == null)
                //{
                //    ViewBag.nu = "Không có ảnh nào !";
                //    return View(db_Trainee.Images.Where(x => x.Theme.Contains(name)).OrderByDescending(x => x.Idimg).ToPagedList(page, pageSize)); ;
                //}
                //else
                //{
                    return View(db_Trainee.Images.Where(x => x.Theme.Contains(name)).OrderByDescending(x => x.Idimg).ToPagedList(page, pageSize));
                //}
                
            }
        }
        public ActionResult Header()
        {
            return PartialView(db_Trainee.ThemeImages);
        }
        public ActionResult AddTheme()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddTheme(ThemeImage ThemeImage)
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
        public ActionResult uploadImage()
        {
            return View(db_Trainee.ThemeImages);
        }
        [HttpPost]
        public ActionResult uploadImage(HttpPostedFileBase file, Image image)
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
        public ActionResult DeleteImage(int? id)
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

    }
}