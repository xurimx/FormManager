using FormManager.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormManager.Application.Common.Exceptions
{
    public class AuthenticateException : Exception, IBaseException
    {
        public AuthenticateException() : base() { }

        public AuthenticateException(string message, int status, List<Error> errors) : base(message)
        {
            this.Status = status;
            this.Errors = errors;
        }

        public AuthenticateException(string message, int status) : base(message)
        {
            this.Status = status;
        }

        public AuthenticateException(string message) : base(message) { }

        public AuthenticateException(string message, Exception innerException)
            : base(message, innerException) { }

        public List<Error> Errors { get; set; } = new List<Error>();
        public int Status { get; set; } = 400;
    }
}
