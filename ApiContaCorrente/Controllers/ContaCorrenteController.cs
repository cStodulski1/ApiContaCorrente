using Microsoft.AspNetCore.Mvc;

namespace ApiContaCorrente.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ContaCorrenteController : Controller
    {
        [HttpGet]
        public IActionResult Teste()
        {
            return Ok("Just checking docker");
        }
    }
}
