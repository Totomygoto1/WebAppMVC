using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using WebAppMVC.Models;

namespace WebAppMVC.Controllers
{
    public class HomeController : Controller
    {
        private DBContext db = new DBContext();

        public string email { get; set; }
        public string password { get; set; }

        public ActionResult Index()
        {
            ViewBag.Message = "Welcome! Please fill in your Login Details.";
            return View();
        }

        public IList<Login> user { get; set; }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Index([Bind(Include = "Email,Password")] Login login)
        {

            user = db.Login.Include(a => a.Employee).Where(a => a.Email == login.Email).Where(a => a.Password == login.Password).ToList();

            foreach (var el in user)
            {

                if (el.Employee.JobTitle == "Kontoret")
                {
                    MvcHelper.SetCookie("Kontoret", el.EmployeeID.ToString());

                    return RedirectToAction("../Assignments/Index");
                }
                else
                {
                    MvcHelper.SetCookie("Employee", el.EmployeeID.ToString());

                    return RedirectToAction("../Assignments/EmployeeIndex/" + el.EmployeeID);
                }
            }

            ViewBag.email = login.Email;
            ViewBag.password = login.Password + " Try again - Email or Password incorrect ";

            return View(login);

        }

        public ActionResult About()
        {
            ViewBag.Message = "Welcome to SwipeIT Inc.!";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Please Do stay in Touch!";

            return View();
        }

        public ActionResult ShowImages()
        {
            ViewBag.Message = "Show Images";

            return View();
        }

        public ActionResult UploadImage()
        {
            ViewBag.Message = "Upload an Image";

            return View();
        }

        public ActionResult UploadSmallimage()
        {
            ViewBag.Message = "Upload a small Image";

            return View();
        }

        public ActionResult UserData()
        {
            ViewBag.Message = "Save Data to a Textfile";

            return View();
        }

        public ActionResult DisplayData()
        {
            ViewBag.Message = "Read Data from a Textfile";

            return View();
        }

        public ActionResult FileUpload()
        {
            ViewBag.Message = "Read Data from a Textfile";

            return View();
        }

        public ActionResult FileDelete()
        {
            ViewBag.Message = "Delete a File";

            return View();
        }

        public ActionResult SilverlightVideo()
        {
            ViewBag.Message = "Show a SilverlightVideo";

            return View();
        }

        public ActionResult Chart()
        {
            ViewBag.Message = "Show a Chart";

            return View();
        }


    }
}