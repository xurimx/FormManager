using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormManager.Api.Responses
{
    public class ErrorResponse
    {
        public ErrorResponse(string Title, string Description, int Status)
        {
            this.Title = Title;
            this.Description = Description;
            this.Status = Status;
        }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }
}
