using System;
using  Infrastructure.Context;
using Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UniOfWork;

public class EfUnirOfWork : IUniOfWork
{
    private readonly AppDbContext _contextdb;

    public EfUnirOfWork (AppDbContext db)
    {
        _contextdb = db;
    }
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _contextdb.SaveChangesAsync(ct);

    public async Task ExecuteInTransactionAsync(Func<CancellationToken, Task> operation, CancellationToken ct = default)
    {
        await using var tx = await _contextdb.Database.BeginTransactionAsync(ct);
        try
        {
            await operation(ct);
            await _contextdb.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
        }
        catch
        {
            await tx.RollbackAsync(ct);
            throw;
        }
    }    
}
