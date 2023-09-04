using System.Security.Cryptography;

namespace ModularPipelines.Context;

internal class Checksum : IChecksum
{
    public string Md5(string filePath)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(filePath);
        var hash = md5.ComputeHash(stream);
        return BitConverter.ToString(hash).Replace("-", String.Empty);
    }
}