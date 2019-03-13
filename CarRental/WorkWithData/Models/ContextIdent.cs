using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

namespace WorkWithData.Models
{
    public class ContextIdent : IdentityDbContext<ApplicationUser>
    {
        public ContextIdent() : base("Context") { }
        public static ContextIdent Create()
        {
            return new ContextIdent();
        }
    }
}
