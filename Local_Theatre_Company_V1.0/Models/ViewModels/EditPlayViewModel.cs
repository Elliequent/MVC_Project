using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Local_Theatre_Company_V1._0.Models.ViewModels
{

    // Ian Fraser - 19/04/2021 - Local_Theatre_Company_V1.0

    public class EditPlayViewModel
    {

        [Display(Name = "Play Name")]
        public string PlayName { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime PlayStart { get; set; }

        [Display(Name = "End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime PlayEnd { get; set; }

        [Display(Name = "Play Poster")]
        public string PlayPoster { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Play Description")]
        public string PlayDescription { get; set; }

        [Display(Name = "Play Genre")]
        public PlayType PlayType { get; set; }


    }   // End of Class


}   // End of NameSpace