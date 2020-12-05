using System;
using System.Collections.Generic;
using System.Text;

namespace FormManager.Application.Common.Exceptions
{
    public class UserExistsException : Exception
    {
        public UserExistsException() : base() { }

        public UserExistsException(string message) : base(message) { }

        public UserExistsException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
