namespace ModularPipelines.Attributes;

/// <summary>
/// Specifies the format for CLI option values.
/// </summary>
public enum OptionFormat
{
    /// <summary>
    /// Space-separated format: --option value.
    /// </summary>
    SpaceSeparated,

    /// <summary>
    /// Equals-separated format: --option=value.
    /// </summary>
    EqualsSeparated,

    /// <summary>
    /// Colon-separated format: --option:value.
    /// </summary>
    ColonSeparated,

    /// <summary>
    /// No separator, value directly follows option: --optionvalue.
    /// </summary>
    NoSeparator,
}
