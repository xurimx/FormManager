using System;
using System.Collections.Generic;
using FormManager.Application.Common.Models;

namespace FormManager.Application.Common.Exceptions
{
    public class AuthenticateException : Exception
    {
        public AuthenticateException(string message, int status, List<Error> errors) : base(message)
        {
            Status = status;
            Errors = errors;
        }

        public AuthenticateException(string message, int status) : base(message)
        {
            Status = status;
        }

        public AuthenticateException(string message) : base(message) { }

        public AuthenticateException(string message, Exception innerException)
            : base(message, innerException) { }

        public List<Error> Errors { get; set; } = new List<Error>();
        public int Status { get; set; } = 400;
    }
}
