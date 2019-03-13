using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace WorkWithData.Models 
{
    internal class Context : ContextIdent
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarBrand> CarBrands { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<QualityClass> QualityClasses { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
