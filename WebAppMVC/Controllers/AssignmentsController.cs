using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using WebAppMVC.Models;


namespace WebAppMVC.Controllers
{

    public class AssignmentsController : Controller
    {

        private DBContext db = new DBContext();

        public string email { get; set; }
        public string password { get; set; }
        public string workhours { get; set; }
        public string hourly { get; set; }
        public string total { get; set; }
        public string employeename { get; set; }
        public string timeperiod { get; set; }
        public string startmonth = "0";
        public string startyear = "0";
        public List<string> emptotalhours { get; set; }

        // GET: Assignments - Show All Assignments
        public ActionResult Index()
        {           
            var assignment = db.Assignment.Include(a => a.Customer).Include(a => a.Employee);
            return View(assignment.ToList());
        }

        
        // GET: Assignments - Show Assignment for specific Employee - Assignments?id=1 .. 
        public ActionResult EmployeeIndex(int? id)
        {
            var assignment = db.Assignment.Include(a => a.Customer).Include(a => a.Employee).Where(a => a.EmployeeID == id);
            return View(assignment.ToList());
        }

        public IList<Assignment> hours { get; set; }

        // GET: Assignments - Show All WorkHours per Month per Employee ..
        public ActionResult WorkHours()
        {
            // ## Antal arbetade timmar för alla anställda för en viss månad - Kanske "CARDS" som FrontEnd

            decimal TotalHours = 0;
            decimal TotalSalary = 0;
            decimal HourlyPay = 0;
            string Name = "";

            hours = db.Assignment.Include(a => a.Customer).Include(a => a.Employee).OrderBy(a => a.EmployeeID).ToList();

            int x = 0;
            foreach (var el in hours)
            {
                if (x == 0)
                {
                    Name = el.Employee.FirstName + " " + el.Employee.LastName;
                    x = 1;
                }
                if (Name == el.Employee.FirstName + " " + el.Employee.LastName)
                {   
                    // Same employee                  
                    TotalHours += el.HoursAmount;
                    
                }
                else
                {
                    // Write the first line out from Employee TotalHours

                    ViewBag.emptotalhours += "Name: " + Name + " Total workhours: " + TotalHours + " ";
                    ViewBag.emptotalhours += "-----------------------------------------------------------------";
                    ViewBag.emptotalhours += "-----------------------------------------------------------------";
                    ViewBag.emptotalhours += "-----------------------------------------------------";

                    TotalHours = 0;
                    TotalHours += el.HoursAmount;

                    // New employee
                    Name = el.Employee.FirstName + " " + el.Employee.LastName;
                }

            }
            ViewBag.emptotalhours += "Name: " + Name + " Total workhours: " + TotalHours;
            ViewBag.emptotalhours += "-----------------------------------------------------------------";
            ViewBag.emptotalhours += "-----------------------------------------------------------------";
            ViewBag.emptotalhours += "-----------------------------------------------------";

            return View(hours);
        }

        public IList<Assignment> salary { get; set; }

        
        // GET: Assignments - Show Salary Sum per Month for specific Employee - Assignments?id=1 .. 
        public ActionResult SalaryEmployee(int? id)
        {
            // List all hours - add and deduct special cost --
            // SP - add per row then sum it up - then taxes ..

            //var ID = Request.Cookies["sitecookies"]["Email"].ToString();
            //var id = Convert.ToInt32(ID);

            if (Request["Year"] != null)
            {
                startyear = Request["Year"];
            }
            else
            {
                startyear = "2019";
            }

            if (Request["Month"] != null)
            {
                startmonth = Request["Month"];
            }
            else
            {
                startmonth = "1";
            }

            int intyear = Convert.ToInt32(startyear);
            int intmonth = Convert.ToInt32(startmonth);
            int days = DateTime.DaysInMonth(intyear, intmonth);

            ViewBag.startyear = startyear;
            ViewBag.startmonth = startmonth;

            string d = startyear + "-" + startmonth + "-01 00:00:00";
            DateTime startdate = Convert.ToDateTime(d);

            string d2 = startyear + "-" + startmonth + "-" + days.ToString() + " 00:00:00";
            
            DateTime enddate = Convert.ToDateTime(d2);

            decimal TotalHours = 0;
            decimal TotalSalary = 0;
            decimal HourlyPay = 0;
            string Name = "";
            string Emailto = "";

            salary = db.Assignment.Include(a => a.Customer).Include(a => a.Employee).Where(a => a.EmployeeID == id).Where(a => a.Date >= startdate).Where(a => a.Date <= enddate).ToList();

            foreach (var el in salary)
            {
                ViewBag.workhours += el.HoursAmount + " * " + el.Employee.HourlySalary + " <br> ";

                TotalHours += el.HoursAmount;

                HourlyPay = el.Employee.HourlySalary;
                Name = el.Employee.FirstName + " " + el.Employee.LastName;
                Emailto = el.Employee.Email;

                if (el.OB1 != 0)
                {
                }
            }
            TotalSalary = TotalHours * HourlyPay;

            ViewBag.employeename = Name;
            ViewBag.timeperiod = "Timeperiod: " + startdate + " - " + enddate;
            ViewBag.hourly = "Hourly Salary: " + HourlyPay + " SEK";
            ViewBag.workhours = "Total Hours: " + TotalHours + " Hours";
            ViewBag.total = "Total Salary: " + TotalSalary + "  ";

            if (Request["SendEmail"] != null)
            {
                if (Request["SendEmail"] == "Yes")
                {
                    // Use my own email-address for Testing, not DB Email, read Code above => 
                    // Comment out after Testing, the Row below =>
                    Emailto = "receiver@gmail.com";

                    var body = "Email From: SwipeIT Inc. \n\n" +
                    "Message: Here is your Current Salary Receipt. \n\n" +
                    "Employeename: " + Name + "\n\n" + "Timeperiod: " + startdate + " - " + enddate + "\n" +
                    "Hourly Salary: " + HourlyPay + "\t" + "Total Hours: " + TotalHours + "\t" + "Total Salary: " + TotalSalary + "\n\n" +
                    "Regards \nSwipeIT Inc. ";
                
                    string To = Emailto;  
                    string From = "sendaddress@hotmail.com";  
                    string Subject = "Salary Receipt";
                    string Body = string.Format(body);

                    using (var smtp = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = "sendaddress@hotmail.com",  
                            Password = "password"  
                        };
                        
                        smtp.Credentials = credential;
                        smtp.Host = "smtp-mail.outlook.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        smtp.Send(From, To, Subject, Body);
                        
                    }
                }
            }

            return View(salary);
        }

        // GET: Assignments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignment.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // GET: Assignments/EmployeeDetails/5
        public ActionResult EmployeeDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignment.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // GET: Assignments/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customer, "CustomerID", "CompanyName");
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "JobTitle");
            return View();
        }

        // POST: Assignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssignmentID,EmployeeID,CustomerID,Description,Date,StartTime,EndTime,HoursAmount,OB1,OB2,Karens,SickLeave,VAB,Vacation,VacationCompensation,ExtraCost,GPSLocation")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                db.Assignment.Add(assignment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Customer, "CustomerID", "CompanyName", assignment.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "JobTitle", assignment.EmployeeID);
            return View(assignment);
        }

        // GET: Assignments/EmployeeCreate
        public ActionResult EmployeeCreate()
        {
            ViewBag.CustomerID = new SelectList(db.Customer, "CustomerID", "CompanyName");
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "JobTitle");
            return View();
        }

        // POST: Assignments/EmployeeCreate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeCreate([Bind(Include = "AssignmentID,EmployeeID,CustomerID,Description,Date,StartTime,EndTime,HoursAmount,OB1,OB2,Karens,SickLeave,VAB,Vacation,VacationCompensation,ExtraCost,GPSLocation")] Assignment assignment)
        {
            var emp_id = 0;
            if (Request.Cookies["sitecookies"] != null)
            {
                var ID = Request.Cookies["sitecookies"]["Email"].ToString();
                emp_id = Convert.ToInt32(ID);               
            }

            if (ModelState.IsValid)
            {
                db.Assignment.Add(assignment);
                db.SaveChanges();
                return RedirectToAction("EmployeeIndex/" + emp_id);
            }

            ViewBag.CustomerID = new SelectList(db.Customer, "CustomerID", "CompanyName", assignment.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "JobTitle", assignment.EmployeeID);
            return View(assignment);
        }

        // GET: Assignments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignment.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customer, "CustomerID", "CompanyName", assignment.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "JobTitle", assignment.EmployeeID);
            return View(assignment);
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssignmentID,EmployeeID,CustomerID,Description,Date,StartTime,EndTime,HoursAmount,OB1,OB2,Karens,SickLeave,VAB,Vacation,VacationCompensation,ExtraCost,GPSLocation")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customer, "CustomerID", "CompanyName", assignment.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "JobTitle", assignment.EmployeeID);
            return View(assignment);
        }

        // GET: Assignments/EmployeeEdit/5
        public ActionResult EmployeeEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignment.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customer, "CustomerID", "CompanyName", assignment.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "JobTitle", assignment.EmployeeID);
            return View(assignment);
        }

        // POST: Assignments/EmployeeEdit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeEdit([Bind(Include = "AssignmentID,EmployeeID,CustomerID,Description,Date,StartTime,EndTime,HoursAmount,OB1,OB2,Karens,SickLeave,VAB,Vacation,VacationCompensation,ExtraCost,GPSLocation")] Assignment assignment)
        {
            var emp_id = 0;
            if (Request.Cookies["sitecookies"] != null)
            {
                var ID = Request.Cookies["sitecookies"]["Email"].ToString();
                emp_id = Convert.ToInt32(ID);
            }

            if (ModelState.IsValid)
            {
                db.Entry(assignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EmployeeIndex/" + emp_id);
            }
            ViewBag.CustomerID = new SelectList(db.Customer, "CustomerID", "CompanyName", assignment.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "JobTitle", assignment.EmployeeID);
            return View(assignment);
        }

        // GET: Assignments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignment.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Assignment assignment = db.Assignment.Find(id);
            db.Assignment.Remove(assignment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Assignments/EmployeeDelete/5
        public ActionResult EmployeeDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignment.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // POST: Assignments/EmployeeDelete/5
        [HttpPost, ActionName("EmployeeDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeDeleteConfirmed(int id)
        {
            var emp_id = 0;
            if (Request.Cookies["sitecookies"] != null)
            {
                var ID = Request.Cookies["sitecookies"]["Email"].ToString();
                emp_id = Convert.ToInt32(ID);
            }

            Assignment assignment = db.Assignment.Find(id);
            db.Assignment.Remove(assignment);
            db.SaveChanges();
            return RedirectToAction("EmployeeIndex/" + emp_id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
 
    }
}
