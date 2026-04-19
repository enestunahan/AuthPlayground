using AuthPlayground.Application.Common.Repositories.Books;
using AuthPlayground.Domain.Entities;
using AuthPlayground.Persistence.Contexts;

namespace AuthPlayground.Persistence.Repositories.Books;

public sealed class BookWriteRepository(AuthPlaygroundDbContext context)
    : WriteRepository<Book>(context), IBookWriteRepository;
