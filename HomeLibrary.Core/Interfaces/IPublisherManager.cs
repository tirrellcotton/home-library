using HomeLibrary.Core.DataTransferObjects;

namespace HomeLibrary.Core.Interfaces;

public interface IPublisherManager
{
    Task<IEnumerable<PublisherDto>> GetPublishersAsync();
}
