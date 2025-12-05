using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// Type detector for .NET CLI commands.
/// DotNet CLI format uses different patterns:
///   --no-restore     [default: False]  (boolean)
///   --framework, -f  &lt;FRAMEWORK&gt;       (string with placeholder)
///   --verbosity, -v  &lt;LEVEL&gt;           (string/enum)
/// </summary>
public partial class DotNetHelpTypeDetector : HelpTextTypeDetectorBase
{
    public override int Priority => 100;
    public override string Name => "DotNetHelp";

    public DotNetHelpTypeDetector(ICliCommandExecutor executor, ILogger<DotNetHelpTypeDetector> logger)
        : base(executor, logger)
    {
    }

    public override bool CanHandle(string toolName)
    {
        return toolName.Equals("dotnet", StringComparison.OrdinalIgnoreCase);
    }

    protected override OptionTypeDetectionResult ParseOptionFromHelpText(
        string helpText,
        OptionDetectionContext context)
    {
        foreach (var optionName in context.AllNames)
        {
            // Find the line containing this option
            var optionLine = FindDotNetOptionLine(helpText, optionName);
            if (optionLine is null)
                continue;

            // PRIORITY 1: Check for "Allowed values" pattern - indicates enum
            // Pattern: "Allowed values are q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic]"
            var allowedValuesMatch = AllowedValuesPattern().Match(optionLine);
            if (allowedValuesMatch.Success)
            {
                var valuesText = allowedValuesMatch.Groups["values"].Value;
                Logger.LogDebug("Detected {Option} as Enum (Allowed values: {Values})", optionName, valuesText);
                return new OptionTypeDetectionResult
                {
                    Type = CliOptionType.Enum,
                    Confidence = 98,
                    Source = Name,
                    Notes = $"Detected via 'Allowed values' pattern: {valuesText}",
                    EnumValues = ParseAllowedValues(valuesText)
                };
            }

            // PRIORITY 2: Check for [default: False] or [default: True] - indicates boolean
            if (DefaultBoolPattern().IsMatch(optionLine))
            {
                Logger.LogDebug("Detected {Option} as Bool (default value pattern)", optionName);
                return new OptionTypeDetectionResult
                {
                    Type = CliOptionType.Bool,
                    Confidence = 95,
                    Source = Name,
                    Notes = "Detected via [default: True/False] pattern"
                };
            }

            // PRIORITY 3: Check for <PLACEHOLDER> pattern - indicates string value
            var placeholderMatch = PlaceholderPattern().Match(optionLine);
            if (placeholderMatch.Success)
            {
                var placeholder = placeholderMatch.Groups["placeholder"].Value;
                var detectedType = InferTypeFromPlaceholder(placeholder);

                Logger.LogDebug("Detected {Option} as {Type} (placeholder: {Placeholder})",
                    optionName, detectedType, placeholder);

                return new OptionTypeDetectionResult
                {
                    Type = detectedType,
                    Confidence = 90,
                    Source = Name,
                    Notes = $"Inferred from placeholder: <{placeholder}>"
                };
            }

            // PRIORITY 4: Check for numeric default value
            var defaultMatch = DefaultValuePattern().Match(optionLine);
            if (defaultMatch.Success)
            {
                var defaultValue = defaultMatch.Groups["value"].Value;
                if (int.TryParse(defaultValue, out _))
                {
                    Logger.LogDebug("Detected {Option} as Int (numeric default: {Default})",
                        optionName, defaultValue);
                    return new OptionTypeDetectionResult
                    {
                        Type = CliOptionType.Int,
                        Confidence = 90,
                        Source = Name,
                        Notes = $"Numeric default value: {defaultValue}"
                    };
                }
            }

            // PRIORITY 5: No type indicators found - check if it looks like a flag
            // (options with no value placeholder and no default are usually flags)
            if (!optionLine.Contains('<') && !optionLine.Contains('['))
            {
                Logger.LogDebug("Detected {Option} as Bool (no value placeholder)", optionName);
                return new OptionTypeDetectionResult
                {
                    Type = CliOptionType.Bool,
                    Confidence = 70,
                    Source = Name,
                    Notes = "Assumed boolean - no value placeholder or default"
                };
            }
        }

        return OptionTypeDetectionResult.Unknown(Name);
    }

    /// <summary>
    /// Parses "Allowed values" text to extract enum values.
    /// Handles formats like: "q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic]"
    /// </summary>
    private static string[] ParseAllowedValues(string valuesText)
    {
        // Remove "and" and split by comma or whitespace
        var cleaned = valuesText
            .Replace(" and ", ", ")
            .Replace(" or ", ", ");

        // Extract values - handle bracket notation like "q[uiet]" -> "quiet"
        var values = new List<string>();
        var parts = cleaned.Split([',', ' '], StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in parts)
        {
            var trimmed = part.Trim().Trim('.');

            // Handle bracket notation: "q[uiet]" -> "quiet"
            var bracketMatch = BracketNotationPattern().Match(trimmed);
            if (bracketMatch.Success)
            {
                var fullValue = bracketMatch.Groups["prefix"].Value + bracketMatch.Groups["suffix"].Value;
                values.Add(fullValue.ToLowerInvariant());
            }
            else if (!string.IsNullOrWhiteSpace(trimmed) && trimmed.Length > 0)
            {
                values.Add(trimmed.ToLowerInvariant());
            }
        }

        return values.Distinct().ToArray();
    }

    private static string? FindDotNetOptionLine(string helpText, string optionName)
    {
        var lines = helpText.Split('\n');
        var escapedName = Regex.Escape(optionName);

        // Look for lines containing this option
        foreach (var line in lines)
        {
            // Match the option at word boundary
            if (Regex.IsMatch(line, $@"(^|\s|,){escapedName}(\s|,|$)", RegexOptions.IgnoreCase))
            {
                return line;
            }
        }

        return null;
    }

    private static CliOptionType InferTypeFromPlaceholder(string placeholder)
    {
        var normalized = placeholder.ToUpperInvariant();

        return normalized switch
        {
            // Numeric placeholders - Note: LEVEL removed since it's often enum (e.g., verbosity)
            "NUMBER" or "COUNT" or "N" or "PORT" or "SIZE" => CliOptionType.Int,

            // Level/verbosity are typically enums or strings, not ints
            "LEVEL" or "VERBOSITY" => CliOptionType.String,

            // Common string placeholders
            "PATH" or "FILE" or "DIRECTORY" or "DIR" or "FOLDER" => CliOptionType.String,
            "NAME" or "VALUE" or "VERSION" or "URL" or "URI" => CliOptionType.String,
            "FRAMEWORK" or "RUNTIME" or "RID" or "TFM" => CliOptionType.String,
            "CONFIGURATION" or "CONFIG" => CliOptionType.String,
            "OUTPUT" or "INPUT" or "SOURCE" or "TARGET" => CliOptionType.String,
            "FORMAT" => CliOptionType.String, // Often enum, but string is safer default

            // Array-like indicators
            _ when normalized.EndsWith("S") && normalized.Length > 2 => CliOptionType.String, // Could be array
            _ when normalized.Contains(":") => CliOptionType.KeyValue, // key:value format

            // Default to string
            _ => CliOptionType.String
        };
    }

    /// <summary>
    /// Matches [default: True] or [default: False] patterns.
    /// </summary>
    [GeneratedRegex(@"\[default:\s*(True|False)\]", RegexOptions.IgnoreCase)]
    private static partial Regex DefaultBoolPattern();

    /// <summary>
    /// Matches &lt;PLACEHOLDER&gt; patterns in option descriptions.
    /// </summary>
    [GeneratedRegex(@"<(?<placeholder>[^>]+)>")]
    private static partial Regex PlaceholderPattern();

    /// <summary>
    /// Matches [default: value] patterns.
    /// </summary>
    [GeneratedRegex(@"\[default:\s*(?<value>[^\]]+)\]")]
    private static partial Regex DefaultValuePattern();

    /// <summary>
    /// Matches "Allowed values are X, Y, Z" or "Allowed values: X, Y, Z" patterns.
    /// </summary>
    [GeneratedRegex(@"Allowed values[:\s]+(?:are\s+)?(?<values>.+?)(?:\.|$)", RegexOptions.IgnoreCase)]
    private static partial Regex AllowedValuesPattern();

    /// <summary>
    /// Matches bracket notation like "q[uiet]" to extract "quiet".
    /// </summary>
    [GeneratedRegex(@"^(?<prefix>\w+)\[(?<suffix>\w+)\]$")]
    private static partial Regex BracketNotationPattern();
}
