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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public Login logEmp { get; set; }

        // GET: Employees
        public ActionResult LoginPage(string Email, string Password)
        {
            var log = db.Login.Include(a => a.Employee).Where(a => a.Email == Email).Where(a => a.Password == Password);

            /*
            logEmp = (Login)db.Login.Include(a => a.Employee).Where(a => a.Email == Email).Where(a => a.Password == Password);

            if (logEmp == null)
            {
                return View();
            }
            else if (logEmp.EmployeeID == 1)
            {
                return RedirectToAction("../Assignments/EmployeeIndex?id=" + logEmp.EmployeeID);
            }
            else
            {

            }
            */

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