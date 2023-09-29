using System.ComponentModel.DataAnnotations;

namespace UsuariosAPI.Data.Dtos; 
public class CreateUsuarioDto {
    [Required]
    public string Username { get; set; }

    [Required]
    public DateTime DataNascimento { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Compare("Password")]
    public string ConfirmaPassword { get; set; }
}
