using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// Type detector for Cobra-based CLIs (Docker, Helm, kubectl).
/// Cobra CLI format shows type after the flag name:
///   --name string     (string type)
///   --verbose         (boolean - no type shown)
///   --volume list     (list/array type)
///   --env stringArray (array type)
///   --labels map      (key-value type)
/// </summary>
public partial class CobraHelpTypeDetector : HelpTextTypeDetectorBase
{
    private static readonly string[] SupportedTools = ["docker", "helm", "kubectl"];

    public override int Priority => 100; // Primary automated detector
    public override string Name => "CobraHelp";

    public CobraHelpTypeDetector(ICliCommandExecutor executor, ILogger<CobraHelpTypeDetector> logger)
        : base(executor, logger)
    {
    }

    public override bool CanHandle(string toolName)
    {
        return SupportedTools.Contains(toolName.ToLowerInvariant());
    }

    protected override OptionTypeDetectionResult ParseOptionFromHelpText(
        string helpText,
        OptionDetectionContext context)
    {
        // Cobra format patterns:
        // -s, --long-name type    Description
        // --long-name type        Description
        // -s, --long-name         Description (boolean - no type)

        foreach (var optionName in context.AllNames)
        {
            // Find the option line in help text
            var optionLine = FindCobraOptionLine(helpText, optionName);
            if (optionLine is null)
            {
                continue;
            }

            // PRIORITY 1: Check for enum patterns in description
            // Patterns like: "One of: json|yaml|table" or "(json, yaml, table)"
            var enumResult = TryDetectEnumValues(optionLine, optionName);
            if (enumResult is not null)
            {
                return enumResult;
            }

            // PRIORITY 2: Try standard Cobra type detection
            var match = FindCobraOptionMatch(helpText, optionName);
            if (match is not null)
            {
                var typeHint = match.Groups["type"].Value.Trim();
                var detectedType = CliTypeMapper.FromCobraTypeHint(typeHint);

                Logger.LogDebug("Detected {Option} as {Type} (Cobra type hint: '{Hint}')",
                    optionName, detectedType, typeHint);

                return new OptionTypeDetectionResult
                {
                    Type = detectedType,
                    Confidence = string.IsNullOrEmpty(typeHint) ? 85 : 95,
                    Source = Name,
                    Notes = string.IsNullOrEmpty(typeHint)
                        ? "No type hint - assumed boolean flag"
                        : $"Cobra type hint: {typeHint}"
                };
            }
        }

        return OptionTypeDetectionResult.Unknown(Name);
    }

    /// <summary>
    /// Tries to detect enum values from the option line.
    /// </summary>
    private OptionTypeDetectionResult? TryDetectEnumValues(string optionLine, string optionName)
    {
        // Pattern 1: "One of: json|yaml|table" or "one of json, yaml, table"
        var oneOfMatch = OneOfPattern().Match(optionLine);
        if (oneOfMatch.Success)
        {
            var values = ParseEnumValues(oneOfMatch.Groups["values"].Value);
            if (values.Length > 0)
            {
                Logger.LogDebug("Detected {Option} as Enum (One of: {Values})", optionName, string.Join(", ", values));
                return new OptionTypeDetectionResult
                {
                    Type = CliOptionType.Enum,
                    Confidence = 95,
                    Source = Name,
                    Notes = $"Detected via 'One of' pattern: {string.Join(", ", values)}",
                    EnumValues = values
                };
            }
        }

        // Pattern 2: "(json, yaml, table)" or "(json|yaml|table)" in parentheses
        var parenMatch = ParenthesizedValuesPattern().Match(optionLine);
        if (parenMatch.Success)
        {
            var values = ParseEnumValues(parenMatch.Groups["values"].Value);
            // Only treat as enum if we have 2-10 values and they look like identifiers
            if (values.Length >= 2 && values.Length <= 10 && values.All(v => IdentifierPattern().IsMatch(v)))
            {
                Logger.LogDebug("Detected {Option} as Enum (parenthesized: {Values})", optionName, string.Join(", ", values));
                return new OptionTypeDetectionResult
                {
                    Type = CliOptionType.Enum,
                    Confidence = 85,
                    Source = Name,
                    Notes = $"Detected via parenthesized values: {string.Join(", ", values)}",
                    EnumValues = values
                };
            }
        }

        // Pattern 3: "Must be one of" or "Valid values:"
        var validValuesMatch = ValidValuesPattern().Match(optionLine);
        if (validValuesMatch.Success)
        {
            var values = ParseEnumValues(validValuesMatch.Groups["values"].Value);
            if (values.Length > 0)
            {
                Logger.LogDebug("Detected {Option} as Enum (Valid values: {Values})", optionName, string.Join(", ", values));
                return new OptionTypeDetectionResult
                {
                    Type = CliOptionType.Enum,
                    Confidence = 95,
                    Source = Name,
                    Notes = $"Detected via 'Valid values' pattern: {string.Join(", ", values)}",
                    EnumValues = values
                };
            }
        }

        return null;
    }

    /// <summary>
    /// Parses enum values from a string like "json|yaml|table" or "json, yaml, table".
    /// </summary>
    private static string[] ParseEnumValues(string valuesText)
    {
        // Split on common separators: |, comma, "or", "and"
        var values = valuesText
            .Replace(" or ", "|")
            .Replace(" and ", "|")
            .Split(['|', ','], StringSplitOptions.RemoveEmptyEntries)
            .Select(v => v.Trim().Trim('"', '\'', '`'))
            .Where(v => !string.IsNullOrWhiteSpace(v) && v.Length < 30) // Reasonable length
            .Distinct()
            .ToArray();

        return values;
    }

    /// <summary>
    /// Finds the full line(s) in help text that describe an option.
    /// </summary>
    private static string? FindCobraOptionLine(string helpText, string optionName)
    {
        var lines = helpText.Split('\n');
        var escapedName = Regex.Escape(optionName);

        for (var i = 0; i < lines.Length; i++)
        {
            // Match the option at word boundary
            if (Regex.IsMatch(lines[i], $@"(^|\s|,){escapedName}(\s|,|=|$)", RegexOptions.IgnoreCase))
            {
                // Include continuation lines (indented lines that follow)
                var fullLine = lines[i];
                var j = i + 1;
                while (j < lines.Length && lines[j].StartsWith("                    "))
                {
                    fullLine += " " + lines[j].Trim();
                    j++;
                }
                return fullLine;
            }
        }

        return null;
    }

    private Match? FindCobraOptionMatch(string helpText, string optionName)
    {
        // Pattern to match Cobra-style option definitions
        // Handles: -s, --long type   OR  --long type  OR  -s, --long  OR  --long
        var escapedName = Regex.Escape(optionName);

        // Try patterns in order of specificity
        var patterns = new[]
        {
            // -s, --long-name type   Description
            $@"^\s*(?:-\w,\s*)?{escapedName}\s+(?<type>\S+)\s+",
            // --long-name    (no type = boolean)
            $@"^\s*(?:-\w,\s*)?{escapedName}\s*(?<type>)(?:\s{{2,}}|$)"
        };

        foreach (var pattern in patterns)
        {
            var regex = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            var match = regex.Match(helpText);
            if (match.Success)
            {
                // Validate that we're not matching description text as type
                var typeHint = match.Groups["type"].Value.Trim();
                if (CliTypeMapper.IsKnownCobraType(typeHint))
                {
                    return match;
                }
            }
        }

        // Fallback: line-by-line search
        var lines = helpText.Split('\n');
        foreach (var line in lines)
        {
            if (line.Contains(optionName, StringComparison.OrdinalIgnoreCase))
            {
                var lineMatch = CobraOptionLinePattern().Match(line);
                if (lineMatch.Success)
                {
                    var shortFlag = lineMatch.Groups["short"].Value;
                    var longFlag = lineMatch.Groups["long"].Value;

                    // Check if this line is for our option
                    if (longFlag.Equals(optionName, StringComparison.OrdinalIgnoreCase) ||
                        shortFlag.Equals(optionName, StringComparison.OrdinalIgnoreCase))
                    {
                        return lineMatch;
                    }
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Matches a Cobra-style option line.
    /// Examples:
    ///   -d, --detach                 Run container in background
    ///   -e, --env list               Set environment variables
    ///       --name string            Assign a name to the container
    /// </summary>
    [GeneratedRegex(@"^\s*(?:(?<short>-\w),\s*)?(?<long>--[\w-]+)(?:\s+(?<type>\S+))?\s{2,}", RegexOptions.Multiline)]
    private static partial Regex CobraOptionLinePattern();

    /// <summary>
    /// Matches "One of: value1|value2" or "one of value1, value2" patterns.
    /// </summary>
    [GeneratedRegex(@"[Oo]ne of[:\s]+(?<values>[\w\-|,\s""'`]+?)(?:\s*\(|$|\.|;)", RegexOptions.IgnoreCase)]
    private static partial Regex OneOfPattern();

    /// <summary>
    /// Matches "(value1, value2, value3)" or "(value1|value2|value3)" patterns.
    /// </summary>
    [GeneratedRegex(@"\((?<values>[\w\-]+(?:\s*[|,]\s*[\w\-]+)+)\)")]
    private static partial Regex ParenthesizedValuesPattern();

    /// <summary>
    /// Matches "Valid values:" or "Must be one of:" patterns.
    /// </summary>
    [GeneratedRegex(@"(?:Valid values|Must be one of|Allowed values)[:\s]+(?<values>[\w\-|,\s""'`]+?)(?:\s*\(|$|\.|;)", RegexOptions.IgnoreCase)]
    private static partial Regex ValidValuesPattern();

    /// <summary>
    /// Matches valid identifier-like strings for enum values.
    /// </summary>
    [GeneratedRegex(@"^[\w][\w\-]*$")]
    private static partial Regex IdentifierPattern();
}
