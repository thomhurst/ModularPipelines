using ModularPipelines.Helpers;

namespace ModularPipelines.Context;

/// <summary>
/// Provides access to encoding and hashing helpers.
/// </summary>
internal class EncodingContext : IEncodingContext
{
    /// <inheritdoc />
    public IBase64 Base64 { get; }

    /// <inheritdoc />
    public IHex Hex { get; }

    /// <inheritdoc />
    public IHasher Hasher { get; }

    /// <inheritdoc />
    public IChecksum Checksum { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EncodingContext"/> class.
    /// </summary>
    public EncodingContext(IBase64 base64, IHex hex, IHasher hasher, IChecksum checksum)
    {
        Base64 = base64;
        Hex = hex;
        Hasher = hasher;
        Checksum = checksum;
    }
}
