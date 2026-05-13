// Application/UseCase/Continents/CreateContinent.cs
using MediatR;

namespace Application.UseCase.Continents;

public sealed record CreateContinent(string Name) : IRequest<Guid>;