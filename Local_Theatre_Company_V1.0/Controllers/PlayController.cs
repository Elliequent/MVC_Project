using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Local_Theatre_Company_V1._0.Models;
using Local_Theatre_Company_V1._0.Models.ViewModels;


namespace Local_Theatre_Company_V1._0.Controllers
{

    // Ian Fraser - 13/04/2021 - Local_Theatre_Company_V1.0

    /// <summary>
    /// Play controller section
    /// Admins / Managers can create / Edit / delete plays
    /// Admins / Managers / Staff can view play details 
    /// Customers cannot access this menu
    /// </summary>

    // Testing - PASS - 01/06/2021

    [Authorize(Roles = "Admin, Manager, Staff")]
    public class PlayController : Controller
    {

        // Database connection
        LTCDbContext db = new LTCDbContext();

        [HttpGet]
        public ActionResult Plays()
        {

            // Collecting all users in order of regiseration and converting to list
            var plays = db.Plays.OrderBy(u => u.PlayStart).ToList();

            return View(plays);


        }   // End of ACTIONRESULT Plays() [GET]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Plays(string searchString)
        {

            // Takes play entered search string and finds database entries containing like terms
            var plays = db.Plays.Where(s => DbFunctions.Like(s.PlayName, "%" + searchString + "%")).ToList();

            if (plays == null)
            {

                ViewBag.Error = "No plays found with search string";

            }

            return View(plays);

        }   // End of ACTIONRESULT Plays() [POST]


        // Create Play Section ---------------------------------------------------------------------------------

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult CreatePlay()
        {

            // Creating instance of CreatePlayViewModel to pass to View
            CreatePlayViewModel play = new CreatePlayViewModel();

            return View(play);

        }   // End of ACTIONRESULT CreateEmployee() [GET]

        [HttpPost]
        public ActionResult CreatePlay(CreatePlayViewModel model)
        {

            // Error prevention
            if (ModelState.IsValid)
            {

                // Creating play from user inputs
                Play newPlay = new Play
                {

                    PlayId = db.Plays.OrderByDescending(p => p.PlayId).First().PlayId + 1,
                    PlayName = model.PlayName,
                    PlayStart = model.PlayStart,
                    PlayEnd = model.PlayEnd,
                    PlayPoster = "",                                // Left blank to edit in next section
                    PlayType = model.PlayType,
                    PlayDescription = model.PlayDescription
                    
                };

                // Adding new play to database
                db.Plays.Add(newPlay);
                db.SaveChanges();

                return RedirectToAction("CreatePlayPoster", "Play", new { id = db.Plays.OrderByDescending(p => p.PlayId).First().PlayId });

            }   // End of IF (ModelState.IsValid)

            return View(model);

        }   // End of ACTIONRESULT CreatePlay() [POST]

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult CreatePlayPoster()
        {

            // Redirect from CreatePlay - adds Play Poster to Play
            return View();

        }   // End of ACTIONRESULT CreatePlayPoster() [GET]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePlayPoster(HttpPostedFileBase file, int id)
        {

            // Confirms file has been uploaded
            if (file != null && file.ContentLength > 0)
            {

                // Try to upload file to DB
                try
                {

                    // Creates image path to be saved
                    string path = "/Images/Plays/" + file.FileName;

                    // File confirm message
                    ViewBag.Error = "File uploaded successfully";

                    // Finding blog associated with image
                    Play play = db.Plays.Find(id);

                    // Saving image path to blog db
                    play.PlayPoster = path;
                    db.SaveChanges();

                    return RedirectToAction("Plays", "Play");

                }
                catch (Exception ex)
                {

                    ViewBag.Error = "[DB SAVE] Error: Image did not save to database, MESSAGE:" + ex.Message.ToString();

                }

            }  
            else
            {

                ViewBag.Error = "[X] Error: You have not specified a file.";


            }   // End of IF (file != null && file.ContentLength > 0)

            return View();

        }   // End of ACTIONRESULT CreatePlayPoster() [POST]


        // Edit Play Section ---------------------------------------------------------------------------------

        [Authorize(Roles = "Admin, Manager")]
        [HttpGet]
        public ActionResult EditPlay(int? id)
        {

            // Error prevention
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            // Creating play from passed id
            Play play = db.Plays.Find(id);

            // Error prevention
            if (play == null)
            {

                ViewBag.Error = "[DB NULL] Error: Play not found in database";

            }

            // Creating View with EditPlayViewModel
            return View(new EditPlayViewModel
            {

                PlayName = play.PlayName,
                PlayStart = play.PlayStart,
                PlayEnd = play.PlayEnd,
                PlayPoster = play.PlayPoster,
                PlayType = play.PlayType,
                PlayDescription = play.PlayDescription

            });

        }   // End of ACTIONRESULT EditPlay() [GET]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPlay(int id, [Bind(Include = "PlayName, PlayStart, PlayEnd, PlayPoster, PlayType")] EditPlayViewModel model)
        {

            // Error prevention
            if (ModelState.IsValid)
            {

                // Finds play by ID
                Play play = await db.Plays.FindAsync(id);

                // Updating play information
                play.PlayName = model.PlayName;
                play.PlayStart = model.PlayStart;
                play.PlayEnd = model.PlayEnd;
                play.PlayPoster = model.PlayPoster;
                play.PlayType = model.PlayType;

                // Updates play information in database with new changes
                bool result = TryUpdateModel(play);
                db.SaveChanges();

                // If play updated in database redirect to list - else error message
                if (result)
                {

                    return RedirectToAction("Plays", "Play");

                }
                else
                {

                    ViewBag.Message = "[DB SAVE] Error: Play not saved to database";

                }

            }   // End of IF (ModelState.IsValid)

            return View();

        }   // End of ACTIONRESULT EditPlay() [POST]


        // Play details section -------------------------------------------------------------------------------

        [Authorize(Roles = "Admin, Manager, Staff")]
        public ActionResult Details(int? id)
        {

            // Error prevention
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            // Extracting play with passed id
            Play plays = db.Plays.Find(id);

            if (plays == null)
            {

                ViewBag.Error = "[DB NULL] Error: Play not found in database";
                return RedirectToAction("Plays", "Play");

            }
            else
            {

                return View("DetailsPlay", plays);

            }

        }   // End of Details()


        // Delete play Section ----------------------------------------------------------------------------------------

        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> Delete(int? id)
        {

            // Error prevention
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            // Extracting plays with passed id
            Play play = await db.Plays.FindAsync(id);

            // Error prevention
            if (play == null)
            {

                ViewBag.Error = "[DB NULL] Error: Play not found in database";
                return RedirectToAction("Plays", "Play");

            }

            // Removing play from database
            db.Plays.Remove(play);
            db.SaveChanges();

            return RedirectToAction("Plays", "Play");

        }   // End of TASK<ACTIONRESULT> Delete()

        protected override void Dispose(bool disposing)
        {

            // Dispose method releases the resources currently
            // being used by the controller method and closes
            // DB connection for security

            if (disposing)
            {

                db.Dispose();

            }

            base.Dispose(disposing);

        }   // End of METHOD Dispose()


    }   // End of Class


}   // End of NameSpace