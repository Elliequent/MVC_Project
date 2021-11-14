using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Local_Theatre_Company_V1._0.Models.ViewModels
{

    // Ian Fraser - 18/04/2021 - Local_Theatre_Company_V1.0

    public class ChangeRoleViewModel
    {

        public string UserName { get; set; }

        public string OldRole { get; set; }

        public ICollection<SelectListItem> Roles { get; set; }

        [Required, Display(Name = "Roles")]
        public string Role { get; set; }


    }   // End of Class


}   // End of NameSpace