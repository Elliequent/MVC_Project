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

    public class LTCDbContext : IdentityDbContext<User>
    {

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Play> Plays { get; set; }
        public DbSet<Catagory> Catagories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }


        public LTCDbContext() : base("Local_Theatre_Connection", throwIfV1Schema: false)
        {

            Database.SetInitializer(new DatabaseInitialiser());

        }

        public static LTCDbContext Create()
        {

            return new LTCDbContext();

        }


    }   // End of Class


}   // End of NameSpace