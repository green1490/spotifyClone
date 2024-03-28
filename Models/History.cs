using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

[Table("history")]
public class History
{   
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID {get;set;}

    [Column("song_id")]
    [ForeignKey("song")]
    public int SongID {get;set;}

    [Column("user_id")]
    [ForeignKey("user")]
    public int UserID {get;set;}

    [Column("listened")]
    public DateTime Listened {get;set;} = DateTime.UtcNow;
}