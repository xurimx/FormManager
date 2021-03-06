﻿using FormManager.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormManager.Application.Common.Exceptions
{
    public class FormMgrException : Exception
    {
        public FormMgrException(string message, int status, List<Error> errors) : base(message)
        {
            Status = status;
            Errors = errors;
        }

        public FormMgrException(string message, int status) : base(message)
        {
            Status = status;
        }

        public FormMgrException(string message) : base(message) { }

        public FormMgrException(string message, Exception innerException)
            : base(message, innerException) { }

        public List<Error> Errors { get; set; } = new List<Error>();
        public int Status { get; set; } = 400;
    }
}
