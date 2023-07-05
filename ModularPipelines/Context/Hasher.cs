using System.Security.Cryptography;
using System.Text;

namespace ModularPipelines.Context;

public class Hasher : IHasher
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
        return ComputeHash(SHA1.Create(), input, hashType);
    }

    public string Sha256(string input, HashType hashType = HashType.Hex)
    {
        return ComputeHash(SHA256.Create(), input, hashType);
    }

    public string Sha384(string input, HashType hashType = HashType.Hex)
    {
        return ComputeHash(SHA384.Create(), input, hashType);
    }

    public string Sha512(string input, HashType hashType = HashType.Hex)
    {
        return ComputeHash(SHA512.Create(), input, hashType);
    }

    public string Md5(string input, HashType hashType = HashType.Hex)
    {
        return ComputeHash(MD5.Create(), input, hashType);
    }

    private string ComputeHash(HashAlgorithm hashAlgorithm, string input, HashType hashType)
    {
        using (hashAlgorithm)
        {
            var bytes = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            return hashType == HashType.Hex ? _hex.ToHex(bytes) : _base64.ToBase64String(bytes);
        }
    }
}
