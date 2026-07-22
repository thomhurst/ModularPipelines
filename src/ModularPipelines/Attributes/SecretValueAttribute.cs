namespace ModularPipelines.Attributes;

/// <summary>
/// Marks a property as containing sensitive information that should be obfuscated in logs.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class SecretValueAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance that marks the entire property value as sensitive.
    /// </summary>
    public SecretValueAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance that marks values for the specified keys as sensitive.
    /// </summary>
    /// <param name="keys">Keys whose associated values contain secrets.</param>
    public SecretValueAttribute(params string[] keys)
    {
        Keys = keys;
    }

    /// <summary>
    /// Gets the keys whose values are sensitive when the annotated property contains key-value pairs.
    /// An empty collection means the entire property value is sensitive.
    /// </summary>
    public IReadOnlyList<string> Keys { get; } = [];
}
