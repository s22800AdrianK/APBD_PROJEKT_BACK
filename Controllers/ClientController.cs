using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekt.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _service;

        public ClientController(IClientService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Models.DTOs.Client client)
        {
            if (await _service.IsLogginUnique(client.Login)) return BadRequest("Login jest juz zajety");
            if (!_service.CheckPasswordLength(client.Password)) return BadRequest("Haslo za krótkie");
            await _service.RegisterNewClient(client);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Models.DTOs.Client client)
        {
            if (!await _service.IsLogginUnique(client.Login)) return BadRequest("nie ma takiego loginu");
            if (!await _service.DoesPasswordMatch(client)) return BadRequest("Haslo sie nie zgadza");
            var c = await _service.Login(client);
            var option = _service.GetOptions(c.IdUser);
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(option), c.RefreshToken, Id = c.IdUser });
        }

        [HttpPost("refreash")]
        public async Task<IActionResult> Refreash([FromBody] Models.DTOs.RefreashToken refreashToken)
        {
            if (!await _service.DoesRefreashTokenMatch(refreashToken.token)) return BadRequest("zly token");
            var c = await _service.GetRefrashedToken(refreashToken.token);
            var option = _service.GetOptions(c.IdUser);
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(option), c.RefreshToken, Id = c.IdUser });
        }

    }
}
