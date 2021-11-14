using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Local_Theatre_Company_V1._0.Models.ViewModels
{

    // Ian Fraser - 18/04/2021 - Local_Theatre_Company_V1.0

    public class EditCustomerViewModel
    {

        [Required]
        [Display(Name = "First Name")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

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
        [DataType(DataType.PostalCode)]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Suspended")]
        public bool IsSuspended { get; set; }

        [Display(Name = "Account Active")]
        public bool IsActive { get; set; }


    }   // End of Class


}   // End of NameSpace