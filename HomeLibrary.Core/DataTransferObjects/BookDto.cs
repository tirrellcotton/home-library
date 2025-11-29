namespace HomeLibrary.Core.DataTransferObjects;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Author { get; set; }
    public string Genre { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public int? PublishedYear { get; set; }
}
