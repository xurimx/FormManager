using System;
using System.Collections.Generic;
using System.Text;

namespace FormManager.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base() {}

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
