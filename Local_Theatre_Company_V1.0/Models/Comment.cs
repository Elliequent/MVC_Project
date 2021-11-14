using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Local_Theatre_Company_V1._0.Models
{

    // Ian Fraser - 13/04/2021 - Local_Theatre_Company_V1.0

    public class Comment
    {

        // Class Properties
        [Key]
        [Display(Name = "Comment ID")]
        public int CommentID { get; set; }

        [Display(Name = "Post Body")]
        public string Body { get; set; }

        [Display(Name = "Date Posted")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime PostedOn { get; set; }

        [Display(Name = "Comment Awaiting Approval")]
        public bool CommentBlocked { get; set; }

        // Navigation Properties
        [ForeignKey("Customer")]
        public string UserId { get; set; }
        public virtual Customer Customer { get; set; }


        [ForeignKey("Blog")]
        public int? BlogId { get; set; }
        public virtual Blog Blog { get; set; }


    }   // End of Class


}   // End of NameSpace