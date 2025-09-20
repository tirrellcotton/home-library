using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeLibrary.Core.Models;

public class Genre
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [InverseProperty("Genre")]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
