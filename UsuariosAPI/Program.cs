using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UsuariosAPI.Authorization;
using UsuariosAPI.Data;
using UsuariosAPI.Models;
using UsuariosAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



// Fazendo conexao com o banco de dados mysql:
var connectionString = builder.Configuration.GetConnectionString("UsuarioConnection");

builder.Services.AddDbContext<UsuarioDbContext>(opts => {
    opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// confguraçoes de Identity
builder.Services
    // adicionando Identity
    .AddIdentity<Usuario, IdentityRole>()
    // utilizabdo o UsuarioDbContext para fazer a comunicaçao com o banco de dados
    .AddEntityFrameworkStores<UsuarioDbContext>()
    // vai ser utilizado para a autenticaçao
    .AddDefaultTokenProviders();



// configurando o AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// fazendo injeção de dependencia do UsuarioService para usar o serivço do usuario
builder.Services.AddScoped<UsuarioService>();
// fazendo injeção de dependencia do TokenService para usar o serivço do token
builder.Services.AddScoped<TokenService>();



// Autenticaçao do usuários no sistema através do Identity

// adicionando a classe IdadeAuthorization para terminar a autorizaçao da policy
builder.Services.AddSingleton<IAuthorizationHandler, IdadeAuthorization>();

/* vamos adicionar uma autenticação que será feita através do JWT bearer token.
 * precisamos instalar o pacote do JWT
 *                  Microsoft.AspNetCore.Authentication.JwtBearer 6.0.14
*/
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

    // vamos definir as configuraçeos do JwtBearer
}).AddJwtBearer(options => {
    // opçoes de validaçao do token
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters {
        // validaçao da chave que criamos no TokenService
        ValidateIssuerSigningKey = true,
        // utilizansdo a chave symetrica que criamos no TokenService
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("23sfREdhthLUngh12aad")),
        ValidateAudience = false,
        ValidateIssuer = false,
        // faz o get e set do alinhamentodo relogio
        ClockSkew = TimeSpan.Zero
    };
    // la embaixo precisamos dizer que a aplicaçao usa a autenticaçao
});

// definindo a Policy de IdadeMinima para a autorização
builder.Services.AddAuthorization(options => {
    options.AddPolicy("IdadeMinima", policy =>
         policy.AddRequirements(new IdadeMinima(18))

         // crie uma pasta Authorization é dentro uma classe IdadeMinima que vai
         // herdar de IAuthorizationRequirement    
         );
});
 


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// informando que a aplicaçao usa a autenticaçao
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
