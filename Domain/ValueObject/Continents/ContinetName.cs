namespace Domain.ValueObject.Continents;

public readonly record struct ContinentName
{
    public string Value {get;}

    public ContinentName (string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Name cannot be empty",nameof(value));
        if (value.Length > 100)
            throw new ArgumentException("Name cannot exceed 100 characters.",nameof(value));
        Value = value.Trim();
    }
    public override string ToString() => Value;
}
