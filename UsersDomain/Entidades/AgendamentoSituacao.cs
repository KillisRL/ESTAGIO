using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
