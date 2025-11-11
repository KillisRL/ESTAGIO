using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersDomain.Entidades
{
    [Table("tblBarbeiro")]
    public class Barbeiro
    {
        [Key]
        public int IdBarbeiro { get; set; } 

        public string? NomeBarbeiro { get; set; } // Coluna para o nome (baseado no script SQL original)
        public bool? Ativo { get; set; } // Recomendado manter anuláve

        public string? Login { get; set; }

        public string? Senha { get; set; }

        [Required]
        [Column("IdPessoaTipo")] // Mapeia explicitamente para a coluna do banco
        public TipoUsuario IdPessoaTipo { get; set; }
    }
}
