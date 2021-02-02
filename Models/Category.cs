using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage = "Este campo deve conter no máximo 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter no mínimo 3 caracteres")]
        [DataType("nvarchar")]
        public string Title { get; set; }
    }
}
