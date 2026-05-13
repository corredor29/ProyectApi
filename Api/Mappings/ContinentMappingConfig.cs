using Api.Dtos.Continents;
using Application.UseCase.Continents;
using Domain.Entities.Continents;
using Domain.ValueObject.Continents;
using Mapster;

namespace Api.Mappings;

public sealed class ContinentMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Continent, ContinentDto>()
            .Map(dest => dest.Id,   src => src.Id.Value)
            .Map(dest => dest.Name, src => src.Name.Value);

        config.NewConfig<CreateContinentRequest, CreateContinent>()
            .MapWith(src => new CreateContinent(src.Name));

        config.NewConfig<UpdateContinentRequest, UpdateContinent>()
            .MapWith(src => new UpdateContinent(Guid.Empty, src.Name));
    }
}