using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

[Table("song")]
public class Song
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int ID {get;set;}

    [Column("name")]
    public string Name {get;set;} = string.Empty;

    [Column("artist")]
    public string Artist {get;set;} = string.Empty;

    [Column("song_path")]
    public string SongPath {get;set;} = string.Empty;

    [Column("user_id")]
    [ForeignKey("users")]
    public int UserID {get;set;}
}