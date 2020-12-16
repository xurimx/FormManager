using FormManager.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormManager.Api.Responses
{
    public class ErrorResponse
    {
        public ErrorResponse(string message, int status)
        {
            this.Description = message;
            this.Status = status;
        }

        public ErrorResponse(string message, int status, List<Error> errors)
        {
            this.Description = message;
            this.Status = status;
            this.Errors = errors;
        }
        public string Description { get; set; }
        public int Status { get; set; }

        public List<Error> Errors { get; set; } = new List<Error>();
    }
}
