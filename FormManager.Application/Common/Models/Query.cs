using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormManager.Application.Common.Models
{
    public interface IRequestQuery
    {
        string? SearchInput { get; set; } 
        string? OrderBy { get; set; }
        string? OrderDirection { get; set; }
        int? Page { get; set; }
        int? Limit { get; set; }
        string[] SearchColumns { get; set; }
    }

    public abstract class RequestQuery<T> : IRequest<T>, IRequestQuery
    {
        public string? SearchInput { get; set; }
        public string? OrderBy { get; set; }
        public string? OrderDirection { get; set; }
        public int? Page { get; set; } = 1;
        public int? Limit { get; set; } = 10;
        public string[] SearchColumns { get; set; } = new string[0];
    }
}
