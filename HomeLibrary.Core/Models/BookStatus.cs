using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeLibrary.Core.Models;

[Table("BookStatus")]
public class BookStatus
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty("BookStatus")]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
