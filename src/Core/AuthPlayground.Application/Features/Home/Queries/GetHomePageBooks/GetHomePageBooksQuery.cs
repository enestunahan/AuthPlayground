using MediatR;

namespace AuthPlayground.Application.Features.Home.Queries.GetHomePageBooks;

public sealed record GetHomePageBooksQuery : IRequest<IReadOnlyList<HomePageBookDto>>;
