using System.Net;
using System.Text;
using System.Text.Json;

namespace ModularPipelines.Engine;

/// <summary>
/// Generates common textual representations of a secret that may appear in logs.
/// </summary>
internal static class SecretMaskingPatternGenerator
{
    public static IReadOnlyList<string> Generate(string secret)
    {
        var json = JsonSerializer.Serialize(secret);
        var patterns = new HashSet<string>(StringComparer.Ordinal)
        {
            secret,
            Convert.ToBase64String(Encoding.UTF8.GetBytes(secret)),
            Uri.EscapeDataString(secret),
            WebUtility.UrlEncode(secret),
            json[1..^1],
        };

        return patterns.ToArray();
    }
}
