using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace WorkWithData.Models
{
    public class CarsListViewModel
    {
        public IList<Car> Cars { get; set; }
        public SelectList QualityClas { get; set; }
        public SelectList CarBrand { get; set; }
        public PageInfo PageInfo { get; set; } 
    }
}
