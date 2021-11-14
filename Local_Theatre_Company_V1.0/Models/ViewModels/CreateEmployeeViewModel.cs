using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Local_Theatre_Company_V1._0.Models.ViewModels
{

    // Ian Fraser - 15/04/2021 - Local_Theatre_Company_V1.0

    public class CreateEmployeeViewModel
    {

        // User Variables - Person
        [Required]
        [Display(Name = "First Name")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        // User Variables - Location
        [Required]
        [Display(Name = "Address Line 1")]
        [DataType(DataType.Text)]
        public string Address1 { get; set; }

        [Display(Name = "Address Line 2")]
        [DataType(DataType.Text)]
        public string Address2 { get; set; }

        [Required]
        [Display(Name = "City")]
        [DataType(DataType.Text)]
        public string City { get; set; }

        [Required]
        [Display(Name = "Post code")]
        [DataType(DataType.PostalCode)]
        public string PostCode { get; set; }

        // User Variables - Account
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Employment Status")]
        public EmploymentStatus EmploymentStatus { get; set; }

        public string Role { get; set; }

        public ICollection<SelectListItem> Roles { get; set; }


    }   // End of Class


}   // End of NameSpace