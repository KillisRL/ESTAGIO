using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersDomain.Entidades
{
    [Table ("tblAgendamentoSituacao")]
    public class AgendamentoSituacao
    {
        [Key]
        public int IdSituacao { get; set; }
        public string DescSituacao { get; set; }
    }
}
