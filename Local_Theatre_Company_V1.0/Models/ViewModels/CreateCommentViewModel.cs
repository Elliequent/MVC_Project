using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Local_Theatre_Company_V1._0.Models.ViewModels
{

    // Ian Fraser - 27/05/2021 - Local_Theatre_CompanyV1.0

    public class CreateCommentViewModel
    {

        [Display(Name = "Blog ID")]
        public int BlogID { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Post Body")]
        public string Body { get; set; }

        [Display(Name = "Comment Blocked")]
        public virtual Customer Customer { get; set; }


    }   // End of Class


}   // End of NameSpace