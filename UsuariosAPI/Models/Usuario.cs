using Microsoft.AspNetCore.Identity;

namespace UsuariosAPI.Models {
    /* precisamos instalar os pacotes de entitys, identity e mysql:
     * Microsoft.EntityFrameworkCore.Tools
     * Microsoft.AspNetCore.Identity.EntityFrameworkCore
     * Microsoft.Extensions.Identity.Stores
     * Pomelo.EntityFrameworkCore.MySql
    */

    /* Usuario vai ser do tipo IdentityUser que ja vem com classe de usuario montada e podemos
     * ter todas as propriedades de IdentityUser herdando da classe,
     * também evitamos o erro na conexao com o db na classe UsuarioDbContext
    */
    public class Usuario : IdentityUser {
        public DateTime DataNascimento { get; set; }

        // fazendo a chamada do construtor da super classe IdentityUser
        public Usuario(): base() { }
    }
}

/*            Migration:
 * Add-Migration "Criando Usuario"
 * Update-Database
*/
