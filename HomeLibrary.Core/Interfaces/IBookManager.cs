namespace HomeLibrary.Core.Interfaces;

public interface IBookManager
{
    Task<int> GetOwnedBookCountAsync();
    Task<int> GetWishlistBookCountAsync();
}
