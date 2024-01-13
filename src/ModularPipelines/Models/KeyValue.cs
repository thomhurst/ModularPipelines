namespace ModularPipelines.Models;

public record KeyValue(string Key, string Value, string Separator)
{
    public KeyValue(string key, string value) : this(key, value, "=")
    {
    }

    public override string ToString()
    {
        return $"{Key}{Separator}{Value}";
    }

    public static implicit operator KeyValue((string Key, string Value) stringTuple) => new(stringTuple.Key, stringTuple.Value);

    public static implicit operator KeyValue(Tuple<string, string> tuple) => new(tuple.Item1, tuple.Item2);

    public static implicit operator KeyValue(KeyValuePair<string, string> tuple) => new(tuple.Key, tuple.Value);
}