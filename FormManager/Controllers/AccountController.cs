using FormManager.Api.Responses;
using FormManager.Application.Users.Commands;
using FormManager.Application.Users.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FormManager.Api.Controllers
{
    public class AccountController : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<TokenResponse>> GetToken([FromBody] AuthenticateCommand login)
        {
            return await Mediator.Send(login);
        }

        [HttpGet("userinfo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<UserInfo> UserInfo()
        {
            var claims = new UserInfo
            {
                Sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub),
                Aud = User.FindFirstValue(JwtRegisteredClaimNames.Aud),
                Iss = User.FindFirstValue(JwtRegisteredClaimNames.Iss),
                Exp = User.FindFirstValue(JwtRegisteredClaimNames.Exp),
                Email = User.FindFirstValue(ClaimTypes.Email),
                NameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Roles = User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).ToArray(),
            };

            //Dictionary<string, string[]> claimList = User.Claims.GroupBy(x => x.Type.ToString())
            //                                .Select(g => (key: g.Key, values: g.Select(k => k.Value).ToArray()))
            //                                .ToDictionary(k => k.key, k => k.values);
                     
            return claims;
        }

        [HttpPost("createuser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> CreateUserAsync(CreateUserCommand register)
        {
            return Ok(await Mediator.Send(register));
        }
    }
}

