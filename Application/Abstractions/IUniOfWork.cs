using System;

namespace Application.Abstractions;

public interface IUniOfWork
{
    Task<int> SaveChangesAsync (CancellationToken ct = default);
    Task ExecuteInTransactionAsync(Func<CancellationToken, Task> action, CancellationToken ct = default);
}
