using FormManager.Api.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator mediator;
        protected IMediator Mediator => mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    }
}
