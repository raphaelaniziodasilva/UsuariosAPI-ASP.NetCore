using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace UsuariosAPI.Authorization;

// AuthorizationHandler<IdadeMinima> = vai ser reconhecida pelo dotenet como um gerenciador
// que consegue lidar com a autorizaçao e interceptaçao
public class IdadeAuthorization : AuthorizationHandler<IdadeMinima> {
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        IdadeMinima requirement) {

        // vamos fazer a logica para pegar a data de nascimento e verificar se esta valida ou nao

        // recuperar o token e pegar claim de data de nascimento: DateOfBirth
        // context = vai permitir acessar as informaçoes do usuario
        var dataNascimentoClaim = context.User.FindFirst(claim =>
        claim.Type == ClaimTypes.DateOfBirth);

        // verificando se a informçao existe
        if(dataNascimentoClaim is null) {
            return Task.CompletedTask;
        }

        // vamos recuperar a data de nascimento e fazer a conversao para o dateTime 
        var dataNascimento = Convert.ToDateTime(dataNascimentoClaim.Value);

        // calculando a idade do usuario
        var idadeUsuario = DateTime.Today.Year - dataNascimento.Year;

        if(dataNascimento > DateTime.Today.AddYears(-idadeUsuario)) {
            idadeUsuario--;
        }

        // verificando se a idade e maior que 18 anos
        if(idadeUsuario >= requirement.Idade) {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

// precisamos adicionar essa classe no Program.cs