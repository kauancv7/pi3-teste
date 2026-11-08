
using WebApplicationsgls.Domain.Enums;

namespace WebApplicationsgls.Application.DTOs
{
    public class CriarUsuarioDTO
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public PerfilUsuarioEnum Perfil { get; set; } 

    }
}
