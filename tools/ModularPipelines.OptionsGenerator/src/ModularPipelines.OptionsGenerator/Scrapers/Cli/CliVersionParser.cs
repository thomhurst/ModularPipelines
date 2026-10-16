using System.Text.RegularExpressions;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// Extracts release identities from version headers without retaining environment metadata.
/// </summary>
internal static partial class CliVersionParser
{
    private const string VersionRegex = @"v?(?<version>\d+(?:\.\d+)+(?:-[0-9A-Za-z]+(?:[.-][0-9A-Za-z]+)*)?(?:\+[0-9A-Za-z]+(?:[.-][0-9A-Za-z]+)*)?)";

    public static string? Parse(CliCommandResult result, string headerPattern)
    {
        var pattern = new Regex(
            $@"^(?:(?:{headerPattern}){VersionRegex}(?=$|[\s,\]}})""])|{VersionRegex}$)",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant,
            TimeSpan.FromSeconds(1));

        return ParseStream(result.StandardOutput, pattern) ?? ParseStream(result.StandardError, pattern);
    }

    private static string? ParseStream(string output, Regex pattern)
    {
        using var reader = new StringReader(output);
        while (reader.ReadLine() is { } line)
        {
            var match = pattern.Match(AnsiEscapeSequencePattern().Replace(line, string.Empty).Trim());
            if (match.Success)
            {
                return match.Groups["version"].Value;
            }
        }

        return null;
    }

    [GeneratedRegex(@"\x1B(?:\][^\x07\x1B]*(?:\x07|\x1B\\)|\[[0-?]*[ -/]*[@-~])")]
    private static partial Regex AnsiEscapeSequencePattern();
}
