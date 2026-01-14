using System.Security.Cryptography;
using System.Text;
using ModularPipelines.Context.Domains.Security;

namespace ModularPipelines.Context;

/// <summary>
/// Provides hashing operations using various algorithms.
/// </summary>
/// <remarks>
/// Uses the static HashData methods available in .NET 5+ which are thread-safe
/// and don't require disposal, avoiding resource leaks.
/// </remarks>
internal class Hasher : IHasher, IHasherContext
{
    private readonly IHex _hex;
    private readonly IBase64 _base64;

    public Hasher(IHex hex, IBase64 base64)
    {
        _hex = hex;
        _base64 = base64;
    }

    public string Sha1(string input, HashType hashType = HashType.Hex)
    {
        var bytes = System.Security.Cryptography.SHA1.HashData(Encoding.UTF8.GetBytes(input));
        return hashType == HashType.Hex ? _hex.ToHex(bytes) : _base64.ToBase64String(bytes);
    }

    public string Sha256(string input, HashType hashType = HashType.Hex)
    {
        var bytes = System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return hashType == HashType.Hex ? _hex.ToHex(bytes) : _base64.ToBase64String(bytes);
    }

    public string Sha384(string input, HashType hashType = HashType.Hex)
    {
        var bytes = System.Security.Cryptography.SHA384.HashData(Encoding.UTF8.GetBytes(input));
        return hashType == HashType.Hex ? _hex.ToHex(bytes) : _base64.ToBase64String(bytes);
    }

    public string Sha512(string input, HashType hashType = HashType.Hex)
    {
        var bytes = System.Security.Cryptography.SHA512.HashData(Encoding.UTF8.GetBytes(input));
        return hashType == HashType.Hex ? _hex.ToHex(bytes) : _base64.ToBase64String(bytes);
    }

    public string Md5(string input, HashType hashType = HashType.Hex)
    {
        var bytes = System.Security.Cryptography.MD5.HashData(Encoding.UTF8.GetBytes(input));
        return hashType == HashType.Hex ? _hex.ToHex(bytes) : _base64.ToBase64String(bytes);
    }
}