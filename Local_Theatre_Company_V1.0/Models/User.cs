using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Local_Theatre_Company_V1._0.Models
{

    // Ian Fraser - 10/04/2021 - Local_Theatre_Company_v1.0

    public abstract class User : IdentityUser
    {

        // Class properties

        // User name details
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // User Address Details
        [Display(Name = "Address Line 1")]
        public string Address1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string Address2 { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        // User Account status Details
        [Display(Name = "Date Registered")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RegisteredAt { get; set; }

        [Display(Name = "Account Active")]
        public bool IsActive { get; set; }

        // User current role
        private ApplicationUserManager userManager;

        [NotMapped]
        public string CurrentRole
        {

            get
            {

                if (userManager == null)
                {

                    userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

                }

                return userManager.GetRoles(Id).Single();

            }

        }   // End of Method CurrentRole()

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {

            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;

        }   // End of Method GenerateUserIdentityAsync()


    }   // End of Class


}   // End of Name Space