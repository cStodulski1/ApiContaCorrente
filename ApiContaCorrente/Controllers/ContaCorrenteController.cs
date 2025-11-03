using ApiContaCorrente.ContasCorrentes.Commands.Requests;
using ApiContaCorrente.ContasCorrentes.Queries.Requests;
using ApiContaCorrente.Helpers;
using ApiContaCorrente.Models.Enums;
using ApiContaCorrente.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ApiContaCorrente.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ContaCorrenteController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Cadastrar(
            [FromBody]CriarContaCorrenteRequest command,
            [FromHeader(Name = "X-Idempotency-Key")] string requestId)
        {
            if(!Guid.TryParse(requestId, out Guid requestIdParsed)) {
                return BadRequest("X-Idempotency-Key header is missing or invalid.");
            }

            command.RequestId = requestIdParsed;

            var response = await _mediator.Send(command);
            
            if (response.TipoDeFalha == TipoDeFalha.INVALID_DOCUMENT) return BadRequest(response.Message);

            return Ok(response.Message);
        }

        [HttpGet]
        public async Task<IActionResult> Login([FromQuery] LoginContaRequest command)
        {
            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
            {
                return Unauthorized(response.TipoDeFalha.ToString() + ": " + response.Message);
            }

            return Ok(response.Token);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> InativarConta([FromBody]InativarContaCorrenteRequest command)
        {
            string headerAuth = Request.Headers[HeaderNames.Authorization];

            AuthenticationHeaderValue.TryParse(headerAuth, out AuthenticationHeaderValue headerValue);

            string token = headerValue.Parameter;
            command.Token = token;
            var response = await _mediator.Send(command);

            if (!response.IsSuccess) return Forbid();

            return NoContent();
        }
    }
}
