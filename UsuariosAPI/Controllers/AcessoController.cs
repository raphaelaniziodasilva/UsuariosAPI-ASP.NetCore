using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UsuariosAPI.Controllers;

[ApiController]
[Route("[Controller]")]
public class AcessoController : ControllerBase {
    // validando o acesso com o token

    [HttpGet]
    // [Authorize] = dada a determinada condição de autorização
    // Policy = serve para criar a politica de acesso da condição de autorização
    // precisamos definir a Policy de IdadeMinima e como vai ser definida la no arquivo Program.cs
    [Authorize(Policy = "IdadeMinima")]
    public IActionResult Get() {
        return Ok("Acesso permitido!");
    }
}
