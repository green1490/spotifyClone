using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Model;

[Table("playlist_content")]
[PrimaryKey(nameof(PlaylistID), nameof(SongID))]
public class PlaylistContent
{
    [Key, Column("playlist_id")]
    public int PlaylistID {get; set;}

    [Key, Column("song_id")]
    public int SongID {get; set;}
}
