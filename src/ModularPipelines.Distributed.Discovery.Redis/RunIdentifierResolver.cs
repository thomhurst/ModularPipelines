using System.Security.Cryptography;
using System.Text;

namespace ModularPipelines.Distributed.Discovery.Redis;

/// <summary>
/// Resolves a run identifier for isolating concurrent pipeline runs in Redis.
/// </summary>
internal static class RunIdentifierResolver
{
    public static string Resolve(string? configured)
    {
        if (!string.IsNullOrWhiteSpace(configured))
        {
            return configured;
        }

        // Fall back to a hash of the current working directory
        var cwd = Environment.CurrentDirectory;
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(cwd));
        return Convert.ToHexString(hash)[..12].ToLowerInvariant();
    }
}
