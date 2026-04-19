using AuthPlayground.Application.Common.Repositories.Books;
using AuthPlayground.Application.Features.Home.Queries.GetHomePageBooks;
using AuthPlayground.Domain.Entities;
using AuthPlayground.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AuthPlayground.Persistence.Repositories.Books;

public sealed class BookReadRepository(AuthPlaygroundDbContext context)
    : ReadRepository<Book>(context), IBookReadRepository
{
    public async Task<IReadOnlyList<HomePageBookDto>> GetHomePageBooksAsync(CancellationToken cancellationToken = default)
    {
        return await Table
            .AsNoTracking()
            .OrderBy(book => book.Title)
            .Select(book => new HomePageBookDto(
                book.Id,
                book.Title,
                book.Description,
                book.Isbn,
                book.PublicationYear,
                book.Price))
            .ToListAsync(cancellationToken);
    }
}
