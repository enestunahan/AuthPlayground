using MediatR;

namespace AuthPlayground.Application.Features.Books.Queries.GetBooksForAdmin;

public sealed record GetBooksForAdminQueryRequest : IRequest<GetBooksForAdminQueryResponse>;
