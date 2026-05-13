using Application.Abstractions;
using Domain.Entities.Continents;
using Domain.ValueObject.Continents;
using MediatR;

namespace Application.UseCase.Continents;

public sealed class CreateContinentHandler : IRequestHandler<CreateContinent, Guid>
{
    private readonly IUniOfWork _uow;

    public CreateContinentHandler(IUniOfWork uow) => _uow = uow;

    public async Task<Guid> Handle(CreateContinent req, CancellationToken ct)
    {
        var name = new ContinentName(req.Name);

        if (await _uow.Continent.ExistsNameAsync(name, ct))
            throw new InvalidOperationException("Ya existe un continente con ese nombre.");

        var id = ContinentId.New();
        var continent = new Continent(id, name);

        await _uow.Continent.AddAsync(continent, ct);
        await _uow.SaveChangesAsync(ct);

        return id.Value;
    }
}