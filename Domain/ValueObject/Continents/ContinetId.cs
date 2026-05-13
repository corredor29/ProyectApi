namespace Domain.ValueObject.Continents;

public readonly record struct ContinentId
{
    public Guid Value {get;}

    public ContinentId (Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Id continents cannot be empty.",nameof(value));
        
        Value = value;
    }

    public static ContinentId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
