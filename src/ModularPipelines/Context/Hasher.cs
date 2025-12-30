using System.Security.Cryptography;
using System.Text;

namespace ModularPipelines.Context;

/// <summary>
/// Provides hashing operations using various algorithms.
/// </summary>
/// <remarks>
/// HashAlgorithm instances are not thread-safe, so we use ThreadLocal to cache
/// one instance per thread. This avoids the allocation overhead of creating new
/// instances on every call while maintaining thread safety.
/// </remarks>
internal class Hasher : IHasher
{
    private readonly IHex _hex;
    private readonly IBase64 _base64;

    // ThreadLocal instances for thread-safe caching of hash algorithms
    // HashAlgorithm is not thread-safe, so each thread gets its own instance
    private static readonly ThreadLocal<SHA1> Sha1Algorithm = new(() => SHA1.Create());
    private static readonly ThreadLocal<SHA256> Sha256Algorithm = new(() => SHA256.Create());
    private static readonly ThreadLocal<SHA384> Sha384Algorithm = new(() => SHA384.Create());
    private static readonly ThreadLocal<SHA512> Sha512Algorithm = new(() => SHA512.Create());
    private static readonly ThreadLocal<MD5> Md5Algorithm = new(() => MD5.Create());

    public Hasher(IHex hex, IBase64 base64)
    {
        _hex = hex;
        _base64 = base64;
    }

    public string Sha1(string input, HashType hashType = HashType.Hex)
    {
        return ComputeHash(Sha1Algorithm.Value!, input, hashType);
    }

    public string Sha256(string input, HashType hashType = HashType.Hex)
    {
        return ComputeHash(Sha256Algorithm.Value!, input, hashType);
    }

    public string Sha384(string input, HashType hashType = HashType.Hex)
    {
        return ComputeHash(Sha384Algorithm.Value!, input, hashType);
    }

    public string Sha512(string input, HashType hashType = HashType.Hex)
    {
        return ComputeHash(Sha512Algorithm.Value!, input, hashType);
    }

    public string Md5(string input, HashType hashType = HashType.Hex)
    {
        return ComputeHash(Md5Algorithm.Value!, input, hashType);
    }

    private string ComputeHash(HashAlgorithm hashAlgorithm, string input, HashType hashType)
    {
        var bytes = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

        return hashType == HashType.Hex ? _hex.ToHex(bytes) : _base64.ToBase64String(bytes);
    }
}