using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersDomain.Entidades
{
    [Table("tblBarbeiro")]
    public class Barbeiro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdBarbeiro { get; set; } 
        public string? NomeBarbeiro { get; set; } 
        public bool? Ativo { get; set; } 
        public string? Login { get; set; }
        public string? Senha { get; set; }

        [Required]
        [Column("IdPessoaTipo")] 
        public TipoUsuario IdPessoaTipo { get; set; }
    }
}
