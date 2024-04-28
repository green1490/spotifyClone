using System.ComponentModel.DataAnnotations.Schema;

public class GenreSong
{
    [Column("genre_id")]
    [ForeignKey("genre")]
    public int GenreID {get;set;}
    
    [Column("song_id")]
    [ForeignKey("song")]
    public int SongID {get;set;}
}