using HomeLibrary.Core.DataTransferObjects;

namespace HomeLibrary.Core.Interfaces;

public interface IAuthorManager
{
    Task<IEnumerable<AuthorDto>> GetAuthorsAsync();
    AuthorDto? GetAuthorById(int id);
}