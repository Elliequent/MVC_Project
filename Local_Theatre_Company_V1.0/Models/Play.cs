using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Local_Theatre_Company_V1._0.Models
{

    // Ian Fraser - 13/04/2021 - Local_Theatre_Company_V1.0

    public class Play
    {

        // Class Properties

        [Key]
        [Display(Name = "Play ID")]
        public int PlayId { get; set; }

        [Display(Name = "Play Name")]
        public string PlayName { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime PlayStart { get; set; }

        [Display(Name = "End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime PlayEnd { get; set; }

        [Display(Name = "Play Poster")]
        public string PlayPoster { get; set; }

        [Display(Name = "Play Description")]
        public string PlayDescription { get; set; }

        [Display(Name = "Play Genre")]
        public PlayType PlayType { get; set; }


    }   // End of Class

    public enum PlayType
    {

        Comdey,
        Drama,
        Romance,
        Tragedy,
        Pantomime,
        [Display(Name = "One Off")]
        OneOff,
        Event,
        Magic,
        Musical

    }   // End of ENUM PlayType


}   // End of NameSpace