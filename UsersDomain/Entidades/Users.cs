using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersDomain.Entidades
{
    [Table("tblUsuario")]
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IDUsuario { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? SenhaHash { get; set; }

        [Required]
        [Column("IdPessoaTipo")] // Mapeia explicitamente para a coluna do banco
        public TipoUsuario IdPessoaTipo { get; set; }
    }
}
