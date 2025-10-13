using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicosDomain.Entidades
{
    [Table("tblServicos")]
    public class Servicos
    {
        [Key]
        public int IdServico { get; set; }
        public string DescServico { get; set; }
        public string Duracao { get; set; }
        public decimal ValorServico { get; set; }
    }
}
