using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Local_Theatre_Company_V1._0.Models
{

    // Ian Fraser - 20/05/21 - Local_Theatre_Company_V1.0

    public class Catagory
    {

        [Key]
        [Display(Name = "Category ID")]
        public int CategoryId { get; set; }

        [Display(Name = "Category Name")]
        public string Name { get; set; }

        public Catagory()
        {

            Blogs = new List<Blog>();

        }

        // Navigational Properties
        public virtual ICollection<Blog> Blogs { get; set; }


    }   // End of Class


}   // End of NameSpace