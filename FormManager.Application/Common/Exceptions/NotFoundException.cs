﻿using FormManager.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormManager.Application.Common.Exceptions
{
    public class NotFoundException : Exception, IBaseException
    {
        public NotFoundException() : base() {}

        public NotFoundException(string message, int status, List<Error> errors) : base(message)
        {
            this.Status = status;
            this.Errors = errors;
        }

        public NotFoundException(string message, int status) : base(message)
        {
            this.Status = status;
        }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        public List<Error> Errors { get; set; } = new List<Error>();
        public int Status { get; set; } = 400;
    }
}
