namespace ModularPipelines.Attributes;

/// <summary>
/// Specifies the string value representation of an enum field.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class EnumValueAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumValueAttribute"/> class.
    /// </summary>
    /// <param name="value">The string value representation of the enum field.</param>
    public EnumValueAttribute(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the string value representation of the enum field.
    /// </summary>
    public string Value { get; }
}
