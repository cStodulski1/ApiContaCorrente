using ApiContaCorrente.Helpers;
using ApiContaCorrente.Interfaces;
using ApiContaCorrente.Models.Dto;
using ApiContaCorrente.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiContaCorrente.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ContaCorrenteController(IContaCorrenteService contaCorrenteService) : Controller
    {
        private readonly IContaCorrenteService contaCorrenteService = contaCorrenteService;

        [HttpPost]
        public async Task<IActionResult> Cadastrar(CriarContaDto contaDto)
        {
            bool cpfValido = CpfHelper.IsCpfValid(contaDto.Cpf);
            if (!cpfValido) return BadRequest("INVALID_DOCUMENT: Cpf inválido");

            var result = await contaCorrenteService.CriarContaCorrente(contaDto);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        //[HttpPut]
        //[Authorize]
        //public IActionResult InativarConta()
        //{

        //}
    }
}
