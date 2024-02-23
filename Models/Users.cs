using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace Model;

[Table("users")]
public class Users 
{
    [Key]
    [Column("id")]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get;set;}

    [DataType(DataType.Text)]
    [MinLength(4, ErrorMessage = "You need a longer name!")]
    [Required]
    [Column("name")]
    public string Name {get;set;} = string.Empty;

    [DataType(DataType.Password)]
    [MinLength(4,ErrorMessage = "You need a longer password!")]
    [Required]
    [Column("password")]
    public string Password {get;set;} = string.Empty;
}