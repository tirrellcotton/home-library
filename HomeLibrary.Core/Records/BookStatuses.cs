namespace HomeLibrary.Core.Records;

public record BookStatuses(int Id, string Name)
{
    public static BookStatuses Owned { get; } = new BookStatuses(1, "Owned");
    public static BookStatuses Wishlist { get; } = new BookStatuses(2, "Wishlist");
    public static BookStatuses Read { get; } = new BookStatuses(3, "Read");
}