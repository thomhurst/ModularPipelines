namespace ModularPipelines.Context.Domains.Security;

/// <summary>
/// Specifies the text encoding used for a hash result.
/// </summary>
public enum HashEncoding
{
    /// <summary>
    /// Lowercase hexadecimal text.
    /// </summary>
    Hex,

    /// <summary>
    /// Base64 text.
    /// </summary>
    Base64,
}
