using AutoMapper;
using UsuariosAPI.Data.Dtos;
using UsuariosAPI.Models;

namespace UsuariosAPI.Profiles;
/* vamos utilizar o Automapper, precisamos instalar o pacote do automapper:
 *          AutoMapper.Extensions.Microsoft.DependencyInjection
*/
public class UsuarioProfile : Profile {

    public UsuarioProfile() {
        CreateMap<CreateUsuarioDto, Usuario>();
    }
}
