using Microsoft.AspNetCore.Authorization;

namespace UsuariosAPI.Authorization;

// IdadeMinima vai herdar de IAuthorizationRequirement
public class IdadeMinima : IAuthorizationRequirement {
    public int Idade { get; set; }

    public IdadeMinima(int idade) {
        Idade = idade;
    }

    /* vamos escrever a logica responsavel por receber o token do usuario e interceptar,
     * validar se a data de nascimento pertencente ao token e maior que 18 anos e
     * se sim vai ter acesso e se nao agente bloqueia
     * 
     * vamos criar a classe IdadeAuthorization que vai ficar responsavel por essa logica
     */

}
