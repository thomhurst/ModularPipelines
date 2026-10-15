using System.Net;
using System.Text;
using System.Text.Json;
using ModularPipelines.Helpers.Internal;

namespace ModularPipelines.Secrets;

/// <summary>
/// Generates common textual representations of a secret that may appear in logs.
/// </summary>
internal static class SecretMaskingPatternGenerator
{
    public static IReadOnlyList<string> Generate(string secret, int minimumSecretLength = 1)
    {
        var patterns = new HashSet<string>(StringComparer.Ordinal);
        AddPatterns(patterns, secret);

        if (secret.Contains('\r') || secret.Contains('\n'))
        {
            var normalized = secret.Replace("\r\n", "\n").Replace('\r', '\n');
            AddPatterns(patterns, normalized);
            AddPatterns(patterns, normalized.Replace("\n", "\r\n"));

            // Command streams and native CI maskers match one line at a time.
            foreach (var line in normalized.Split('\n'))
            {
                if (line.Length >= Math.Max(1, minimumSecretLength) && !string.IsNullOrWhiteSpace(line))
                {
                    AddPatterns(patterns, line);
                }
            }
        }

        return [.. patterns];
    }

    private static void AddPatterns(HashSet<string> patterns, string secret)
    {
        patterns.Add(secret);
        patterns.Add(Convert.ToBase64String(Encoding.UTF8.GetBytes(secret)));
        patterns.Add(Uri.EscapeDataString(secret));
        patterns.Add(WebUtility.UrlEncode(secret));
        patterns.Add(JsonEncodedText.Encode(secret).ToString());
        AddArgumentEscapedPatterns(patterns, secret);

        var commandScriptEscaped = secret.Replace("\"", "\"\"");
        patterns.Add(commandScriptEscaped);
        AddArgumentEscapedPatterns(patterns, commandScriptEscaped);
    }

    private static void AddArgumentEscapedPatterns(HashSet<string> patterns, string secret)
    {
        var escaped = CommandLineArguments.Escape(secret);
        patterns.Add(escaped);

        if (escaped.Length >= 2 && escaped[0] == '"' && escaped[^1] == '"')
        {
            patterns.Add(escaped[1..^1]);
        }
    }
}
