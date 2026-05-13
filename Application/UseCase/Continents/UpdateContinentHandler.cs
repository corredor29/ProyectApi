// Application/UseCase/Continents/UpdateContinentHandler.cs
using Application.Abstractions;
using Domain.ValueObject.Continents;
using MediatR;

namespace Application.UseCase.Continents;

public sealed class UpdateContinentHandler(IUniOfWork uow) : IRequestHandler<UpdateContinent>
{
    public async Task Handle(UpdateContinent req, CancellationToken ct)
    {
        var continentId = new ContinentId(req.Id);
        var continent = await uow.Continent.GetByIdAsync(continentId, ct);

        if (continent is null)
            throw new KeyNotFoundException("Continente no encontrado.");

        var newName = new ContinentName(req.Name);

        if (continent.Name != newName && await uow.Continent.ExistsNameAsync(newName, ct))
            throw new InvalidOperationException("Ya existe un continente con ese nombre.");

        continent.UpdateName(newName);

        await uow.Continent.UpdateAsync(continent, ct);
        await uow.SaveChangesAsync(ct);
    }
}