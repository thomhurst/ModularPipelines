namespace ModularPipelines.Context;

/// <summary>
/// Computes cryptographic hashes for text and files.
/// </summary>
public interface IHashContext
{
    /// <summary>Computes the MD5 hash of UTF-8 text.</summary>
    /// <param name="text">The text to hash.</param>
    /// <param name="encoding">The result encoding.</param>
    /// <returns>The encoded hash.</returns>
    string Md5(string text, HashEncoding encoding = HashEncoding.Hex);

    /// <summary>Computes the MD5 hash of a file.</summary>
    /// <param name="path">The file path.</param>
    /// <param name="encoding">The result encoding.</param>
    /// <returns>The encoded hash.</returns>
    string Md5File(string path, HashEncoding encoding = HashEncoding.Hex);

    /// <summary>Computes the SHA-1 hash of UTF-8 text.</summary>
    /// <param name="text">The text to hash.</param>
    /// <param name="encoding">The result encoding.</param>
    /// <returns>The encoded hash.</returns>
    string Sha1(string text, HashEncoding encoding = HashEncoding.Hex);

    /// <summary>Computes the SHA-1 hash of a file.</summary>
    /// <param name="path">The file path.</param>
    /// <param name="encoding">The result encoding.</param>
    /// <returns>The encoded hash.</returns>
    string Sha1File(string path, HashEncoding encoding = HashEncoding.Hex);

    /// <summary>Computes the SHA-256 hash of UTF-8 text.</summary>
    /// <param name="text">The text to hash.</param>
    /// <param name="encoding">The result encoding.</param>
    /// <returns>The encoded hash.</returns>
    string Sha256(string text, HashEncoding encoding = HashEncoding.Hex);

    /// <summary>Computes the SHA-256 hash of a file.</summary>
    /// <param name="path">The file path.</param>
    /// <param name="encoding">The result encoding.</param>
    /// <returns>The encoded hash.</returns>
    string Sha256File(string path, HashEncoding encoding = HashEncoding.Hex);

    /// <summary>Computes the SHA-384 hash of UTF-8 text.</summary>
    /// <param name="text">The text to hash.</param>
    /// <param name="encoding">The result encoding.</param>
    /// <returns>The encoded hash.</returns>
    string Sha384(string text, HashEncoding encoding = HashEncoding.Hex);

    /// <summary>Computes the SHA-384 hash of a file.</summary>
    /// <param name="path">The file path.</param>
    /// <param name="encoding">The result encoding.</param>
    /// <returns>The encoded hash.</returns>
    string Sha384File(string path, HashEncoding encoding = HashEncoding.Hex);

    /// <summary>Computes the SHA-512 hash of UTF-8 text.</summary>
    /// <param name="text">The text to hash.</param>
    /// <param name="encoding">The result encoding.</param>
    /// <returns>The encoded hash.</returns>
    string Sha512(string text, HashEncoding encoding = HashEncoding.Hex);

    /// <summary>Computes the SHA-512 hash of a file.</summary>
    /// <param name="path">The file path.</param>
    /// <param name="encoding">The result encoding.</param>
    /// <returns>The encoded hash.</returns>
    string Sha512File(string path, HashEncoding encoding = HashEncoding.Hex);
}
