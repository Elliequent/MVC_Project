using Local_Theatre_Company_V1._0.Models;
using Local_Theatre_Company_V1._0.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Text.RegularExpressions;

namespace Local_Theatre_Company_V1._0.Controllers
{

    // Ian Fraser - 26/05/21 - Local_Theatre_CompanyV1.0

    /// <summary>
    /// Comment Controller Section
    /// Admins / Managers / Moderators can approve comments - comments cannot be seen until approved
    /// Admins / Managers / Moderators / Customers can edit comments
    /// Customers can be suspended for comments by a Admin / Managers / Moderators
    /// Admins / Managers / Moderators can delete comments
    /// </summary>

    // Testing - PASS - 01/06/2021

    [Authorize(Roles = "Admin, Manager, Moderator, Staff, Customer")]
    public class CommentController : AccountController
    {

        // Database connection
        LTCDbContext db = new LTCDbContext();

        [Authorize(Roles = "Admin, Moderator, Manager")]
        public ActionResult Comments(int? id)
        {

            // Creating list of comments associated with blog in order of date posted
            var comments = db.Comments.Where(p => p.BlogId == id).OrderBy(p => p.PostedOn).ToList();

            return View(comments);

        }   // End of ACTIONRESULT Comments() [GET]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Comments(string searchString, int? id)
        {

            // Takes play entered search string and finds database entries containing like terms
            var comments = db.Comments.Where(s => DbFunctions.Like(s.Body, "%" + searchString + "%")).Where(p => p.BlogId == id).ToList();

            // Error prevention
            if (comments == null || comments.Count() == 0)
            {

                ViewBag.Error = "[DB NULL] Error: Comments not found in database";

            }

            return View(comments);

        }   // End of ACTIONRESULT Comments() [POST]


        // View Comments (As part of Blog) Section ------------------------------------------------------------

        [Authorize(Roles = "Admin, Manager, Moderator, Staff, Customer")]
        public ActionResult View_Comments(int? id)
        {

            // Create a list of all comments associated with blog in order of date where not blocked
            var comments = db.Comments.Where(p => p.BlogId == id).Where(a => a.CommentBlocked == false).OrderBy(p => p.PostedOn).ToList();

            // Passing blogId to comment
            ViewBag.ID = id;

            return View(comments);


        }// End of ACTIONRESULT BlogView() [POST]


        // Create Comments Section -------------------------------------------------------------------------------------


        [HttpGet]
        [Authorize(Roles = "Admin, Customer, Moderator, Staff, Manager")]
        public ActionResult CreateComment(int id)
        {

            // Creating instance of CreateCommentViewModel
            CreateCommentViewModel comment = new CreateCommentViewModel();

            // Stores blog ID to pass over
            ViewBag.ID = id;

            return View(comment);

        }   // End of ACTIONRESULT CreateBlog() [GET]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment(CreateCommentViewModel model)
        {

            // Error prevention
            if (ModelState.IsValid)
            {

                // Checks if comment body is blank or null
                if (model.Body != "" && model.Body != null)
                {

                    if (Regex.IsMatch(model.Body, @"^\d+$"))
                    {

                        ViewBag.Error = "[X] Error: Comments cannot just be numbers";

                    }
                    else
                    {

                        // Comment word length limit - 50 characters
                        int maxLength = 50;

                        // If comment is greater than 50 characters
                        if (model.Body.Length > maxLength)
                        {

                            ViewBag.Error = "[X] Error: Over character limit - 50 characters or less";

                        }
                        else
                        {

                            // Create customer list to find cutomer to add to the comment
                            var customer_list = db.Customers.OrderBy(p => p.RegisteredAt).ToArray();
                            Customer customer = new Customer();
                            string username = User.Identity.Name;

                            foreach (var index in customer_list)
                            {

                                if (index.UserName == username)
                                {

                                    customer = index;

                                }

                            }

                            if (User.IsInRole("Customer"))
                            {

                                // Comment created from data passed
                                Comment newComment = new Comment
                                {

                                    CommentID = db.Comments.OrderByDescending(p => p.CommentID).First().CommentID + 1,
                                    Body = model.Body,
                                    Customer = customer,
                                    PostedOn = DateTime.Now,
                                    Blog = db.Blogs.Find(model.BlogID),
                                    CommentBlocked = true

                                };

                                // Adding comment to database
                                db.Comments.Add(newComment);
                                db.SaveChanges();

                                return RedirectToAction("View_Comments", "Comment", new { id = model.BlogID });

                            }
                            else if (User.IsInRole("Suspended"))
                            {

                                ViewBag.Error = "[DB NULL] Error: Suspended members cannot comment on blogs";

                            }
                            else
                            {

                                ViewBag.Error = "[DB NULL] Error: Employees cannot comment on blogs on work accounts";


                            }   // End of IF (User.IsInRole("Customer"))

                            return RedirectToAction("CreateComment", "Comment");

                        }   // End of IF (model.Body.Length > maxLength)

                    }   // End of IF (Regex.IsMatch(model.Body, @"^\d+$"))

                }
                else
                {

                    ViewBag.Error = "[X] Error: Please ensure body are complete before submitting";


                }   // End of IF (model.Body != "" && model.Body != null)

            }   // End of IF (ModelState.IsValid)

            return View(model);

        }// End of ACTIONRESULT CreateComment() [POST]


        // Edit Comment Section -------------------------------------------------------------------------------


        [Authorize(Roles = "Admin, Manager, Moderator, Customer, Staff")]
        [HttpGet]
        public ActionResult EditComment(int? id)
        {

            // Error prevention
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            // Creating comment object from passed id
            Comment comment = db.Comments.Find(id);

            // Error prevention - Checks if return is null
            if (comment == null)
            {

                ViewBag.Error = "[DB NULL] Error: Comment not found in database";

            }

            // Stores blog ID to pass over
            ViewBag.ID = comment.BlogId;

            // Creates view based upon EditCommentViewModel
            return View(new EditCommentViewModel
            {

                Body = comment.Body,
                Blog = comment.Blog,
                CommentBlocked = comment.CommentBlocked,
                Customer = comment.Customer,
                PostedOn = comment.PostedOn
                
            });

        }   // End of ACTIONRESULT EditComment() [GET]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditComment(int id, [Bind(Include = "Body, Blog, CommentBlocked, Customer, PostedOn")] EditCommentViewModel model)
        {

            // Error prevention
            if (ModelState.IsValid)
            {

                if (model.Body == null || model.Body == "")
                {

                    ViewBag.Error = "[X] Error: Comments cannot be blank";
                    return RedirectToAction("EditComment", "Comment");

                }
                else
                {

                    if (Regex.IsMatch(model.Body, @"^\d+$"))
                    {

                        ViewBag.Error = "[X] Error: Comments cannot just be numbers";
                        return RedirectToAction("EditComment", "Comment");

                    }
                    else
                    {


                        // Finds comment by ID
                        Comment comment = await db.Comments.FindAsync(id);

                        // Updating comment information
                        comment.Body = model.Body;
                        comment.Blog = comment.Blog;
                        comment.CommentBlocked = comment.CommentBlocked;
                        comment.Customer = db.Customers.Find(comment.UserId);
                        comment.PostedOn = comment.PostedOn;

                        // Updates play information in database with new changes
                        bool result = TryUpdateModel(comment);

                        // If play updated in database redirect to list - else error message
                        if (result)
                        {

                            // If tree to determine redirect based upon users role
                            if (User.IsInRole("Admin") || User.IsInRole("Manager") || User.IsInRole("Moderator"))
                            {

                                db.SaveChanges();
                                return RedirectToAction("Comments", "Comment", new { id = comment.Blog.BlogID });

                            }

                            if (User.IsInRole("Customer"))
                            {

                                db.SaveChanges();
                                return RedirectToAction("View_Comments", "Comment", new { id = comment.Blog.BlogID });

                            }

                        }
                        else
                        {

                            ViewBag.Error = "[DB SAVE] Error: Comment not saved to database";


                        }   // End of IF (result)

                    }   // End of IF (Regex.IsMatch(model.Body, @"^\d+$"))

                }   // End of IF (model.Body == null || model.Body == "")

            }   // End of IF (ModelState.IsValid)

            return View();

        }   // End of ACTIONRESULT EditComment() [POST]


        // Buttons Section -------------------------------------------------------------------------------------

        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult Approve(int? id)
        {

            // Error prevention
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            // Finding comment in database
            Comment comment = db.Comments.Find(id);

            // Changing comment blocked status and updating db
            comment.CommentBlocked = false;
            var result = TryUpdateModel(comment);

            // Error prevention - result determines action
            if (result)
            {

                db.SaveChanges();

            }
            else
            {

                ViewBag.Error = "[DB SAVE] Error: Database could not update approval";


            }   // End of IF (result)

            return RedirectToAction("Comments", "Comment", new { id = db.Comments.Find(id).BlogId });

        }   // End of ACTIONRESULT Approve()

        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Suspend(int? id)
        {

            // Error prevention
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            // Finding comment in database
            Comment comment = db.Comments.Find(id);

            // Author of comment discovered and role identified
            Customer user = (Customer)UserManager.FindById(comment.Customer.Id);
            string oldRole = (UserManager.GetRoles(comment.Customer.Id)).Single();

            // Remove old role
            UserManager.RemoveFromRole(comment.Customer.Id, "Customer");

            // Add new role (Suspended)
            await UserManager.AddToRoleAsync(comment.Customer.Id, "Suspended");

            // Updating customer records
            user.IsSuspended = true;
            await UserManager.UpdateAsync(user);

            return RedirectToAction("Comments", "Comment", new { id = db.Comments.Find(id).BlogId });

        }   // End of ACTIONRESULT Suspend()

        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> UnSuspend(int? id)
        {

            // Error prevention
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            // Finding comment in database
            Comment comment = db.Comments.Find(id);

            // Author of comment discovered and role identified
            Customer user = (Customer)UserManager.FindById(comment.Customer.Id);
            string oldRole = (UserManager.GetRoles(comment.Customer.Id)).Single();

            // Remove old role
            UserManager.RemoveFromRole(comment.Customer.Id, "Suspended");

            // Add new role (Suspended)
            await UserManager.AddToRoleAsync(comment.Customer.Id, "Customer");

            // Updating customer records
            user.IsSuspended = false;
            await UserManager.UpdateAsync(user);

            return RedirectToAction("Comments", "Comment", new { id = db.Comments.Find(id).BlogId });

        }   // End of ACTIONRESULT Suspend()

        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Delete(int? id)
        {

            // Error prevention
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            // Finding comment in db
            Comment comment = await db.Comments.FindAsync(id);

            string blogID = comment.BlogId.ToString();

            // Error prevention - check if null return
            if (comment == null)
            {

                ViewBag.Error = "[DB NULL] Error: Comment not found in database";

            }

            // Comment removed from database
            db.Comments.Remove(comment);
            db.SaveChanges();

            return RedirectToAction("Comments", "Comment", new { id = blogID });

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