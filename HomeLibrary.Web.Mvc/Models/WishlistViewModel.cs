using HomeLibrary.Core.DataTransferObjects;

namespace HomeLibrary.Web.Mvc.Models;

public class WishlistViewModel
{
    public IEnumerable<BookDto> Books { get; set; } = new List<BookDto>();
    public string? SearchTerm { get; set; }
}
