using Local_Theatre_Company_V1._0.Models;
using Local_Theatre_Company_V1._0.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Local_Theatre_Company_V1._0.Controllers
{

    // Ian Fraser - 20/04/2021 - Local_Theatre_Company_V1.0

    /// <summary>
    /// Main Navbar links + landing page
    /// Landing Page - Video playing in background with transparent dark overlay
    /// What's on - Dynamic site that display details of plays as they come into date
    /// About - Details about company
    /// Contact - How to contact company
    /// Blog - LTC Blog - requires account to use
    /// </summary>

    // Testing - PASS - 01/06/2021

    public class HomeController : Controller
    {

        // Database connection
        LTCDbContext db = new LTCDbContext();

        public ActionResult Index()
        {

            // Main Landing page
            return View();

        }

        public ActionResult About()
        {

            // About Local Theatre Company
            return View();

        }

        public ActionResult Contact()
        {

            // Contact details for company including Google Maps
            return View();

        }

        public ActionResult WhatsOn()
        {

            // Finds play currently playing within the current date range 
            var current_play = db.Plays.Where(p => p.PlayStart < DateTime.Now && p.PlayEnd > DateTime.Now).Single();

            // Error detection - redirect to landing page
            if (current_play == null)
            {

                return RedirectToAction("Index", "Home");

            }

            // View created with WhatsOnViewModel
            return View(new WhatsOnViewModel { 
            
                PlayName = current_play.PlayName,
                PlayDescription = current_play.PlayDescription,
                PlayPoster = current_play.PlayPoster,
                PlayStart = current_play.PlayStart,
                PlayEnd = current_play.PlayEnd,
                PlayType = current_play.PlayType

            });

        }   // End of ACTIONRESULT WhatsOn()

        public ActionResult Blog()
        {

            // Populating blog with list of blogs that are published in order of date posted
            var blogs = db.Blogs.OrderBy(u => u.PostedOn).Where(p => p.Published == true).ToList();

            return View(blogs);


        }   // End of ACTIONRESULT Blog() [GET]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Blog(string searchString)
        {

            // Takes blog entered search string and finds catagories containing like terms
            var blogs = db.Blogs.Where(s => DbFunctions.Like(s.Catagory.Name, "%" + searchString + "%")).Where(p => p.Published == true).ToList();

            // Error prevention
            if (blogs == null || blogs.Count() == 0)
            {

                ViewBag.Error = "[DB NULL] Error: No Catagories found with search string";

            }

            return View(blogs);

        }   // End of ACTIONRESULT Plays() [POST]


    }   // End of Class


}   // End of NameSpace