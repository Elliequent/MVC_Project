using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Local_Theatre_Company_V1._0.Models.ViewModels
{

    // Ian Fraser - 21/05/21 - Local_Theatre_Company_V1.0

    public class EditBlogViewModel
    {

        // Class Properties
        [Display(Name = "Blog ID")]
        public int BlogID { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Post Body")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        [Display(Name = "Date Posted")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime PostedOn { get; set; }

        [Display(Name = "Blog Published")]
        public bool Published { get; set; }

        [Display(Name = "Blog Image")]
        public string BlogImage { get; set; }

        public ICollection<SelectListItem> Catagories { get; set; }

        [Display(Name = "Catagory")]
        public int? CatagoryId { get; set; }


    }   // End of Class

}   // End of NameSpace