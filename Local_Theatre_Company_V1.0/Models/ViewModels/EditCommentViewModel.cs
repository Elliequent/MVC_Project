using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Local_Theatre_Company_V1._0.Models.ViewModels
{

    // Ian Fraser - 29/05/2021 - Local_Theatre_Company_V1.0

    public class EditCommentViewModel
    {

        [Display(Name = "Post Body")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        [Display(Name = "Date Posted")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime PostedOn { get; set; }

        [Display(Name = "Comment Blocked")]
        public bool CommentBlocked { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Blog Blog { get; set; }


    }   // End of Class


}   // End of NameSpace