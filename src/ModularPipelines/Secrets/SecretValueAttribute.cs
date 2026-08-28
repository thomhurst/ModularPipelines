namespace ModularPipelines.Secrets;

/// <summary>
/// Marks a property as containing sensitive information that should be obfuscated in logs.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class SecretValueAttribute : Attribute
{
    /// <summary>
    /// Initialises a new instance of the <see cref="SecretValueAttribute"/> class
    /// that marks the entire property value as sensitive.
    /// </summary>
    public SecretValueAttribute()
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="SecretValueAttribute"/> class
    /// that marks values for the specified keys as sensitive.
    /// </summary>
    /// <param name="keys">Keys whose associated values contain secrets.</param>
    public SecretValueAttribute(params string[] keys)
    {
        ArgumentNullException.ThrowIfNull(keys);
        Keys = Array.AsReadOnly([.. keys]);
    }

    /// <summary>
    /// Gets the keys whose values are sensitive when the annotated property contains key-value pairs.
    /// Keys also match complete identifier segments separated by dots, underscores, hyphens, or casing changes.
    /// An empty collection means the entire property value is sensitive.
    /// </summary>
    public IReadOnlyList<string> Keys { get; } = [];
}
