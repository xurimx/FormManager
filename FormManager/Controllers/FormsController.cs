using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FormManager.Api.ViewModels;
using FormManager.Application.Forms.Queries;
using FormManager.Domain.Entities;
using FormManager.Application.Forms.Commands;
using FormManager.Application.Common.Models;
using FormManager.Api.Responses;
using FormManager.Application.Forms.Mappings.ViewModels;

namespace FormManager.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FormsController : BaseApiController
    {
        [HttpGet]
        [Authorize(Roles ="admin")]
        [ProducesResponseType(typeof(Pagination<FormVM>), 200)]
        public async Task<ActionResult<Pagination<FormVM>>> GetForms([FromQuery]GetFormsQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(FormVM), 200)]
        public async Task<ActionResult<FormVM>> GetForm(Guid id)
        {
            return await Mediator.Send(new GetFormByIdQuery(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateFormCommand), 201)]
        public async Task<IActionResult> PostForm([FromBody]CreateFormCommand form)
        {
            Guid guid = await Mediator.Send(form);
            return CreatedAtAction("GetForm", new { id = guid}, form);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteForm(Guid id)
        {
            bool deleted = await Mediator.Send(new DeleteFormCommand(id));
            return deleted ? Ok() : BadRequest();
        }
    }
}
