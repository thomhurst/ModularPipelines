namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// Represents the detected type of a CLI option.
/// </summary>
public enum CliOptionType
{
    /// <summary>
    /// Type could not be determined.
    /// </summary>
    Unknown,

    /// <summary>
    /// Boolean flag - no value required, presence/absence is the value.
    /// </summary>
    Bool,

    /// <summary>
    /// String value.
    /// </summary>
    String,

    /// <summary>
    /// Integer value.
    /// </summary>
    Int,

    /// <summary>
    /// Decimal/floating point value.
    /// </summary>
    Decimal,

    /// <summary>
    /// List of strings (can be specified multiple times).
    /// </summary>
    StringList,

    /// <summary>
    /// Key-value pairs (map type).
    /// </summary>
    KeyValue,

    /// <summary>
    /// Enumeration type with a fixed set of valid values.
    /// </summary>
    Enum,

    /// <summary>
    /// Signed 64-bit integer value.
    /// </summary>
    Long,

    /// <summary>
    /// Unsigned 32-bit integer value.
    /// </summary>
    UInt,

    /// <summary>
    /// Unsigned 64-bit integer value.
    /// </summary>
    ULong
}
