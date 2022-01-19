using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrzykladowyProjektWebApi2.Models
{
    public class PageResult<T> // żeby można było dostosować obiekt listy
    {
        public List<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int ItemFrom { get; set; }
        public int ItemsTo { get; set; }
        public int TotalItemsCount { get; set; }

        public PageResult(List<T> items, int totalItemsCount, int pageSize, int pageNumber)
        {
            Items = items;
            TotalItemsCount = totalItemsCount;
            ItemFrom = pageSize * (pageNumber - 1) + 1;
            ItemsTo = ItemFrom + pageSize - 1;
            TotalPages = (int)Math.Ceiling(totalItemsCount /(double) pageSize);
        }

    }

}
