using FormManager.Application.Common.Models;
using FormManager.Application.Forms.Queries;
using FormManager.Application.Users.Commands;
using FormManager.Application.Users.Queries;
using FormManager.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormManager.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public class UsersController : BaseApiController
    {
        [HttpGet]
        public async Task<Pagination<User>> GetUsers([FromQuery] GetUsersQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("{id}")]
        public async Task<User> GetUser(string id)
        {
            return await Mediator.Send(new GetUserByIdQuery(id));
        }

        [HttpGet("{id}/forms")]
        public async Task<Pagination<Form>> GetUserForms(string id)
        {
            return await Mediator.Send(new GetFormsQuery { SearchInput = id, SearchColumns = new[] { "senderid" } });
        }

        [HttpPost("createuser")]
        public async Task<IActionResult> CreateUserAsync(CreateUserCommand register)
        {
            string id = await Mediator.Send(register);
            return base.CreatedAtAction("GetUser", new { id }, register);
        }
    }
}
