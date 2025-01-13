using System.ComponentModel.DataAnnotations;

namespace Models;

public class Review : Entity
{
    [Required] [Range(1,5)] public int Value { get; set; }
    public string Message { get; set; } = "";
    public int UserId { get; set; }
    public int AlbumId { get; set; }
    public DateTime DateTime { get; set; }

    public virtual User User { get; set; } = new User();
    public virtual Album Album { get; set; } = new Album();
}