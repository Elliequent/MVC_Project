using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Local_Theatre_Company_V1._0.Models
{

    // Ian Fraser - 10/04/2021 - Local_Theatre_Company_V1.0
    
    public class DatabaseInitialiser : DropCreateDatabaseAlways<LTCDbContext>
    {

        protected override void Seed(LTCDbContext context)
        {

            base.Seed(context);

            // If database is empty
            if (!context.Users.Any())
            {

                // Identity Roles ----------------------------------------------------------------------------

                // Creating role manager to create and store roles
                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                if (!roleManager.RoleExists("Admin"))
                {

                    roleManager.Create(new IdentityRole("Admin"));

                }

                if (!roleManager.RoleExists("Staff"))
                {

                    roleManager.Create(new IdentityRole("Staff"));

                }

                if (!roleManager.RoleExists("Manager"))
                {

                    roleManager.Create(new IdentityRole("Manager"));

                }

                if (!roleManager.RoleExists("Customer"))
                {

                    roleManager.Create(new IdentityRole("Customer"));

                }

                // Role for suspended users
                if (!roleManager.RoleExists("Suspended"))
                {

                    roleManager.Create(new IdentityRole("Suspended"));

                }

                // Role for Moderator users
                if (!roleManager.RoleExists("Moderator"))
                {

                    roleManager.Create(new IdentityRole("Moderator"));

                }

                context.SaveChanges();

                // Creating Seed users --------------------------------------------------------------------

                // Creating user manager to create and store users
                UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context));

                // Adding Administrator
                if (userManager.FindByName("admin@LTC.com") == null)
                {

                    userManager.PasswordValidator = new PasswordValidator
                    {

                        RequireDigit = false,
                        RequiredLength = 1,
                        RequireLowercase = false,
                        RequireNonLetterOrDigit = false,
                        RequireUppercase = false

                    };

                    var administrator = new Employee
                    {

                        UserName = "admin@LTC.com",
                        Email = "admin@LTC.com",
                        FirstName = "Admin",
                        LastName = "User",
                        Address1 = "123 Test Street",
                        Address2 = "Test Town",
                        City = "Test City",
                        PostCode = "T35 5TS",
                        RegisteredAt = DateTime.Now.AddYears(-5),
                        EmailConfirmed = true,
                        IsActive = true,
                        EmploymentStatus = EmploymentStatus.FullTime

                    };

                    // Creating Administrator and assiging role
                    userManager.Create(administrator, "admin123");
                    userManager.AddToRole(administrator.Id, "Admin");

                }   // End of IF (userManager.FindByName("admin@jewellerystore.com") == null)

                // Adding Manager
                if (userManager.FindByName("Test1@LTC.com") == null)
                {

                    var Test1 = new Employee
                    {

                        UserName = "Test1@LTC.com",
                        Email = "Test1@LTC.com",
                        FirstName = "Manager",
                        LastName = "Staff",
                        Address1 = "123 Test Street",
                        Address2 = "Test Town",
                        City = "Test City",
                        PostCode = "T35 5TS",
                        RegisteredAt = DateTime.Now.AddYears(-2),
                        EmailConfirmed = true,
                        IsActive = true,
                        EmploymentStatus = EmploymentStatus.FullTime

                    };

                    // Creating Manager and assiging role
                    userManager.Create(Test1, "password1");
                    userManager.AddToRole(Test1.Id, "Manager");

                }   // End of IF (userManager.FindByName("jeff@jewellerystore.com") == null)

                // Adding Staff
                if (userManager.FindByName("Test2@LTC.com") == null)
                {

                    var Test2 = new Employee
                    {

                        UserName = "Test2@LTC.com",
                        Email = "Test2@LTC.com",
                        FirstName = "Employee",
                        LastName = "User",
                        Address1 = "123 Test Street",
                        Address2 = "Test Town",
                        City = "Test City",
                        PostCode = "T35 5TS",
                        RegisteredAt = DateTime.Now.AddYears(-3),
                        EmailConfirmed = true,
                        IsActive = true,
                        EmploymentStatus = EmploymentStatus.PartTime

                    };

                    // Creating Manager and assiging role
                    userManager.Create(Test2, "password1");
                    userManager.AddToRole(Test2.Id, "Staff");

                }   // End of IF (userManager.FindByName("Xander@jewellerystore.com") == null)

                // Adding Moderator
                if (userManager.FindByName("Mod@LTC.com") == null)
                {

                    var Test3 = new Employee
                    {

                        UserName = "Mod@LTC.com",
                        Email = "Mod@LTC.com",
                        FirstName = "Mod",
                        LastName = "User",
                        Address1 = "123 Test Street",
                        Address2 = "Test Town",
                        City = "Test City",
                        PostCode = "T35 5TS",
                        RegisteredAt = DateTime.Now.AddYears(-10),
                        EmailConfirmed = true,
                        IsActive = true,
                        EmploymentStatus = EmploymentStatus.FullTime

                    };

                    // Creating Manager and assiging role
                    userManager.Create(Test3, "password1");
                    userManager.AddToRole(Test3.Id, "Moderator");

                }   // End of IF (userManager.FindByName("Xander@jewellerystore.com") == null)

                // Adding Customers
                if (userManager.FindByName("Customer1@gmail.com") == null)
                {

                    var Customer1 = new Customer
                    {

                        UserName = "Customer1@gmail.com",
                        Email = "Customer1@gmail.com",
                        FirstName = "James",
                        LastName = "Adams",
                        Address1 = "123 Test Street",
                        Address2 = "Test Town",
                        City = "Test City",
                        PostCode = "T35 5TS",
                        RegisteredAt = DateTime.Now.AddMonths(-5),
                        EmailConfirmed = true,
                        IsActive = true,
                        IsSuspended = false

                    };

                    // Creating Manager and assiging role
                    userManager.Create(Customer1, "password1");
                    userManager.AddToRole(Customer1.Id, "Customer");

                }   // End of IF (userManager.FindByName("bill@gmail.com") == null)

                if (userManager.FindByName("Customer2@gmail.com") == null)
                {

                    var Customer2 = new Customer
                    {

                        UserName = "Customer2@gmail.com",
                        Email = "Customer2@gmail.com",
                        FirstName = "Alex",
                        LastName = "Smith",
                        Address1 = "123 Test Street",
                        Address2 = "Test Town",
                        City = "Test City",
                        PostCode = "T35 5TS",
                        RegisteredAt = DateTime.Now.AddYears(-1),
                        EmailConfirmed = true,
                        IsActive = true,
                        IsSuspended = false

                    };

                    // Creating Manager and assiging role
                    userManager.Create(Customer2, "password1");
                    userManager.AddToRole(Customer2.Id, "Customer");

                }   // End of IF (userManager.FindByName("bob@gmail.com") == null)

                if (userManager.FindByName("Customer3@gmail.com") == null)
                {

                    var Customer3 = new Customer
                    {

                        UserName = "Customer3@gmail.com",
                        Email = "Customer3@gmail.com",
                        FirstName = "Sarah",
                        LastName = "Jamieson",
                        Address1 = "123 Test Street",
                        Address2 = "Test Town",
                        City = "Test City",
                        PostCode = "T35 5TS",
                        RegisteredAt = DateTime.Now.AddMonths(-10),
                        EmailConfirmed = true,
                        IsActive = true,
                        IsSuspended = false

                    };

                    // Creating Manager and assiging role
                    userManager.Create(Customer3, "password1");
                    userManager.AddToRole(Customer3.Id, "Customer");

                }   // End of IF (userManager.FindByName("steveb@gmail.com") == null)

                if (userManager.FindByName("Customer4@gmail.com") == null)
                {

                    var Customer4 = new Customer
                    {

                        UserName = "Customer4@gmail.com",
                        Email = "Customer4@gmail.com",
                        FirstName = "Kelly",
                        LastName = "Jackson",
                        Address1 = "123 Test Street",
                        Address2 = "Test Town",
                        City = "Test City",
                        PostCode = "T35 5TS",
                        RegisteredAt = DateTime.Now,
                        EmailConfirmed = true,
                        IsActive = true,
                        IsSuspended = false

                    };

                    // Creating Manager and assiging role
                    userManager.Create(Customer4, "password1");
                    userManager.AddToRole(Customer4.Id, "Customer");

                }   // End of IF (userManager.FindByName("gary@gmail.com") == null)

                // Adding Suspended Customer
                if (userManager.FindByName("Customer5@gmail.com") == null)
                {

                    var Customer5 = new Customer
                    {

                        UserName = "Customer5@gmail.com",
                        Email = "Customer5@gmail.com",
                        FirstName = "Suspended",
                        LastName = "User",
                        Address1 = "123 Test Street",
                        Address2 = "Test Town",
                        City = "Test City",
                        PostCode = "T35 5TS",
                        RegisteredAt = DateTime.Now.AddDays(-28),
                        EmailConfirmed = true,
                        IsActive = true,
                        IsSuspended = true

                    };

                    // Creating Manager and assiging role
                    userManager.Create(Customer5, "password1");
                    userManager.AddToRole(Customer5.Id, "Suspended");

                }   // End of IF (userManager.FindByName("bill@gmail.com") == null)

                context.SaveChanges();


                // Creating testing seed plays
                var play1 = new Play
                {

                    PlayId = 1,
                    PlayName = "Mary Poppins",
                    PlayStart = DateTime.Now.AddMonths(-4),
                    PlayEnd = DateTime.Now.AddMonths(-3),
                    PlayPoster = "/Images/Plays/Mary_Poppins.jpg",
                    PlayType = PlayType.Pantomime,
                    PlayDescription = "When Jane and Michael, the children of the wealthy and uptight Banks family, are faced with the prospect of a new nanny, they are pleasantly surprised by the arrival of the magical Mary Poppins. Embarking on a series of fantastical adventures with Mary and her Cockney performer friend, Bert, the siblings try to pass on some of their nanny's sunny attitude to their preoccupied parents."

                };

                var play2 = new Play
                {

                    PlayId = 2,
                    PlayName = "Romeo and Juliet",
                    PlayStart = DateTime.Now.AddMonths(-3),
                    PlayEnd = DateTime.Now.AddMonths(-2),
                    PlayPoster = "/Images/Plays/Romeo_And_Juliet.jpg",
                    PlayType = PlayType.Romance,
                    PlayDescription = "Baz Luhrmann helped adapt this classic Shakespearean romantic tragedy for the screen, updating the setting to a post-modern city named Verona Beach. In this version, the Capulets and the Montagues are two rival gangs. Juliet is attending a costume ball thrown by her parents. Her father Fulgencio Capulet has arranged her marriage to the boorish Paris as part of a strategic investment plan. Romeo attends the masked ball and he and Juliet fall in love."

                };

                var play3 = new Play
                {

                    PlayId = 3,
                    PlayName = "Alice In Wonderland",
                    PlayStart = DateTime.Now.AddMonths(-2),
                    PlayEnd = DateTime.Now.AddDays(-2),
                    PlayPoster = "/Images/Plays/Alice_In_Wonderland.png",
                    PlayType = PlayType.Comdey,
                    PlayDescription = "A young girl when she first visited magical Underland, Alice Kingsleigh is now a teenager with no memory of the place -- except in her dreams. Her life takes a turn for the unexpected when, at a garden party for her fiance and herself, she spots a certain white rabbit and tumbles down a hole after him. Reunited with her friends the Mad Hatter, the Cheshire Cat and others, Alice learns it is her destiny to end the Red Queen's reign of terror."

                };

                var play4 = new Play
                {

                    PlayId = 4,
                    PlayName = "Phantom Of The Opera",
                    PlayStart = DateTime.Now.AddDays(-1),
                    PlayEnd = DateTime.Now.AddMonths(+1),
                    PlayPoster = "/Images/Plays/Phantom_Of_Opera.jpg",
                    PlayType = PlayType.Musical,
                    PlayDescription = "From his hideout beneath a 19th century Paris opera house, the brooding Phantom schemes to get closer to vocalist Christine Daae. The Phantom, wearing a mask to hide a congenital disfigurement, strong-arms management into giving the budding starlet key roles, but Christine instead falls for arts benefactor Raoul. Terrified at the notion of her absence, the Phantom enacts a plan to keep Christine by his side, while Raoul tries to foil the scheme."

                };

                context.Plays.Add(play1);
                context.Plays.Add(play2);
                context.Plays.Add(play3);
                context.Plays.Add(play4);

                context.SaveChanges();

                // Creating testing seed Catagories
                var catagory1 = new Catagory { Name = "Announcement" };
                var catagory2 = new Catagory { Name = "New Play" };
                var catagory3 = new Catagory { Name = "Alert" };
                var catagory4 = new Catagory { Name = "Discount" };
                var catagory5 = new Catagory { Name = "Holiday" };

                context.Catagories.Add(catagory1);
                context.Catagories.Add(catagory2);
                context.Catagories.Add(catagory3);
                context.Catagories.Add(catagory4);
                context.Catagories.Add(catagory5);

                context.SaveChanges();

                // Creating testing seed Blogs
                var blog1 = new Blog
                {

                    BlogID = 0001,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam erat odio, porta id eros sed, blandit feugiat tortor. Vestibulum cursus quam id auctor congue. Proin efficitur id leo id feugiat. In a mi nec mauris fermentum scelerisque ut aliquet dui. Sed tincidunt ipsum et sollicitudin maximus. Cras vehicula erat orci, sed consectetur lectus dictum nec. Mauris pretium at enim non laoreet.",
                    Title = "New Blog Feature for Local Theatre Company",
                    Employee = (Employee)userManager.FindByName("Test1@LTC.com"),
                    PostedOn = DateTime.Now.AddDays(-60),
                    Published = true,
                    BlogImage = "/Images/blog.jpg",
                    Catagory = catagory1


                };

                var blog2 = new Blog
                {

                    BlogID = 0002,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam erat odio, porta id eros sed, blandit feugiat tortor. Vestibulum cursus quam id auctor congue. Proin efficitur id leo id feugiat. In a mi nec mauris fermentum scelerisque ut aliquet dui. Sed tincidunt ipsum et sollicitudin maximus. Cras vehicula erat orci, sed consectetur lectus dictum nec. Mauris pretium at enim non laoreet.",
                    Title = "Blog 2, electric bogaloo!",
                    Employee = (Employee)userManager.FindByName("admin@LTC.com"),
                    PostedOn = DateTime.Now.AddDays(-50),
                    Published = true,
                    BlogImage = "/Images/blog2.jpg",
                    Catagory = catagory3

                };

                var blog3 = new Blog
                {

                    BlogID = 0003,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam erat odio, porta id eros sed, blandit feugiat tortor. Vestibulum cursus quam id auctor congue. Proin efficitur id leo id feugiat. In a mi nec mauris fermentum scelerisque ut aliquet dui. Sed tincidunt ipsum et sollicitudin maximus. Cras vehicula erat orci, sed consectetur lectus dictum nec. Mauris pretium at enim non laoreet.",
                    Title = "Third Blog, updates coming!",
                    Employee = (Employee)userManager.FindByName("Test2@LTC.com"),
                    PostedOn = DateTime.Now.AddDays(-40),
                    Published = false,
                    BlogImage = "/Images/blog3.jpg",
                    Catagory = catagory5


                };

                var blog4 = new Blog
                {

                    BlogID = 0004,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam erat odio, porta id eros sed, blandit feugiat tortor. Vestibulum cursus quam id auctor congue. Proin efficitur id leo id feugiat. In a mi nec mauris fermentum scelerisque ut aliquet dui. Sed tincidunt ipsum et sollicitudin maximus. Cras vehicula erat orci, sed consectetur lectus dictum nec. Mauris pretium at enim non laoreet.",
                    Title = "Fourth Blog Test",
                    Employee = (Employee)userManager.FindByName("Test2@LTC.com"),
                    PostedOn = DateTime.Now.AddDays(-30),
                    Published = true,
                    BlogImage = "/Images/blog4.jpg",
                    Catagory = catagory3


                };

                var blog5 = new Blog
                {

                    BlogID = 0005,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam erat odio, porta id eros sed, blandit feugiat tortor. Vestibulum cursus quam id auctor congue. Proin efficitur id leo id feugiat. In a mi nec mauris fermentum scelerisque ut aliquet dui. Sed tincidunt ipsum et sollicitudin maximus. Cras vehicula erat orci, sed consectetur lectus dictum nec. Mauris pretium at enim non laoreet.",
                    Title = "Blog Test No5",
                    Employee = (Employee)userManager.FindByName("admin@LTC.com"),
                    PostedOn = DateTime.Now.AddDays(-20),
                    Published = true,
                    BlogImage = "/Images/blog5.jpg",
                    Catagory = catagory2


                };

                var blog6 = new Blog
                {

                    BlogID = 0006,
                    Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam erat odio, porta id eros sed, blandit feugiat tortor. Vestibulum cursus quam id auctor congue. Proin efficitur id leo id feugiat. In a mi nec mauris fermentum scelerisque ut aliquet dui. Sed tincidunt ipsum et sollicitudin maximus. Cras vehicula erat orci, sed consectetur lectus dictum nec. Mauris pretium at enim non laoreet.",
                    Title = "Blog Test Number six!",
                    Employee = (Employee)userManager.FindByName("Test1@LTC.com"),
                    PostedOn = DateTime.Now.AddDays(-10),
                    Published = true,
                    BlogImage = "/Images/blog6.jpg",
                    Catagory = catagory4


                };

                context.Blogs.Add(blog1);
                context.Blogs.Add(blog2);
                context.Blogs.Add(blog3);
                context.Blogs.Add(blog4);
                context.Blogs.Add(blog5);
                context.Blogs.Add(blog6);

                context.SaveChanges();


                // Creating testing seed Comments
                var comment1 = new Comment
                {

                    CommentID = 0001,
                    BlogId = 0001,
                    Body = "Really excited for the new blog feature!",
                    PostedOn = DateTime.Now.AddDays(-3),
                    Customer = (Customer)userManager.FindByName("Customer1@gmail.com"),
                    CommentBlocked = false

                };

                var comment2 = new Comment
                {

                    CommentID = 002,
                    BlogId = 0001,
                    Body = "Cool blog feature!",
                    PostedOn = DateTime.Now.AddDays(-2),
                    Customer = (Customer)userManager.FindByName("Customer2@gmail.com"),
                    CommentBlocked = false

                };

                var comment3 = new Comment
                {

                    CommentID = 0003,
                    BlogId = 0001,
                    Body = "A blog? Awesome, I can leave a review for up and coming shows!",
                    PostedOn = DateTime.Now.AddDays(-2),
                    Customer = (Customer)userManager.FindByName("Customer3@gmail.com"),
                    CommentBlocked = false

                };

                var comment4 = new Comment
                {

                    CommentID = 0004,
                    BlogId = 0002,
                    Body = "Hey LTC! Attended Romeo and Juliet a while back, place looks great!",
                    PostedOn = DateTime.Now.AddDays(-2),
                    Customer = (Customer)userManager.FindByName("Customer1@gmail.com"),
                    CommentBlocked = false

                };

                var comment5 = new Comment
                {

                    CommentID = 0005,
                    BlogId = 0002,
                    Body = "LTC is where I got my acting break, holds a special place in my heart!",
                    PostedOn = DateTime.Now.AddDays(-1),
                    Customer = (Customer)userManager.FindByName("Customer4@gmail.com"),
                    CommentBlocked = false

                };

                var comment6 = new Comment
                {

                    CommentID = 0006,
                    BlogId = 0002,
                    Body = "This blog sucks!",
                    PostedOn = DateTime.Now,
                    Customer = (Customer)userManager.FindByName("Customer5@gmail.com"),
                    CommentBlocked = true

                };

                context.Comments.Add(comment1);
                context.Comments.Add(comment2);
                context.Comments.Add(comment3);
                context.Comments.Add(comment4);
                context.Comments.Add(comment5);
                context.Comments.Add(comment6);

                context.SaveChanges();


            }   // End of IF (!context.Users.Any())


        }   // End of METHOD Seed()


    }   // End of Class


}   // End of NameSpace