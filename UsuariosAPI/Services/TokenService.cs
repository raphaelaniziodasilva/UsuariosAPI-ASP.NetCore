using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsuariosAPI.Models;

namespace UsuariosAPI.Services;

// para usar o TokenService temos que configurar
// para fazer isso precisamos fazer essa injeção de dependencia no arquivo Program.cs.
public class TokenService {
    public string GenerateToken(Usuario usuario) {
        // Claim[] = reivindicações:informações do usuario
        Claim[] claims = new Claim[] {
            new Claim("id", usuario.Id),
            new Claim("username", usuario.UserName),
            new Claim(ClaimTypes.DateOfBirth, usuario.DataNascimento.ToString())
        };

        /* para trabalhar com o token precisamos instalar um pacote para o jwt:
         * System.IdentityModel.Tokens.Jwt
        */

        // gerando a chave da assinatura credenciais
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("23sfREdhthLUngh12aad"));
        // gerando as assinatura credenciais
        var signingCredentials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        // gerando o token
        var token = new JwtSecurityToken(
            expires: DateTime.Now.AddMinutes(10), // expiração do token
            claims: claims, // reivindicações:informações do usuario
            signingCredentials: signingCredentials // assinatura credenciais
            );

        // convertendo o token para string
        return new JwtSecurityTokenHandler().WriteToken(token); 
    }
}