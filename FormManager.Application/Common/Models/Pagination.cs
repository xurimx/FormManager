using FormManager.Application.Forms.Queries;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FormManager.Application.Common.Models
{
    public class Pagination<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int FilteredItems { get; set; }
        public Navigation Navigation { get; set; }

        public void BuildNavigation(IRequestQuery request)
        {
            var queryParams = new Dictionary<string, string>
            {
                {"SearchInput", request.SearchInput },
                {"OrderBy", request.OrderBy },
                {"OrderDirection", request.OrderDirection },
                {"Limit", request.Limit.ToString() },
            };

            string urlParams = QueryHelpers.AddQueryString("", queryParams);
            foreach (var item in request.SearchColumns)
            {
                urlParams = $"{urlParams}&SearchColumns={item}";
            }
            urlParams += "&Page={0}";

            Navigation = new Navigation
            {
                FirstPage = string.Format(urlParams, 1),
                PreviousPage = Page > 1 ? string.Format(urlParams, Page-1): "",
                NextPage = Page < TotalPages ? string.Format(urlParams, Page+1): "",
                LastPage = string.Format(urlParams, TotalPages)
            };
        }
    }
    public class Navigation
    {
        public string FirstPage { get; set; }
        public string PreviousPage { get; set; }
        public string NextPage { get; set; }
        public string LastPage { get; set; }
    }
}
