using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiContaCorrente.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MovimentacaoController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public IActionResult RealizarMovimentacao()
        {
            return Ok();
        }
    }
}
