using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppMVC.Models
{
    public class Assignment
    {
        public int AssignmentID { get; set; }
        public int EmployeeID { get; set; }
        public int CustomerID { get; set; }

        public string Description { get; set; }
        //[DataType(DataType.Date)]
        public DateTime Date { get; set; }
        //[Column(TypeName = "decimal(4, 2)")]
        public decimal StartTime { get; set; }
        //[Column(TypeName = "decimal(4, 2)")]
        public decimal EndTime { get; set; }
        //[Column(TypeName = "decimal(4, 2)")]
        //[Display(Name = "Number of Hours")]
        public decimal HoursAmount { get; set; }
        //[Column(TypeName = "decimal(18, 2)")]
        public decimal OB1 { get; set; }
        //[Column(TypeName = "decimal(18, 2)")]
        public decimal OB2 { get; set; }
        public string Karens { get; set; }
        public string SickLeave { get; set; }
        public string VAB { get; set; }
        public string Vacation { get; set; }
        //[Display(Name = "Vacation Compensation")]
        //[Column(TypeName = "decimal(18, 2)")]
        public decimal VacationCompensation { get; set; }
        //[Column(TypeName = "decimal(18, 2)")]
        //[Display(Name = "Extra Cost")]
        public decimal ExtraCost { get; set; }
        public string GPSLocation { get; set; }

        public Customer Customer { get; set; }
        public Employee Employee { get; set; }
    }
}