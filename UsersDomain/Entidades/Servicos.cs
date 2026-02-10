using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersDomain.Entidades
{
    [Table ("tblServicos")]
    public class Servicos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdServico { get; set; }
        public string? DescServico { get; set; }
        public string? Duracao { get; set; }
        public decimal ValorServico { get; set; }
    }
}
