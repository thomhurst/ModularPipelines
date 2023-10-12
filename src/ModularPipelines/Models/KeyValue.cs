namespace ModularPipelines.Models;

public record KeyValue(string Key, string Value, string Separator)
{
    public KeyValue(string Key, string Value) : this(Key, Value, "=")
    {
    }

    public override string ToString()
    {
        return $"{Key}{Separator}{Value}";
    }

    public static implicit operator KeyValue((string Key, string Value) stringTuple) => new(stringTuple.Key, stringTuple.Value);
}