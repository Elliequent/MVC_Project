using Local_Theatre_Company_V1._0.Models;
using Local_Theatre_Company_V1._0.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Local_Theatre_Company_V1._0.Controllers
{

    // Ian Fraser - 31/05/2021 - Local_Theatre_CompanyV1.0

    /// <summary>
    /// User controller section
    /// Section handles the user profile
    /// Users should have the ability to change their own records
    /// Users should not be able to change the records of other users
    /// </summary>

    // Testing - PASS - 01/06/2021

    [Authorize(Roles = "Admin, Customer, Moderator, Staff, Manager, Suspended")]
    public class UserController : Controller
    {

        // Database connection
        LTCDbContext db = new LTCDbContext();

        public ActionResult Profile_Page()
        {

            // Check if user is customer or employee
            if (User.IsInRole("Customer") || User.IsInRole("Suspended"))
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

                // Collect users comments - filter: not blocked, in order of date
                ViewBag.Comments = db.Comments.Where(p => p.Customer.UserName == User.Identity.Name).Where(a => a.CommentBlocked == false).OrderBy(p => p.PostedOn).ToList();

                // Populates View details with EditUserViewModel
                return View(new EditUserViewModel { 
                
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Address1 = customer.Address1,
                    Address2 = customer.Address2,
                    City = customer.City,
                    PostCode = customer.PostCode,
                    RegisteredAt = customer.RegisteredAt
                
                });

            }
            else
            {

                // Create employee list to find employee to add to the comment
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

                // Populates View details with EditUserViewModel
                return View(new EditUserViewModel {

                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Address1 = employee.Address1,
                    Address2 = employee.Address2,
                    City = employee.City,
                    PostCode = employee.PostCode,
                    RegisteredAt = employee.RegisteredAt

                });

            }   // End of IF (User.IsInRole("Customer") || User.IsInRole("Suspended"))

        }   // End of ACTIONRESULT Profile_Page()

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profile_Page([Bind(Include = "FirstName, LastName, Address1, Address2, City, PostCode, RegisteredAt")] EditUserViewModel model)
        {

            // If User is customer
            if (User.IsInRole("Customer"))
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

                // Error prevention
                if ((model.FirstName == null || model.FirstName == "") || (model.LastName == null || model.LastName == "") || (model.Address1 == null || model.Address1 == "") || (model.City == null || model.City == "") || (model.PostCode == null || model.PostCode == ""))
                {

                    ViewBag.Error = "[X] Error: Please be sure all fields are not blank";

                }
                else
                {

                    if (Regex.IsMatch(model.FirstName, @"^\d+$") || Regex.IsMatch(model.LastName, @"^\d+$") || Regex.IsMatch(model.City, @"^\d+$"))
                    {

                        ViewBag.Error = "[X] Error: Please be sure all relevant fields are text";

                    }
                    else
                    {

                        // Customer records are updated with user input
                        customer.FirstName = model.FirstName;
                        customer.LastName = model.LastName;
                        customer.Address1 = model.Address1;
                        customer.Address2 = model.Address2;
                        customer.City = model.City;
                        customer.PostCode = model.PostCode;

                        // Updates Blog information in database with new changes
                        bool result = TryUpdateModel(customer);
                        db.SaveChanges();

                        // If Blog updated in database redirect to list - else error message
                        if (result)
                        {

                            return RedirectToAction("Profile_Page", "User");

                        }
                        else
                        {

                            ViewBag.Message = "[DB SAVE] Error: Profile not saved to database";

                        }

                    }   // End of IF (Regex.IsMatch(model.FirstName, @"^\d+$") ||...

                }   // End of IF ((model.FirstName == null || model.FirstName == "") ||...

            }
            else if (User.IsInRole("Suspended"))
            {

                ViewBag.Error = "[X] Error: Account cannot be updated while suspended";
                return RedirectToAction("Index", "Home");

            }
            else
            {

                // Create customer list to find cutomer to add to the comment
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

                // Error prevention
                if ((model.FirstName == null || model.FirstName == "") || (model.LastName == null || model.LastName == "") || (model.Address1 == null || model.Address1 == "") || (model.City == null || model.City == "") || (model.PostCode == null || model.PostCode == ""))
                {

                    ViewBag.Error = "[X] Error: Please be sure all fields are not blank";
                    return RedirectToAction("Profile_Page", "User");

                }
                else
                {

                    if (Regex.IsMatch(model.FirstName, @"^\d+$") || Regex.IsMatch(model.LastName, @"^\d+$") || Regex.IsMatch(model.City, @"^\d+$"))
                    {

                        ViewBag.Error = "[X] Error: Please be sure all relevant fields are text";
                        return RedirectToAction("Profile_Page", "User");

                    }
                    else
                    {

                        // Employee records are updated with user input
                        employee.FirstName = model.FirstName;
                        employee.LastName = model.LastName;
                        employee.Address1 = model.Address1;
                        employee.Address2 = model.Address2;
                        employee.City = model.City;
                        employee.PostCode = model.PostCode;

                        // Updates Blog information in database with new changes
                        bool result = TryUpdateModel(employee);
                        db.SaveChanges();

                        // If Blog updated in database redirect to list - else error message
                        if (result)
                        {

                            return RedirectToAction("Profile_Page", "User");

                        }
                        else
                        {

                            ViewBag.Message = "[DB SAVE] Error: Profile not saved to database";

                        }

                    }   // End of IF (Regex.IsMatch(model.FirstName, @"^\d+$") ||...

                }   // End of IF ((model.FirstName == null || model.FirstName == "") ||...

            }   // End of IF (User.IsInRole("Customer"))

            return View();

        }   // End of ACTIONRESULT EditBlog() [POST]


    }   // End of Class


}   // End of NameSpace