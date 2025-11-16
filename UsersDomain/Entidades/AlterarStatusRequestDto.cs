using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UsersDomain.Entidades.SituacaoAgendamentoEnum;

namespace UsersDomain.Entidades
{
    public class AlterarStatusRequestDto
    {
        [Required(ErrorMessage = "O novo status é obrigatório.")]

        [EnumDataType(typeof(StatusAgendamento))]
        public int NovoStatusId { get; set; }
    }
}
