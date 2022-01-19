using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Models
{
    public class RestaurantQuery
    {
        public string SearchPhrase { get; set; }
        public int PageNumer { get; set; }
        public int PageSize { get; set; }

        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }


    }
    public enum SortDirection
    {
        ASC,
        DESC
    }

}
