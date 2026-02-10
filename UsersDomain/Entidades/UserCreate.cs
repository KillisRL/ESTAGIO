using System.ComponentModel.DataAnnotations.Schema;

namespace UsersDomain.Entidades 
{
    [Table("tblUsuario")]
    public class UserCreate
    {
        public readonly object? Entity;
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string SenhaHash { get; set; }
        public TipoUsuario IdPessoaTipo { get; set; }
    }
}
