using System.ComponentModel.DataAnnotations.Schema;

namespace UsersDomain.Entidades
{
    [Table("tblUsuario")]
    public class UserLogin
    {
        public string? Email { get; set; }
        public string? SenhaHash { get; set; }
    }
}
