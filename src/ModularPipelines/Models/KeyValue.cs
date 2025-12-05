namespace ModularPipelines.Models;

/// <summary>
/// Represents a key-value pair with a configurable separator.
/// </summary>
/// <param name="Key">The key portion of the key-value pair.</param>
/// <param name="Value">The value portion of the key-value pair.</param>
/// <param name="Separator">The separator between the key and value.</param>
public record KeyValue(string Key, string Value, string Separator)
{
    /// <summary>
    /// Initialises a new instance of the <see cref="KeyValue"/> class.
    /// Initializes a new instance of the <see cref="KeyValue"/> record with a default "=" separator.
    /// </summary>
    /// <param name="key">The key portion of the key-value pair.</param>
    /// <param name="value">The value portion of the key-value pair.</param>
    public KeyValue(string key, string value) : this(key, value, "=")
    {
    }

    /// <summary>
    /// Returns a string representation of the key-value pair.
    /// </summary>
    /// <returns>A string in the format "Key{Separator}Value".</returns>
    public override string ToString()
    {
        return $"{Key}{Separator}{Value}";
    }

    /// <summary>
    /// Implicitly converts a tuple to a <see cref="KeyValue"/> with default separator.
    /// </summary>
    /// <param name="stringTuple">The tuple containing key and value.</param>
    public static implicit operator KeyValue((string Key, string Value) stringTuple) => new(stringTuple.Key, stringTuple.Value);

    /// <summary>
    /// Implicitly converts a <see cref="Tuple{T1, T2}"/> to a <see cref="KeyValue"/> with default separator.
    /// </summary>
    /// <param name="tuple">The tuple containing key and value.</param>
    public static implicit operator KeyValue(Tuple<string, string> tuple) => new(tuple.Item1, tuple.Item2);

    /// <summary>
    /// Implicitly converts a <see cref="KeyValuePair{TKey, TValue}"/> to a <see cref="KeyValue"/> with default separator.
    /// </summary>
    /// <param name="tuple">The key-value pair.</param>
    public static implicit operator KeyValue(KeyValuePair<string, string> tuple) => new(tuple.Key, tuple.Value);
}