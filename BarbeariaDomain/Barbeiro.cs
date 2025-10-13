using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarbeariaDomain
{
    [Table("tblBarbeiro")]
    public class Barbeiro
    {
        [Key]
        public int IdBarbeiro { get; set; }
        public string NomeBarbeiro { get; set; }

        public bool Ativo { get; set; }
    }
}
