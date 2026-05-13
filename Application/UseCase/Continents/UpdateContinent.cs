// Application/UseCase/Continents/UpdateContinent.cs
using MediatR;

namespace Application.UseCase.Continents;

public sealed record UpdateContinent(Guid Id, string Name) : IRequest;