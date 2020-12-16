using FormManager.Api.Responses;
using FormManager.Application.Config.Commands;
using FormManager.Application.Config.Queries;
using FormManager.Application.Config.ViewModels;
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
    public class ConfigurationController : BaseApiController
    {
        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<ActionResult> SetConfigurationAsync(UpdateConfigurationCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(SmtpConfiguration),200)]
        public async Task<ActionResult<SmtpConfiguration>> GetConfigurationAsync()
        {
            return await Mediator.Send(new GetSmtpConfigurationQuery());
        }
    }
}
