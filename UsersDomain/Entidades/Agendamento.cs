using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersDomain.Entidades
{
    [Table ("tblAgendamento") ]
    public class Agendamento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAgendamento { get; set; }
        public int IdBarbeiro { get; set; }
        public int IdServico { get; set; }
        public int IDUsuario { get; set; }
        public int IdSituacao { get; set; }

        [ForeignKey("IdBarbeiro")]
        public Barbeiro? Barbeiro { get; set; }

        [ForeignKey("IdServico")]
        public Servicos? Servico { get; set; }

        public DateTime DataHora { get; set; }

         [ForeignKey("IDUsuario")]
         public Users? Users { get; set; }

        [ForeignKey("IdSituacao")]
        public AgendamentoSituacao AgendamentoSituacao { get; set; }
    }
}
