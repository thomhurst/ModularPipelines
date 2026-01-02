using ModularPipelines.Helpers;

namespace ModularPipelines.Context;

/// <summary>
/// Provides access to encoding and hashing helpers.
/// </summary>
/// <remarks>
/// This context groups related encoding services (Base64, Hex, Hasher)
/// to reduce constructor parameter count in PipelineContext while maintaining the same public API.
/// Note: Checksum is in IPipelineFileSystem as it operates on files.
/// </remarks>
public interface IEncodingContext
{
    /// <summary>
    /// Gets helpers for converting to and from Base64 strings.
    /// </summary>
    IBase64 Base64 { get; }

    /// <summary>
    /// Gets helpers for converting to and from hex strings.
    /// </summary>
    IHex Hex { get; }

    /// <summary>
    /// Gets helpers for hashing data.
    /// </summary>
    /// <remarks>
    /// Supports MD5, SHA1, SHA256, SHA512 hashing algorithms.
    /// </remarks>
    IHasher Hasher { get; }
}
