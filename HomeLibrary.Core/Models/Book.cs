using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeLibrary.Core.Models;

public class Book
{
    [Key]
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    [Column("ISBN")]
    [StringLength(50)]
    public string? Isbn { get; set; }

    public int? AuthorId { get; set; }

    public int GenreId { get; set; }

    public int? PublisherId { get; set; }

    public int? PublishedYear { get; set; }

    public string? CoverImageUrl { get; set; }

    public int BookStatusId { get; set; }

    public string? Notes { get; set; }

    [ForeignKey("AuthorId")]
    [InverseProperty("Books")]
    public virtual Author? Author { get; set; }

    [ForeignKey("BookStatusId")]
    [InverseProperty("Books")]
    public virtual BookStatus BookStatus { get; set; } = null!;

    [ForeignKey("GenreId")]
    [InverseProperty("Books")]
    public virtual Genre Genre { get; set; } = null!;

    [ForeignKey("PublisherId")]
    [InverseProperty("Books")]
    public virtual Publisher? Publisher { get; set; }
}
