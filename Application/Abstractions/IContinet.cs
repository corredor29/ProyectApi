using Domain.Entities.Continents;
using Domain.ValueObject.Continents;

namespace Application.Abstractions;

public interface IContinent
{
    Task<Continent?> GetByIdAsync(ContinentId id, CancellationToken ct = default);
    Task<Continent?> GetByNameAsync(ContinentName name, CancellationToken ct = default);
    Task<IReadOnlyList<Continent>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Continent>> GetPagedAsync(int page, int pageSize, string? search = null, CancellationToken ct = default);
    Task<int> CountAsync(string? search = null, CancellationToken ct = default);
    Task AddAsync(Continent continent, CancellationToken ct = default);
    Task UpdateAsync(Continent continent, CancellationToken ct = default);
    Task RemoveAsync(Continent continent, CancellationToken ct = default);
    Task<bool> ExistsNameAsync(ContinentName name, CancellationToken ct = default);
}