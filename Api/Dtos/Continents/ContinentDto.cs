namespace Api.Dtos.Continents;

public sealed class ContinentDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public DateTime CreatedAt { get; init; }
}