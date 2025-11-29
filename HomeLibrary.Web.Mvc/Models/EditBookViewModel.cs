using System.ComponentModel.DataAnnotations;
using HomeLibrary.Core.DataTransferObjects;

namespace HomeLibrary.Web.Mvc.Models;

public class EditBookViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string? Title { get; set; }

    [StringLength(50, ErrorMessage = "ISBN cannot exceed 50 characters")]
    public string? Isbn { get; set; }

    [Display(Name = "Author")]
    public int? AuthorId { get; set; }

    [Required(ErrorMessage = "Genre is required")]
    [Display(Name = "Genre")]
    public int GenreId { get; set; }

    [Display(Name = "Publisher")]
    public int? PublisherId { get; set; }

    [Display(Name = "Published Year")]
    [Range(1000, 9999, ErrorMessage = "Please enter a valid year")]
    public int? PublishedYear { get; set; }

    [Required(ErrorMessage = "Status is required")]
    [Display(Name = "Status")]
    public int BookStatusId { get; set; }

    [Display(Name = "Cover Image URL")]
    [Url(ErrorMessage = "Please enter a valid URL")]
    public string? CoverImageUrl { get; set; }

    [Display(Name = "Notes")]
    [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
    public string? Notes { get; set; }

    // Dropdown data
    public IEnumerable<GenreDto> Genres { get; set; } = new List<GenreDto>();
    public IEnumerable<AuthorDto> Authors { get; set; } = new List<AuthorDto>();
    public IEnumerable<PublisherDto> Publishers { get; set; } = new List<PublisherDto>();
}
