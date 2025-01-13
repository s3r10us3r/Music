using System.ComponentModel.DataAnnotations;

namespace Models;

public class User : Entity
{
    [Required] [Length(6, 256)] public string Username { get; set; } = "";
    [Required] public string HashedPassword { get; set; } = "";

    public virtual ICollection<Review> Reviews { get; set; } = [];
}