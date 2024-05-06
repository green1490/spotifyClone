namespace Model;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

[Table("playlist")]
public class Playlist
{   
    [Key]
    [Column("id")]
    [SwaggerSchema(ReadOnly = true)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID {get; set;}

    [Column("name")]
    public string Name {get; set;} = string.Empty;

    [Column("created")]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime Created  {get; set;} = DateTime.UtcNow;

    [Column("modified")]
    [SwaggerSchema(ReadOnly = true)]
    public DateTime Modified {get; set;} = DateTime.UtcNow;

    [Column("user_id")]
    [ForeignKey("users")]
    [SwaggerSchema(ReadOnly = true)]
    public int UserID {get; set;}
}

