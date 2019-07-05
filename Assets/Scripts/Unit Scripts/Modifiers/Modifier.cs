public enum StatModType
{
    Flat,
    Percent,
}

public class Modifier
{
    public readonly float Value;
    public readonly StatModType Type;
    public readonly int Order;


    public Modifier(float value, StatModType type, int order)
    {
        Value = value;
        Type = type;
        Order = order;
    }

    public Modifier(float value, StatModType type) : this(value, type, (int)type) { }
}
