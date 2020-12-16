using FormManager.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormManager.Application.Common.Exceptions
{
    public interface IBaseException
    {
        List<Error> Errors { get; set; }
        int Status { get; set; }
    }
}
