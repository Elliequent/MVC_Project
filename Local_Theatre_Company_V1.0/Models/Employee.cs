using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Local_Theatre_Company_V1._0.Models
{

    // Ian Fraser - 10/04/2021 - Local_Theatre_Company_V1.0

    public class Employee : User
    {

        [Display(Name = "Employment Status")]
        public EmploymentStatus EmploymentStatus { get; set; }


        // Navigational Properties
        public virtual ICollection<Blog> Blogs { get; set; }


    }   // End of Class

    public enum EmploymentStatus
    {

        [Display(Name = "Full Time")]
        FullTime,
        [Display(Name = "Part Time")]
        PartTime

    }   // End of ENUM EmploymentStatus


}   // End of NameSpace