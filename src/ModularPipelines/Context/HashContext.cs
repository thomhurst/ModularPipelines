using System.Security.Cryptography;
using System.Text;
using ModularPipelines.FileSystem;

namespace ModularPipelines.Context;

internal class HashContext(
    IHexContext hex,
    IBase64Context base64,
    IFileSystemProvider fileSystemProvider,
    PipelineWorkingDirectory workingDirectory) : IHashContext
{
    public string Md5(string text, HashEncoding encoding = HashEncoding.Hex) =>
        Encode(MD5.HashData(Encoding.UTF8.GetBytes(text)), encoding);

    public string Md5File(string path, HashEncoding encoding = HashEncoding.Hex) =>
        HashFile(path, encoding, MD5.HashData);

    public string Sha1(string text, HashEncoding encoding = HashEncoding.Hex) =>
        Encode(SHA1.HashData(Encoding.UTF8.GetBytes(text)), encoding);

    public string Sha1File(string path, HashEncoding encoding = HashEncoding.Hex) =>
        HashFile(path, encoding, SHA1.HashData);

    public string Sha256(string text, HashEncoding encoding = HashEncoding.Hex) =>
        Encode(SHA256.HashData(Encoding.UTF8.GetBytes(text)), encoding);

    public string Sha256File(string path, HashEncoding encoding = HashEncoding.Hex) =>
        HashFile(path, encoding, SHA256.HashData);

    public string Sha384(string text, HashEncoding encoding = HashEncoding.Hex) =>
        Encode(SHA384.HashData(Encoding.UTF8.GetBytes(text)), encoding);

    public string Sha384File(string path, HashEncoding encoding = HashEncoding.Hex) =>
        HashFile(path, encoding, SHA384.HashData);

    public string Sha512(string text, HashEncoding encoding = HashEncoding.Hex) =>
        Encode(SHA512.HashData(Encoding.UTF8.GetBytes(text)), encoding);

    public string Sha512File(string path, HashEncoding encoding = HashEncoding.Hex) =>
        HashFile(path, encoding, SHA512.HashData);

    private string HashFile(
        string path,
        HashEncoding encoding,
        Func<Stream, byte[]> hash)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new FileNotFoundException($"Cannot calculate hash: file not found at '{path}'", path);
        }

        path = workingDirectory.ResolvePath(path);
        if (!fileSystemProvider.FileExists(path))
        {
            throw new FileNotFoundException($"Cannot calculate hash: file not found at '{path}'", path);
        }

        using var stream = fileSystemProvider.OpenRead(path);
        return Encode(hash(stream), encoding);
    }

    private string Encode(byte[] bytes, HashEncoding encoding) =>
        encoding switch
        {
            HashEncoding.Hex => hex.ToHex(bytes),
            HashEncoding.Base64 => base64.ToBase64String(bytes),
            _ => throw new ArgumentOutOfRangeException(nameof(encoding), encoding, null),
        };
}
