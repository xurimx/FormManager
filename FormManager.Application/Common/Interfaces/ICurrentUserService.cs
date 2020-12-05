using System;
using System.Collections.Generic;
using System.Text;

namespace FormManager.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        IDictionary<string, string[]> Claims { get; }
    }
}
