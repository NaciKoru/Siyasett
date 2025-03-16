using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Siyasett.Web.Models
{
    public class PagerInfo
    {

        public Dictionary<string, string> Query { get; set; }
        public int PageSize { get; set; } = 30;
        public int CurrentPage { get; set; } = 1;
        public int Total { get; set; } = 0;
        public int SortFieldIndex { get; set; }
        public string SearchKeyword { get; set; }
        /// <summary>
        /// sayfa numarası için page kullan
        /// </summary>
        /// <param name="col"></param>
        public void SetQuery(IQueryCollection col)
        {
            foreach (var item in col.Keys)
            {
                if (item != null)
                    Query.Add(item, col[item]);
            }
            if (Query.ContainsKey("page"))
                Query.Remove("page");
        }
        public PagerInfo()
        {
            Query = new Dictionary<string, string>();
            
        }
        public int PageCount
        {
            get
            {
                int c = (int)Math.Ceiling((Total * 1.0) / (PageSize * 1.0));
                if (c < 1)
                    c = 1;

                return c;
            }
        }


    }

}
