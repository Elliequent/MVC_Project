using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Local_Theatre_Company_V1._0.Models;
using Local_Theatre_Company_V1._0.Models.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using System.Text.RegularExpressions;

namespace Local_Theatre_Company_V1._0.Controllers
{

    // Ian Fraser - 13/04/2021 - Local_Theatre_Company_V1.0

    /// <summary>
    /// Admin controller section
    /// Allows Admin / Manager to add / change / delete the information of other users
    /// Allows Admin / Manager to create / change roles of other users
    /// Allows Admin / Manager to view the details of other user accounts including employees and customers
    /// </summary>

    // Testing - PASS - 01/06/2021

    [Authorize(Roles = "Admin , Manager")]
    public class AdminController : AccountController
    {

        // Database connection
        LTCDbContext db = new LTCDbContext();

        // Constructors
        public  AdminController() : base()
        {

            // Empty

        }

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager) : base(userManager, signInManager)
        {

            // Empty

        }


        // Main Employee Section ---------------------------------------------------------------------------------


        public ActionResult Users()
        {

            // Collecting all users in order of regiseration and converting to list
            var staff = db.Users.OrderBy(u => u.RegisteredAt).ToList();

            return View(staff);


        }   // End of ACTIONRESULT Users() [GET]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Users(string searchString)
        {

            // Takes user entered search string and finds database entries containing like terms
            var staff = db.Users.Where(s => DbFunctions.Like(s.UserName, "%" + searchString + "%")).ToList();

            // Error prevention - check if null return
            if (staff == null || staff.Count() == 0)
            {

                ViewBag.Error = "No users found";

            }

            return View(staff);

        }   // End of ACTIONRESULT Users() [POST]


        // Create Employee Section ---------------------------------------------------------------------------------


        [HttpGet]
        public ActionResult CreateEmployee()
        {

            // Creating instance of CreateEmployeeViewModel to assign SelectListItem below
            CreateEmployeeViewModel employee = new CreateEmployeeViewModel();

            // Populates drop down list of all roles in database
            var roles = db.Roles.Select(r => new SelectListItem
            {

                Text = r.Name,
                Value = r.Name

            }).ToList();

            // Assigning SelectListItems Roles to CreateEmployeeViewModel
            employee.Roles = roles;

            return View(employee);

        }   // End of ACTIONRESULT CreateEmployee() [GET]


        [HttpPost]
        public ActionResult CreateEmployee(CreateEmployeeViewModel model)
        {

            // Error prevention - check if model return is valid
            if (ModelState.IsValid)
            {

                if ((model.FirstName == null || model.FirstName == "") || (model.LastName == null || model.LastName == "") || (model.Address1 == null || model.Address1 == "") || (model.City == null || model.City == "") || (model.Email == null || model.Email == "") || (model.PostCode == null || model.PostCode == ""))
                {

                    ViewBag.Error = "[X] Error: Please be sure all fields are not blank";
                    return RedirectToAction("CreateEmployee", "Admin");

                }
                else if (Regex.IsMatch(model.FirstName, @"^\d+$") || Regex.IsMatch(model.LastName, @"^\d+$") || Regex.IsMatch(model.City, @"^\d+$"))
                {

                    ViewBag.Error = "[X] Error: Please be sure all relevant fields are text";
                    return RedirectToAction("CreateEmployee", "Admin");

                }
                else
                {

                    // Creating Employee object with returned values
                    Employee newEmployee = new Employee
                    {

                        UserName = model.Email,
                        Email = model.Email,
                        Address1 = model.Address1,
                        Address2 = model.Address2,
                        City = model.City,
                        PostCode = model.PostCode,
                        PhoneNumber = model.PhoneNumber,
                        PhoneNumberConfirmed = true,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        EmploymentStatus = model.EmploymentStatus,
                        IsActive = true,
                        RegisteredAt = DateTime.Now

                    };

                    // Adding newEmployee to database
                    var result = UserManager.Create(newEmployee, model.Password);

                    // If Employee object created successfully
                    if (result.Succeeded)
                    {

                        UserManager.AddToRole(newEmployee.Id, model.Role);
                        return RedirectToAction("Users", "Admin");

                    }
                    else
                    {

                        ViewBag.Error = "[DB SAVE] Error: Employee failed to save to database";

                    }

                }   // End of IF ((model.FirstName == null || model.FirstName == "") || (model.LastName == null...

            }   // End of IF (ModelState.IsValid)

            return View(model);

        }   // End of ACTIONRESULT CreateEmployee() [POST]


        // Edit Employee Section ---------------------------------------------------------------------------------


        [HttpGet]
        public ActionResult EditStaff(string id)
        {

            // Error prevention - check if null return
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            // Search DB for Employee id
            Employee staff = db.Users.Find(id) as Employee;

            // Error prevention - check if null return
            if (staff == null)
            {

                ViewBag.Error = "[DB NULL] Error: No employee found with matching ID";

            }

            // Returning employee information for editing with EditEmployeeViewModel
            return View(new EditEmployeeViewModel
            {

                Address1 = staff.Address1,
                Address2 = staff.Address2,
                City = staff.City,
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                PostCode = staff.PostCode,
                EmploymentStatus = staff.EmploymentStatus,
                Email = staff.Email,
                PhoneNumber = staff.PhoneNumber,
                Role = staff.CurrentRole

            });

        }   // End of ACTIONRESULT EditStaff() [GET]


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditStaff(string id, [Bind(Include = "FirstName, LastName, Address1, Address2, City, PostCode, Email, PhoneNumber, EmploymentStatus")] EditEmployeeViewModel model)
        {

            // Error check - if model is valid
            if (ModelState.IsValid)
            {

                if ((model.FirstName == null || model.FirstName == "") || (model.LastName == null || model.LastName == "") || (model.Address1 == null || model.Address1 == "") || (model.City == null || model.City == "") || (model.Email == null || model.Email == "") || (model.PostCode == null || model.PostCode == ""))
                {

                    ViewBag.Error = "[X] Error: Please be sure all fields are not blank";

                }
                else if (Regex.IsMatch(model.FirstName, @"^\d+$") || Regex.IsMatch(model.LastName, @"^\d+$") || Regex.IsMatch(model.City, @"^\d+$"))
                {

                    ViewBag.Error = "[X] Error: Please be sure none of the text fields have numbers";

                }
                else
                {

                    // Finds user by ID
                    Employee staff = (Employee)await UserManager.FindByIdAsync(id);

                    // Updaing new staff details
                    UpdateModel(staff);
                    IdentityResult result = await UserManager.UpdateAsync(staff);

                    // If update successful
                    if (result.Succeeded)
                    {

                        return RedirectToAction("Users", "Admin");

                    }
                    else
                    {

                        ViewBag.Error = "[DB SAVE] Error: Employee edits did not save to database";


                    }   // End of IF (result.Succeeded)

                }   // End of IF ((model.FirstName == null || model.FirstName == "") || (model.LastName == null || model.LastName...

            }   // End of IF (ModelState.IsValid)

            return View();

        }   // End of ACTIONRESULT EditStaff() [POST]


        // Edit Customer Section ---------------------------------------------------------------------------------


        [HttpGet]
        public ActionResult EditCustomer(string id)
        {

            // Error prevention - check if id is null
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            // Finding customer within DB with id
            Customer customer = db.Users.Find(id) as Customer;

            if (customer == null)
            {

                ViewBag.Error = "[DB NULL] Error: No customer found with matching ID";

            }

            // Returning customer details via EditCustomerViewModel
            return View(new EditCustomerViewModel
            {

                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address1 = customer.Address1,
                Address2 = customer.Address2,
                PostCode = customer.PostCode,
                City = customer.City,
                PhoneNumber = customer.PhoneNumber,
                IsSuspended = customer.IsSuspended,
                IsActive = customer.IsActive

            });

        }   // End of ACTIONRESULT EditCustomers() [GET]


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCustomer(string id, [Bind(Include = "FirstName, LastName, Address1, Address2, City, PostCode, PhoneNumber, IsSuspended")] EditCustomerViewModel model)
        {

            // Error prevention - if model is valid
            if (ModelState.IsValid)
            {

                if ((model.FirstName == null || model.FirstName == "") || (model.LastName == null || model.LastName == "") || (model.Address1 == null || model.Address1 == "") || (model.City == null || model.City == "") || (model.PostCode == null || model.PostCode == ""))
                {

                    ViewBag.Error = "[X] Error: Please be sure all fields are not blank";

                }
                else if (Regex.IsMatch(model.FirstName, @"^\d+$") || Regex.IsMatch(model.LastName, @"^\d+$") || Regex.IsMatch(model.City, @"^\d+$"))
                {

                    ViewBag.Error = "[X] Error: Please be sure none of the text fields have numbers";

                }
                else
                {

                    // Creating customer object from returned information
                    Customer customer = (Customer)await UserManager.FindByIdAsync(id);

                    // Updating customer information
                    UpdateModel(customer);
                    IdentityResult result = await UserManager.UpdateAsync(customer);

                    // If result successful
                    if (result.Succeeded)
                    {

                        return RedirectToAction("Users", "Admin");

                    }
                    else
                    {

                        ViewBag.Error = "[DB SAVE] Error: Customer edits did not save to database";


                    }   // End of IF (result.Succeeded)

                }   // End of IF ((model.FirstName == null || model.FirstName == "") || (model.LastName == null || model.LastName... 

            }   // End of IF (ModelState.IsValid)

            return View(model);

        }   // End of ACTIONRESULT EditCustomer() [POST]


        // Staff and Customer details section -------------------------------------------------------------------------


        public ActionResult Details(string id)
        {

            // Error prevention - if id is null
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            // Creating user from id
            User user = db.Users.Find(id);

            // Error prevention - if user not found
            if (user == null)
            {

                ViewBag.Error = "[DB NULL] Error: No user found with matching ID";

            }

            // If branch for different inherited users
            if (user is Employee)
            {

                return View("DetailsStaff", (Employee)user);

            }

            if (user is Customer)
            {

                return View("DetailsCustomer", (Customer)user);

            }

            // Error prevention - only achieved when user is not found / not customer or employee
            return HttpNotFound();

        }   // End of Details()


        // Create Role Section --------------------------------------------------------------------------------------


        [HttpGet]
        public ActionResult CreateRole()
        {

            // Uses CreateRoleViewModel to create View()
            return View();


        }   // End of ACTION RESULT CreateRole() [GET]


        [HttpPost]
        public ActionResult CreateRole(RoleViewModel model)
        {

            // Error prevention
            if (ModelState.IsValid)
            {

                if (model.RoleName == "" || model.RoleName == null)
                {

                    ViewBag.Error = "[X] Error: Role cannot be blank";

                }
                else if (Regex.IsMatch(model.RoleName, @"^\d+$"))
                {

                    ViewBag.Error = "[X] Error: Roles must be words, not numbers";

                }
                else
                {

                    // Creating role manager object to change add new role
                    RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

                    // Error prevention - if role exist
                    if (!roleManager.RoleExists(model.RoleName))
                    {

                        // Adds role to role DB
                        roleManager.Create(new IdentityRole(model.RoleName));
                        return RedirectToAction("Users", "Admin");

                    }

                }   // End of IF (model.RoleName == "" || model.RoleName == null)

            }   // End of IF (ModelState.IsValid)

            return View(model);

        }   // End of ACTIONRESULT CreateRole() [POST]


        // Change Role Section -------------------------------------------------------------------------------------


        [HttpGet]
        public async Task<ActionResult> ChangeRole(string id)
        {

            // Error prevention
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            // If user is changing their own role = redirect
            if (id == User.Identity.GetUserId())
            {

                return RedirectToAction("Users", "Admin");

            }

            // Extracts the users current role
            User user = await UserManager.FindByIdAsync(id);
            string oldRole = (await UserManager.GetRolesAsync(id)).Single();

            // Extract all roles within database in SelectListItem
            var items = db.Roles.Select(r => new SelectListItem
            {

                Text = r.Name,
                Value = r.Name,
                Selected = r.Name == oldRole

            }).ToList();

            // Returns values with ChangeRoleViewModel
            return View(new ChangeRoleViewModel
            {

                UserName = user.UserName,
                Roles = items,
                OldRole = oldRole

            });

        }   // End of ACTIONRESULT ChangeRole() [GET]


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ChangeRole")]
        public async Task<ActionResult> ChangeRoleConfirmed(string id, [Bind(Include = "Role, oldRole")] ChangeRoleViewModel model)
        {

            // If user is changing their own role
            if (id == User.Identity.GetUserId())
            {

                return RedirectToAction("Users", "Admin");

            }

            // Error prevention
            if (ModelState.IsValid)
            {

                // Creating user manager to change user role
                ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                string userRole = userManager.GetRoles(id).Single();

                // If user is not an employee
                if (userRole == "Customer" || userRole == "Suspended")
                {

                    // Creating customer object with returned id
                    Customer user = (Customer)await UserManager.FindByIdAsync(id);
                    string oldRole = (await UserManager.GetRolesAsync(id)).Single();

                    // If new role is the same as current role
                    if (oldRole == model.Role)
                    {

                        ViewBag.Error = "[X] Error: Current role and new role are the same";

                    }

                    // Remove old role
                    await UserManager.RemoveFromRoleAsync(id, oldRole);

                    // Add new role
                    await UserManager.AddToRoleAsync(id, model.Role);

                    // If role is changed to suspended - update user records
                    if (model.Role == "Suspended")
                    {

                        user.IsSuspended = true;
                        await UserManager.UpdateAsync(user);

                    }

                    // If user is being lifted from being suspended - update user records
                    if (oldRole == "Suspended")
                    {

                        user.IsSuspended = false;
                        await UserManager.UpdateAsync(user);

                    }

                }   // If user is not a customer
                else if (userRole == "Staff" || userRole == "Manager" || userRole == "Admin" )
                {

                    // Creating employee object from returned id
                    Employee user = (Employee)await UserManager.FindByIdAsync(id);
                    string oldRole = (await UserManager.GetRolesAsync(id)).Single();

                    // If new role is the same as current role
                    if (oldRole == model.Role)
                    {

                        ViewBag.Error = "[X] Error: Current role and new role are the same";

                    }

                    // If attempting to suspend employees
                    if (model.Role == "Suspended")
                    {

                        ViewBag.Error = "[X] Error: Employees cannot be suspended";

                    }

                    // Remove old role
                    await UserManager.RemoveFromRoleAsync(id, oldRole);

                    // Add new role
                    await UserManager.AddToRoleAsync(id, model.Role);

                }   // End of IF (User.IsInRole("Customer"))

                return RedirectToAction("Users", "Admin");

            }   // End of IF (ModelState.IsValid)

            return View(model);

        }   // End of TASK<ACTIONRESULT> ChangeRoleConfirmed() [POST]


        // Delete user Section ----------------------------------------------------------------------------------------


        public async Task<ActionResult> Delete(string id)
        {

            // Error prevention
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            // Create employee list to find employee
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

            // Check if deleting own account
            if (id == employee.Id)
            {

                ViewBag.Error = "[X] Error: Users cannot delete their own accounts";
                return RedirectToAction("Users", "Admin");

            }

            // Creating object from return id
            User user = await UserManager.FindByIdAsync(id);

            // Error prevention
            if (user == null)
            {

                ViewBag.Error = "[DB NULL] Error: No user found with matching ID";

            }

            if (user.CurrentRole == "Customer" || user.CurrentRole == "Suspended")
            {

                // If Customer - delete associated comments
                await DeleteCustomer(user);

            }
            else if (user.CurrentRole == "Admin" || user.CurrentRole == "Manager" || user.CurrentRole == "Moderator")
            {

                // If employee - delete blogs and comments associated with each blog
                await DeleteEmployee(user);

            }
            else
            {

                // Removes user from DB - new roles
                await UserManager.DeleteAsync(user);

                return RedirectToAction("Users", "Admin");

            }   // End of IF (user == null)

            return RedirectToAction("Users", "Admin");

        }   // End of TASK<ACTIONRESULT> Delete()

        public async Task<ActionResult> DeleteCustomer(User user)
        {

                // Extracting customer comments from id matching
                var customer_comments = await db.Comments.Where(p => p.Customer.Id == user.Id).ToListAsync();

                foreach (var comment in customer_comments)
                {

                    // Removing each comment
                    db.Comments.Remove(comment);

                }

            db.SaveChanges();

            // Removes user from DB
            await UserManager.DeleteAsync(user);

            return RedirectToAction("Users", "Admin");

        }   // Emd of TASK<ACTIONRESULT> DeleteCustomer()

        public async Task<ActionResult> DeleteEmployee(User user)
        {

            // Extracting customer comments from id matching
            var employee_posts = db.Blogs.Where(p => p.Employee.Id == user.Id).ToList();

            foreach (var blog in employee_posts)
            {

                // Finds comments associated with deleted blog
                var comments = db.Comments.Where(p => p.BlogId == blog.BlogID).ToList();

                // Removing each comment associated with deleted blog
                foreach (var comment in comments)
                {

                    db.Comments.Remove(comment);

                }

                // Removing blog
                db.Blogs.Remove(blog);

            }   // End of FOREACH (var blog in employee_posts)

            db.SaveChanges();

            // Removes user from DB
            await UserManager.DeleteAsync(user);

            return RedirectToAction("Users", "Admin");

        }   // Emd of TASK<ACTIONRESULT> DeleteCustomer()

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