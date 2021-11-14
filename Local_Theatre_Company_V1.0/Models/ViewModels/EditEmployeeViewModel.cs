using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Local_Theatre_Company_V1._0.Models.ViewModels
{

    // Ian Fraser - 18/04/2021 - Local_Theatre_Company_V1.0

    public class EditEmployeeViewModel
    {

        // User Variables - Person
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // User Variables - Location
        [Display(Name = "Address Line 1")]
        public string Address1 { get; set; }
        [Display(Name = "Address Line 2")]
        public string Address2 { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Post code")]
        public string PostCode { get; set; }

        // User Variables - Account
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Email Confirm")]
        public bool EmailConfirm { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Phone Confirm")]
        public bool PhoneConfirm { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Employment Status")]
        public EmploymentStatus EmploymentStatus { get; set; }

        public string Role { get; set; }

        public ICollection<SelectListItem> Roles { get; set; }


    }   // End of Class


}   // End of NameSpace