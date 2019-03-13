using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using WorkWithData.Models;
using Microsoft.AspNet.Identity;

namespace WorkWithData.AccessToData
{
    public class WorkWithUsers
    {
        public static List<ApplicationUser> GetUsers()
        {
            ContextIdent contextIdent = new ContextIdent();
            var users = contextIdent.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains("1")).ToList(); // 1 - Role user
            return users;
        }

        public static ApplicationUser GetApplicationUser(string Id)
        {
            ContextIdent contextIdent = new ContextIdent();
            ApplicationUser applicationUser = contextIdent.Users.Find(Id);
            return applicationUser;
        }

        public static void BlockUser(string Id)
        {
            ContextIdent contextIdent = new ContextIdent();
            ApplicationUser applicationUser = contextIdent.Users.Find(Id);
            applicationUser.IsBlocked = true;
            contextIdent.Entry(applicationUser).State = EntityState.Modified;
            contextIdent.SaveChanges();
        }

        public static void AnBlockUser(string Id)
        {
            ContextIdent contextIdent = new ContextIdent();
            ApplicationUser applicationUser = contextIdent.Users.Find(Id);
            applicationUser.IsBlocked = false;
            contextIdent.Entry(applicationUser).State = EntityState.Modified;
            contextIdent.SaveChanges();
        }
    }
}
