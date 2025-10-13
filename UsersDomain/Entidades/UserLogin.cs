using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersDomain.Entidades
{
    [Table("tblUsuario")]
    public class UserLogin
    {
        public string? Email { get; set; }
        public string? SenhaHash { get; set; }
    }
}
