using HomeLibrary.Core.DataTransferObjects;
using HomeLibrary.Core.Interfaces;
using HomeLibrary.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace HomeLibrary.BusinessLogic.Managers;

public class PublisherManager(HomeLibrarySqlContext context) : IPublisherManager
{
    public async Task<IEnumerable<PublisherDto>> GetPublishersAsync()
    {
        return await context.Publishers
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Select(p => new PublisherDto
            {
                Id = p.Id,
                Name = p.Name
            })
            .ToListAsync();
    }
}
