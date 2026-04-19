using AuthPlayground.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace AuthPlayground.Application.Common.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    DbSet<T> Table { get; }
}
