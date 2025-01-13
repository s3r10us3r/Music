using System.ComponentModel.DataAnnotations;

namespace Models;

public class Album : Entity
{
    [Required] [Length(1, 256)] public string Name { get; set; } = "";
    [Required] [Length(1, 256)] public string Artist { get; set; } = "";
    [Required] public string Image { get; set; } = ""; //b64
    [Required] public int Year { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = [];
}