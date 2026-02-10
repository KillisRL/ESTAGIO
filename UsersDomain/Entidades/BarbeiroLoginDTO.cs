using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersDomain.Entidades
{
    [Table("tblBarbeiro")]
    public class BarbeiroLoginDTO
    {
        [Required(ErrorMessage = "O login é obrigatório.")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string? Senha { get; set; }
    }
}
