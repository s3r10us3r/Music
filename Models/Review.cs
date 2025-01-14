using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Review : Entity
{
    [Required] [Range(1,5)] public int Value { get; set; }
    public string Message { get; set; } = "";
    public int UserId { get; set; }
    [ForeignKey(nameof(Album))]
    public int AlbumId { get; set; }
    public DateTime DateTime { get; set; }

    public virtual User User { get; set; } 
    public virtual Album Album { get; set; } 
}