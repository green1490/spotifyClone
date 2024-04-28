using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

public class Genre: DbContext
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID {get;set;}

    [Column("name")]
    [StringLength(50)]
    public string Name {get;set;} = string.Empty;
}