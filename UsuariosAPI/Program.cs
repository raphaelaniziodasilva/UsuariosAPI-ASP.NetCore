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

// confgura�oes de Identity
builder.Services
    // adicionando Identity
    .AddIdentity<Usuario, IdentityRole>()
    // utilizabdo o UsuarioDbContext para fazer a comunica�ao com o banco de dados
    .AddEntityFrameworkStores<UsuarioDbContext>()
    // vai ser utilizado para a autentica�ao
    .AddDefaultTokenProviders();



// configurando o AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// fazendo inje��o de dependencia do UsuarioService para usar o seriv�o do usuario
builder.Services.AddScoped<UsuarioService>();
// fazendo inje��o de dependencia do TokenService para usar o seriv�o do token
builder.Services.AddScoped<TokenService>();



// Autentica�ao do usu�rios no sistema atrav�s do Identity

// adicionando a classe IdadeAuthorization para terminar a autoriza�ao da policy
builder.Services.AddSingleton<IAuthorizationHandler, IdadeAuthorization>();

/* vamos adicionar uma autentica��o que ser� feita atrav�s do JWT bearer token.
 * precisamos instalar o pacote do JWT
 *                  Microsoft.AspNetCore.Authentication.JwtBearer 6.0.14
*/
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

    // vamos definir as configura�eos do JwtBearer
}).AddJwtBearer(options => {
    // op�oes de valida�ao do token
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters {
        // valida�ao da chave que criamos no TokenService
        ValidateIssuerSigningKey = true,
        // utilizansdo a chave symetrica que criamos no TokenService
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("23sfREdhthLUngh12aad")),
        ValidateAudience = false,
        ValidateIssuer = false,
        // faz o get e set do alinhamentodo relogio
        ClockSkew = TimeSpan.Zero
    };
    // la embaixo precisamos dizer que a aplica�ao usa a autentica�ao
});

// definindo a Policy de IdadeMinima para a autoriza��o
builder.Services.AddAuthorization(options => {
    options.AddPolicy("IdadeMinima", policy =>
         policy.AddRequirements(new IdadeMinima(18))

         // crie uma pasta Authorization � dentro uma classe IdadeMinima que vai
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

// informando que a aplica�ao usa a autentica�ao
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
