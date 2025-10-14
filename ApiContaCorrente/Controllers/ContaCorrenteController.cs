using ApiContaCorrente.ContasCorrentes.Commands.Requests;
using ApiContaCorrente.ContasCorrentes.Queries.Requests;
using ApiContaCorrente.Helpers;
using ApiContaCorrente.Interfaces;
using ApiContaCorrente.Models.Dto;
using ApiContaCorrente.Models.Enums;
using ApiContaCorrente.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Cadastrar([FromBody]CriarContaCorrenteRequest command)
        {
            var response = await _mediator.Send(command);
            
            if (response.TipoDeFalha == TipoDeFalha.INVALID_DOCUMENT) return BadRequest(response.Message);

            return Ok(response.Message);
        }

        [HttpGet]
        public async Task<IActionResult> Login([FromQuery] LoginContaRequest command)
        {
            var response = await _mediator.Send(command);
            return Ok(response.Token);
        }

        //criar método de login e utilizar o tokenprovider no retorno

        //[HttpPut]
        //[Authorize]
        //public IActionResult InativarConta()
        //{

        //}
    }
}
