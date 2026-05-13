using Application.Abstractions;
using Domain.Entities.Continents;
using Domain.ValueObject.Continents;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class ContinentRepository : IContinent
{
    private readonly AppDbContext _context;

    public ContinentRepository(AppDbContext context) => _context = context;

    public Task<Continent?> GetByIdAsync(ContinentId id, CancellationToken ct = default)
        => _context.Set<Continent>().AsTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<Continent?> GetByNameAsync(ContinentName name, CancellationToken ct = default)
        => _context.Set<Continent>().AsTracking().FirstOrDefaultAsync(x => x.Name == name, ct);

    public Task<IReadOnlyList<Continent>> GetAllAsync(CancellationToken ct = default)
        => _context.Set<Continent>().ToListAsync(ct)
            .ContinueWith(t => (IReadOnlyList<Continent>)t.Result, ct);

    public async Task<IReadOnlyList<Continent>> GetPagedAsync(int page, int pageSize, string? search = null, CancellationToken ct = default)
    {
        IQueryable<Continent> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Continents.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim().ToUpper()}%";
            query = _context.Continents
                .FromSqlInterpolated($@"
                    SELECT *
                    FROM continents
                    WHERE UPPER(""Name"") LIKE {pattern}")
                .AsNoTracking();
        }

        return await query
            .OrderByDescending(x => x.CreateAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        IQueryable<Continent> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = _context.Continents.AsNoTracking();
        }
        else
        {
            var pattern = $"%{search.Trim().ToUpper()}%";
            query = _context.Continents
                .FromSqlInterpolated($@"
                    SELECT *
                    FROM continents
                    WHERE UPPER(""Name"") LIKE {pattern}")
                .AsNoTracking();
        }

        return query.CountAsync(ct);
    }

    public Task AddAsync(Continent continent, CancellationToken ct = default)
    {
        _context.Continents.Add(continent);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Continent continent, CancellationToken ct = default)
    {
        _context.Continents.Update(continent);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Continent continent, CancellationToken ct = default)
    {
        _context.Set<Continent>().Remove(continent);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsNameAsync(ContinentName name, CancellationToken ct = default)
        => _context.Set<Continent>().AnyAsync(x => x.Name == name, ct);
}