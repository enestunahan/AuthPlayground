using AuthPlayground.Application.Common.Repositories.Books;
using MediatR;

namespace AuthPlayground.Application.Features.Home.Queries.GetHomePageBooks;

public sealed class GetHomePageBooksQueryHandler(IBookReadRepository bookReadRepository)
    : IRequestHandler<GetHomePageBooksQuery, IReadOnlyList<HomePageBookDto>>
{
    public async Task<IReadOnlyList<HomePageBookDto>> Handle(
        GetHomePageBooksQuery request,
        CancellationToken cancellationToken)
    {
        return await bookReadRepository.GetHomePageBooksAsync(cancellationToken);
    }
}
