using MediatR;

namespace AuthPlayground.Application.Features.Books.Commands.DeleteBook;

public sealed record DeleteBookCommand(Guid Id) : IRequest<bool>;
