using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersDomain.Entidades
{
    public class CriarAgendamentoDTO
    {
        // Apenas os campos que o cliente PRECISA enviar
        public int IdBarbeiro { get; set; }
        public int IdServico { get; set; }
        public DateTime DataHora { get; set; }
        public  int IdSituacao { get; set; }
        public int IDUsuario { get; set; }
    }
}
