using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersDomain.Entidades
{
    [Table("tblBarbeiro")]
    public  class BarbeiroCreate
    {
        public readonly object? Entity;
        public string NomeBarbeiro { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public bool Ativo { get; set; }
        public TipoUsuario IdPessoaTipo { get; set; }
    }
}
