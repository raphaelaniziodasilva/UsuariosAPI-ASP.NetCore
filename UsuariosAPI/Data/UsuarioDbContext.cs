using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UsuariosAPI.Models;

// vamos fazer a conexao com o banco de dados

namespace UsuariosAPI.Data;

// UsuarioDbContext vai ser do tipo IdentityDbContext que faz referencia com o model Usuario
public class UsuarioDbContext : IdentityDbContext<Usuario> {
    // construtor
    public UsuarioDbContext(DbContextOptions<UsuarioDbContext> opts) : base(opts) {

    }
}
