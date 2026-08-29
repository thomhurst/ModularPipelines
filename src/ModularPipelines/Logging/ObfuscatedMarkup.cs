using System.Text;
using System.Text.RegularExpressions;
using ModularPipelines.Engine;
using Spectre.Console;

namespace ModularPipelines.Logging;

internal static partial class ObfuscatedMarkup
{
    internal static Markup Create(string value, ISecretObfuscator secretObfuscator)
    {
        var obfuscatedSource = secretObfuscator.Obfuscate(value, null);
        try
        {
            return new Markup(obfuscatedSource);
        }
        catch (InvalidOperationException) when (!string.Equals(
            obfuscatedSource,
            value,
            StringComparison.Ordinal))
        {
            var tagObfuscatedSource = MarkupTagRegex().Replace(
                value,
                match => secretObfuscator.Obfuscate(match.Value, null));
            if (!string.Equals(tagObfuscatedSource, obfuscatedSource, StringComparison.Ordinal))
            {
                throw;
            }

            return new Markup(ObfuscateText(value, secretObfuscator));
        }
    }

    private static string ObfuscateText(string value, ISecretObfuscator secretObfuscator)
    {
        var output = new StringBuilder(value.Length);
        var sourceOffset = 0;
        foreach (Match match in MarkupTagRegex().Matches(value))
        {
            output.Append(secretObfuscator.Obfuscate(value[sourceOffset..match.Index], null));
            output.Append(match.Value);
            sourceOffset = match.Index + match.Length;
        }

        output.Append(secretObfuscator.Obfuscate(value[sourceOffset..], null));
        return output.ToString();
    }

    [GeneratedRegex(@"\[[^\]\r\n]+\]", RegexOptions.CultureInvariant)]
    private static partial Regex MarkupTagRegex();
}
