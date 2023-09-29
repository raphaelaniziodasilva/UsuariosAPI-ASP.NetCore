using Microsoft.AspNetCore.Mvc;
using UsuariosAPI.Data.Dtos;
using UsuariosAPI.Services;

namespace UsuariosAPI.Controllers;

[ApiController]
[Route("[Controller]")]
public class UsuarioController : ControllerBase {
    // chamando o serviço de usuario
    private readonly UsuarioService _usuarioService;

    public UsuarioController(UsuarioService usuarioService) {
        _usuarioService = usuarioService;
    }

    [HttpPost("cadastro")]
    // Task<IActionResult> representa uma operaçao assincrona que pode retornar um valor
    // IActionResult e o resultado do metodo
    public async Task<IActionResult> CadastraUsuario(CreateUsuarioDto usuarioDto) {
        await _usuarioService.CadastraAsync(usuarioDto);
        return Ok("Usuário cadastrado!");
    }

    [HttpPost("login")]
    // Task<IActionResult> representa uma operaçao assincrona que pode retornar um valor
    // IActionResult e o resultado do metodo
    public async Task<IActionResult> Login(LoginUsuarioDto loginUsuarioDto) {
        var token = await _usuarioService.LoginAsync(loginUsuarioDto);
        return Ok(token);
    }
}