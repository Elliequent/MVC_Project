using Local_Theatre_Company_V1._0.Models;
using Local_Theatre_Company_V1._0.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Local_Theatre_Company_V1._0.Controllers
{

    // Ian Fraser - 21/05/21 - Local_Theatre_Company_V1.0

    /// <summary>
    /// Blog controller section
    /// Admins / Managers / Moderators can create blogs
    /// Admins / Managers / Moderators can edit blogs
    /// Admins / Managers / Moderators can delete blogs - deleted blogs delete associated comments
    /// Blogs do not have a character limit
    /// Blogs have an associated image added to them
    /// Admins / Managers/ Moderators can view the details of blogs
    /// Admins / Managers / Moderators can create new blog catagories
    /// Admins / Managers / Moderators can change blog catagories
    /// </summary>

    // Testing - PASS - 01/06/2021

    [Authorize(Roles = "Admin, Manager, Moderator")]
    public class BlogController : Controller
    {

        // Database connection
        LTCDbContext db = new LTCDbContext();


        public ActionResult Blogs()
        {

            // Collecting all blogs in order of regiseration and converting to list
            var blogs = db.Blogs.OrderBy(u => u.PostedOn).ToList();

            return View(blogs);

        }   // End of ACTIONRESULT Blogs() [GET]


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Blogs(string searchString)
        {

            // Takes play entered search string and finds database entries containing like terms
            var blogs = db.Blogs.Where(s => DbFunctions.Like(s.Title, "%" + searchString + "%")).ToList();

            // Error prevention
            if (blogs == null || blogs.Count() == 0)
            {

                ViewBag.Error = "No blogs found with search string";

            }

            return View(blogs);

        }   // End of ACTIONRESULT Blogs() [POST]


        // Create Blog Section ---------------------------------------------------------------------------------


        [HttpGet]
        public ActionResult CreateBlog()
        {

            // Creating instance of CreateBlogViewModel to populate with SelectListItem below
            CreateBlogViewModel blog = new CreateBlogViewModel();

            // Populates drop down list of all catagories in database
            var catagories = db.Catagories.Select(r => new SelectListItem
            {

                Text = r.Name,
                Value = r.CategoryId.ToString()

            }).ToList();

            // Assigning Catagories
            blog.Catagories = catagories;

            return View(blog);

        }   // End of ACTIONRESULT CreateBlog() [GET]


        [HttpPost]
        public ActionResult CreateBlog(CreateBlogViewModel model)
        {

            // Error prevention
            if (ModelState.IsValid)
            {

                // Error prevention - if body or title is blank
                if ((model.Body != "" && model.Body != null) && (model.Title != "" && model.Title != null) )
                {

                    // Creating employee list to find employee
                    var employee_list = db.Employees.OrderBy(p => p.RegisteredAt).ToArray();
                    Employee employee = new Employee();
                    string username = User.Identity.Name;

                    foreach (var index in employee_list)
                    {

                        if (index.UserName == username)
                        {

                            employee = index;

                        }

                    }

                    // Creating blog from user inputs
                    Blog newBlog = new Blog
                    {

                        BlogID = db.Blogs.OrderByDescending(p => p.BlogID).First().BlogID + 1,
                        Title = model.Title,
                        Body = model.Body,
                        BlogImage = "",
                        Employee = employee,
                        Published = model.Published,
                        PostedOn = DateTime.Now,
                        Catagory = db.Catagories.Find(Int32.Parse(model.Catagory))

                    };

                    // Adding new blog to database
                    db.Blogs.Add(newBlog);
                    db.SaveChanges();

                    return RedirectToAction("CreateBlogImage", "Blog", new { id = db.Blogs.OrderByDescending(p => p.BlogID).First().BlogID });

                }
                else if (Regex.IsMatch(model.Title, @"^\d+$") || Regex.IsMatch(model.Body, @"^\d+$"))
                {

                    ViewBag.Error = "[X] Error: Please ensure both title and body are not just numbers";

                }
                else
                {
                    
                    ViewBag.Error = "[X] Error: Please ensure both title and body are complete before submitting";


                }   // End of IF ((model.Body != "" && model.Body != null) && (model.Title != "" && model.Title != null))

            }   // End of IF (ModelState.IsValid)

            return RedirectToAction("CreateBlog", "Blog");

        }   // End of ACTIONRESULT CreateBlog() [POST]


        [HttpGet]
        public ActionResult CreateBlogImage()
        {

            // Redirects from CreateBlog() - allows users to add an image to a blog
            return View();


        }   // End of ACTIONRESULT CreateBlogImage() [GET]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBlogImage(HttpPostedFileBase file, int id)
        {

            // Confirms file has been uploaded
            if (file != null && file.ContentLength > 0)

                // Try to upload file to DB
                try
                {

                    // Creates image path to be saved
                    string path = "/Images/" + file.FileName;

                    // File confirm message
                    ViewBag.Error = "File uploaded successfully";

                    // Finding blog associated with image
                    Blog blog = db.Blogs.Find(id);

                    // Saving image path to blog db
                    blog.BlogImage = path;
                    db.SaveChanges();

                    return RedirectToAction("Blogs", "Blog");

                }
                catch (Exception ex)
                {

                    ViewBag.Error = "[DB SAVE] Error: Image did not save to database, MESSAGE: " + ex.Message.ToString();

                }
            else
            {

                ViewBag.Error = "[X] Error: You have not specified a file.";


            }   // End of IF (file != null && file.ContentLength > 0)

            return View();

        }   // End of ACTIONRESULT CreateBlogImage() [POST]


        // Edit Blog Section ---------------------------------------------------------------------------------


        [HttpGet]
        public ActionResult EditBlog(int? id)
        {

            // Error prevention
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            // Extracting blog from passed id
            Blog blog = db.Blogs.Find(id);

            // Error prevention
            if (blog == null)
            {

                ViewBag.Error = "[DB NULL] Error: Blog not found in database";

            }

            // Extract all roles within database
            var catagories_ListItem = db.Catagories.Select(r => new SelectListItem
            {

                Text = r.Name.ToString(),
                Value = r.CategoryId.ToString(),
                Selected = r.CategoryId == blog.CatagoryId

            }).ToList();

            // Passed EditBlogViewModel values to View
            return View(new EditBlogViewModel
            {

                Title = blog.Title,
                Body = blog.Body,
                PostedOn = blog.PostedOn,
                Published = blog.Published,
                BlogImage = blog.BlogImage,
                Catagories = catagories_ListItem,
                CatagoryId = blog.CatagoryId

            });

        }   // End of ACTIONRESULT EditBlog() [GET]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditBlog(int id, [Bind(Include = "Title, Body, BlogImage, PostedOn, Published, CatagoryId, Catagories")] EditBlogViewModel model)
        {

            // Error prevention
            if (ModelState.IsValid)
            {

                if ((model.Title == null || model.Title == "") || (model.Body == null || model.Body == ""))
                {

                    ViewBag.Error = "[X] Error: Blog Title or Body cannot be blank";
                    return RedirectToAction("EditBlog", "Blog");

                }
                else if (Regex.IsMatch(model.Title, @"^\d+$") || Regex.IsMatch(model.Body, @"^\d+$"))
                {

                    ViewBag.Error = "[X] Error: Blog Title or Body cannot just be numbers";
                    return RedirectToAction("EditBlog", "Blog");

                }
                else
                {

                    // Finds Blog by ID
                    Blog blog = await db.Blogs.FindAsync(id);

                    // Updating Blog information
                    blog.Title = model.Title;
                    blog.Body = model.Body;
                    blog.PostedOn = model.PostedOn;
                    blog.Published = model.Published;
                    blog.BlogImage = model.BlogImage;
                    blog.CatagoryId = model.CatagoryId;

                    // Updates Blog information in database with new changes
                    bool result = TryUpdateModel(blog);
                    db.SaveChanges();

                    // If Blog updated in database redirect to list - else error message
                    if (result)
                    {

                        return RedirectToAction("Blogs", "Blog");

                    }
                    else
                    {

                        ViewBag.Error = "[DB SAVE] Error: Blog not saved to database";

                    }

                }   // End of IF ((model.Title == null || model.Title == "") || (model.Body == null || model.Body == ""))

            }   // End of IF (ModelState.IsValid)

            return View();

        }   // End of ACTIONRESULT EditBlog() [POST]


        // Details Blog Section ---------------------------------------------------------------------------------


        public ActionResult DetailsBlog(int? id)
        {

            // Error prevention
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            // Extract blog with passed id
            Blog blog = db.Blogs.Find(id);

            // Error prevention
            if (blog == null)
            {

                ViewBag.Error = "[DB SAVE] Error: Blog not saved to database";
                return RedirectToAction("Blogs", "Blog");

            }
            else
            {

                return View("DetailsBlog", blog);

            }

        }   // End of Details()


        // Create Catagory Section --------------------------------------------------------------------------------------


        [HttpGet]
        public ActionResult CreateCatagory()
        {

            // Displays textbox so user can enter value that creates a new catagory
            return View();


        }   // End of ACTION RESULT CreateCatagory() [GET]

        [HttpPost]
        public ActionResult CreateCatagory(Catagory model)
        {

            // Collects all catagories that match user input
            var database_check = db.Catagories.Where(p => p.Name.ToLower() == model.Name.ToLower()).ToList();

            // Error prevention
            if (ModelState.IsValid)
            {

                // Error prevention - if name is blank
                if (model.Name != "" && model.Name != null)
                {

                    if (Regex.IsMatch(model.Name, @"^\d+$"))
                    {

                        ViewBag.Error = "[X] Error: Catagory name cannot be just numbers";

                    }
                    else
                    {

                        // If catagory found in database then error message sent
                        if (database_check.Count > 0)
                        {

                            ViewBag.Error = "[X] Error: Catagory already exists";

                        }
                        else
                        {

                            // Creating catagory from user input
                            Catagory catagory = new Catagory
                            {

                                CategoryId = db.Catagories.OrderByDescending(p => p.CategoryId).First().CategoryId + 1,
                                Name = model.Name

                            };

                            // Adding catagory to database
                            db.Catagories.Add(catagory);
                            db.SaveChanges();

                            return RedirectToAction("Blogs", "Blog");

                        }   // End of IF (database_check.Count > 0)

                    }   // End of IF (Regex.IsMatch(model.Name, @"^\d+$"))

                }
                else
                {

                    ViewBag.Error = "[X] Error: Please enter a value before submitting";


                }   // End of IF (model.Name != "" && model.Name != null)

            }   // End of IF (ModelState.IsValid)

            return View(model);

        }   // End of ACTIONRESULT CreateCatagory() [POST]


        // Delete Blog Section ----------------------------------------------------------------------------------------


        public async Task<ActionResult> Delete(int? id)
        {

            // Error prevention
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            // Finding blog to be deleted within db
            Blog blog = await db.Blogs.FindAsync(id);

            // Finds comments associated with deleted blog
            var comments = db.Comments.Where(p => p.BlogId == id).ToList();

            // Error prevention
            if (blog == null)
            {

                return HttpNotFound();

            }

            // Removes user selected blog from db
            db.Blogs.Remove(blog);

            // Removing each comment associated with deleted blog
            foreach (var comment in comments)
            {

                db.Comments.Remove(comment);

            }

            db.SaveChanges();

            return RedirectToAction("Blogs", "Blog");

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