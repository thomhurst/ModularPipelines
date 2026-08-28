using System.Net;
using System.Text;
using System.Text.Json;
using CliWrap.Builders;

namespace ModularPipelines.Secrets;

/// <summary>
/// Generates common textual representations of a secret that may appear in logs.
/// </summary>
internal static class SecretMaskingPatternGenerator
{
    public static IReadOnlyList<string> Generate(string secret)
    {
        var jsonEscaped = JsonEncodedText.Encode(secret).ToString();
        var patterns = new HashSet<string>(StringComparer.Ordinal)
        {
            secret,
            Convert.ToBase64String(Encoding.UTF8.GetBytes(secret)),
            Uri.EscapeDataString(secret),
            WebUtility.UrlEncode(secret),
            jsonEscaped,
        };

        AddArgumentEscapedPatterns(patterns, secret);

        var commandScriptEscaped = secret.Replace("\"", "\"\"");
        patterns.Add(commandScriptEscaped);
        AddArgumentEscapedPatterns(patterns, commandScriptEscaped);

        return patterns.ToArray();
    }

    private static void AddArgumentEscapedPatterns(ISet<string> patterns, string secret)
    {
        var escaped = ArgumentsBuilder.Escape(secret);
        patterns.Add(escaped);

        if (escaped.Length >= 2 && escaped[0] == '"' && escaped[^1] == '"')
        {
            patterns.Add(escaped[1..^1]);
        }
    }
}
