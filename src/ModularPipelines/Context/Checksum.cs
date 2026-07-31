using System.Security.Cryptography;
using ModularPipelines.Context.Domains.Files;
using ModularPipelines.FileSystem;

namespace ModularPipelines.Context;

internal class Checksum(IFileSystemProvider fileSystemProvider) : IChecksumContext
{
    public string Md5(string filePath)
    {
        if (!fileSystemProvider.FileExists(filePath))
        {
            throw new FileNotFoundException($"Cannot calculate MD5 checksum: file not found at '{filePath}'", filePath);
        }

        using var md5 = MD5.Create();
        using var stream = fileSystemProvider.OpenRead(filePath);
        var hash = md5.ComputeHash(stream);
        return Convert.ToHexString(hash);
    }
}
