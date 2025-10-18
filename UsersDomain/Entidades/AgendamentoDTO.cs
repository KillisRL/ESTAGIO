using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersDomain.Entidades
{
    public class AgendamentoDTO
    {
        public int IdAgendamento { get; set; }
        public int IdServico { get; set; }
        public string DescServico { get; set; }
        public DateTime DataHora { get; set; }
        public int IdBarbeiro { get; set; }
        public string NomeBarbeiro { get; set; }
        public int IDUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public string Email { get; set; }
        public int IdSituacao { get; set; }
        public string DescSituacao { get; set; }
    }
}
