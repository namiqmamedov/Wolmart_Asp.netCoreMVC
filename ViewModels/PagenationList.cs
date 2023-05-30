using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Wolmart.Ecommerce.ViewModels
{
    public class PagenationList<T> : List<T>
    {
        public PagenationList(IQueryable<T> query, int page, int pagecount, int itemcount)
        {
            Page = page;
            PageCount = pagecount;
            ItemCount = itemcount;
            HasNext = page < pagecount;
            HasPrev = page > 1;
        }

        public int Page { get;}
        public int PageCount { get; }
        public int ItemCount { get; }
        public bool HasNext { get; }
        public bool HasPrev { get;  }

        public static PagenationList<T> Create(IQueryable<T> query, int page, int itemCount)
        {
              int pageCount = (int)Math.Ceiling((decimal)query.Count() / itemCount);

            return new PagenationList<T>(query,page,pageCount,itemCount);
        }
    }
}
