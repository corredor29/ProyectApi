using System;
using Domain.ValueObject.Continents;
using Domain.Common;

namespace Domain.Entities.Continents;

public sealed class Continent : BaseEntity<ContinentId>
{
    public ContinentName Name {get; private set;}

    private Continent() {}

    public Continent(ContinentId id, ContinentName name)
    {
        Id = id;
        Name = name;
    }
    public void UpdateName (ContinentName name) => Name =name;    
}
