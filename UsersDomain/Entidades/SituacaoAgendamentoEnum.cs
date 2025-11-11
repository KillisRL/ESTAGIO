using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersDomain.Entidades
{
    public class SituacaoAgendamentoEnum
    {
        public enum StatusAgendamento
        {
            Aberto = 1,
            Cancelado = 2,
            Finalizado = 3
            // Adicione outros status conforme necessário
        }
    }
}
