using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Local_Theatre_Company_V1._0.Models
{

    // Ian Fraser - 10/04/2021 - Local_Theatre_Company_V1.0

    public class Customer : User
    {

        [Display(Name = "Account Suspended")]
        public bool IsSuspended { get; set; }


        // Navigational Properties
        public virtual ICollection<Comment> Comments { get; set; }


    }   // End of Class


}   // End of NameSpace