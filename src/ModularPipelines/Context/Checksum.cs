using System.Security.Cryptography;
using ModularPipelines.Context.Domains.Files;

namespace ModularPipelines.Context;

internal class Checksum : IChecksum, IChecksumContext
{
    public string Md5(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Cannot calculate MD5 checksum: file not found at '{filePath}'", filePath);
        }

        using var md5 = MD5.Create();
        using var stream = File.OpenRead(filePath);
        var hash = md5.ComputeHash(stream);
        return BitConverter.ToString(hash).Replace("-", string.Empty);
    }
}
