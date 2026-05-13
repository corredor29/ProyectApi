using System;

namespace Application.Abstractions;

public interface IUniOfWork
{
    IContinent Continent { get; }
    Task<int> SaveChangesAsync (CancellationToken ct = default);
    Task ExecuteInTransactionAsync(Func<CancellationToken, Task> action, CancellationToken ct = default);
}
