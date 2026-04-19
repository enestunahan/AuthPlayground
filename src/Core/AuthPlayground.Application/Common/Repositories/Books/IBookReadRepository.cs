using AuthPlayground.Application.Features.Home.Queries.GetHomePageBooks;
using AuthPlayground.Domain.Entities;

namespace AuthPlayground.Application.Common.Repositories.Books;

public interface IBookReadRepository : IReadRepository<Book>
{
    Task<IReadOnlyList<HomePageBookDto>> GetHomePageBooksAsync(CancellationToken cancellationToken = default);
}
