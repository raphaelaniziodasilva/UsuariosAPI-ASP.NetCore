using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UsuariosAPI.Data.Dtos;
using UsuariosAPI.Models;

namespace UsuariosAPI.Services;


// para usar o UsuarioService temos que configurar
// para fazer isso precisamos fazer essa injeção de dependencia no arquivo Program.cs.
public class UsuarioService {

    // chamando IMapper para converter o usuario
    private readonly IMapper _mapper;
    // chamando o UserManager para ter acesso ao db
    private readonly UserManager<Usuario> _userManager;
    // chamando o SignInManager para fazer o login do usuario
    private readonly SignInManager<Usuario> _signInManager;
    // chamando o TokenService para gerar o token do usuario
    private readonly TokenService _tokenService;

    // injetando IMapper, UserManager e SignInManager no construtor
    public UsuarioService(IMapper mapper, UserManager<Usuario> userManager,
        SignInManager<Usuario> signInManager, TokenService tokenService) {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    // Task<IActionResult> representa uma operaçao assincrona que pode retornar um valor
    public async Task CadastraAsync(CreateUsuarioDto usuarioDto) {
        // convertendo usuario para o tipo CreateUsuarioDto
        Usuario usuario = _mapper.Map<Usuario>(usuarioDto);

        // cadastrando usuario no db utilizando o IdentityResult
        // CreateAsync = vai salvar o usuario e senha no db
        IdentityResult resultado = await _userManager.CreateAsync(usuario, usuarioDto.Password);

        if (!resultado.Succeeded) {
            throw new ApplicationException("Falha ao cadastrar usuário!");
        }
    }

    // Task<IActionResult> representa uma operaçao assincrona que pode retornar um valor
    public async Task<string> LoginAsync(LoginUsuarioDto loginUsuarioDto) {
        // PasswordSignInAsync = a partir do usuario e senha vai fazer a autenticação do usuario
        var resultado = await _signInManager.PasswordSignInAsync(
            loginUsuarioDto.Username, loginUsuarioDto.Password, false, false);

        if(!resultado.Succeeded) {
            throw new ApplicationException("Usuário não autenticado!");
        }

        // precisamos pegar o usuario
        var usuario = _signInManager.UserManager.Users.FirstOrDefault(
            user => user.NormalizedUserName == loginUsuarioDto.Username.ToUpper());

        var token = _tokenService.GenerateToken(usuario);

        return token;
    }
}
