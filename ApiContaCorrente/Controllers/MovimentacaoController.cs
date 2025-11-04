using ApiContaCorrente.Movimentacoes.Commands.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;

namespace ApiContaCorrente.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MovimentacaoController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RealizarMovimentacao(
            [FromBody] RealizarMovimentacaoRequest command,
            [FromHeader(Name = "X-Idempotency-Key")] string requestId
            )
        {
            string headerAuth = Request.Headers[HeaderNames.Authorization];

            AuthenticationHeaderValue.TryParse(headerAuth, out AuthenticationHeaderValue headerValue);

            string token = headerValue.Parameter;
            command.Token = token;

            if (!Guid.TryParse(requestId, out Guid requestIdParsed))
            {
                return BadRequest("X-Idempotency-Key do header está faltando ou é inválida.");
            }

            command.RequestId = requestIdParsed;

            var response = await _mediator.Send(command);

            if(!response.IsSuccess)
            {
                string badResponse = response.TipoDeFalha.ToString() + ": " + response.Message;
                return BadRequest(badResponse); 
            }

            return NoContent();
        }
    }
}
