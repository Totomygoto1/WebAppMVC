using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppMVC.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }

        public string JobTitle { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SocialSecurityNo { get; set; }
        //[Display(Name = "Hiring Date")]
        //[DataType(DataType.Date)]
        public DateTime HiringDate { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        //[Display(Name = "Hourly Salary")]
        //[Column(TypeName = "decimal(18, 2)")]
        public decimal HourlySalary { get; set; }
        //[Display(Name = "Monthly Salary")]
        //[Column(TypeName = "decimal(18, 2)")]
        public decimal MonthlySalary { get; set; }
        public string ContractFile { get; set; }

        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<Login> Login { get; set; }
    }
}