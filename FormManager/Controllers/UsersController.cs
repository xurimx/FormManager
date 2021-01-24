using FormManager.Api.Responses;
using FormManager.Application.Common.Models;
using FormManager.Application.Forms.Mappings.ViewModels;
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
        [ProducesResponseType(typeof(Pagination<User>), 200)]
        public async Task<Pagination<User>> GetUsers([FromQuery] GetUsersQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), 200)]
        public async Task<User> GetUser(string id)
        {
            return await Mediator.Send(new GetUserByIdQuery(id));
        }

        [HttpGet("{id}/forms")]
        [ProducesResponseType(typeof(Pagination<FormVM>), 200)]
        public async Task<Pagination<FormVM>> GetUserForms(string id)
        {
            return await Mediator.Send(new GetFormsQuery { SearchInput = id, SearchColumns = new[] { "senderid" } });
        }

        [HttpPost("createuser")]
        [ProducesResponseType(typeof(CreateUserCommand), 200)]
        public async Task<IActionResult> CreateUserAsync(CreateUserCommand register)
        {
            string id = await Mediator.Send(register);
            register.Password = null;
            return base.CreatedAtAction("GetUser", new { id }, register);
        }
    }
}
