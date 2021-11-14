using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Local_Theatre_Company_V1._0.Models.ViewModels
{

    // Ian Fraser - 19/04/2021 - Local_Theatre_Company_V1.0

    public class CreatePlayPosterViewModels
    {

        [Display(Name = "Image File")]
        public HttpPostedFileBase ImageFile { get; set; }


    }   // End of Class


}   // End of NameSpace