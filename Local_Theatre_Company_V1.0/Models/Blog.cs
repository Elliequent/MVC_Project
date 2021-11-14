using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Local_Theatre_Company_V1._0.Models
{

    // Ian Fraser - 13/04/2021 - Local_Theatre_Company_V1.0

    public class Blog
    {

        // Class Properties
        [Key]
        [Display(Name = "Blog ID")]
        public int BlogID { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Post Body")]
        public string Body { get; set; }

        [Display(Name = "Date Posted")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime PostedOn { get; set; }

        [Display(Name = "Blog Published")]
        public bool Published { get; set; }

        [Display(Name = "Blog Image")]
        public string BlogImage { get; set; }


        // Navigation Properties
        [ForeignKey("Employee")]
        public string UserId { get; set; }
        public virtual Employee Employee { get; set; }

        [ForeignKey("Catagory")]
        public int? CatagoryId { get; set; }
        public virtual Catagory Catagory { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }


    }   // End of Class


}   // End of NameSpace