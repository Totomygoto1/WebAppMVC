using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppMVC.Models
{
    public class Login
    {
        public int LoginID { get; set; }
        public int EmployeeID { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public Employee Employee { get; set; }
    }
}