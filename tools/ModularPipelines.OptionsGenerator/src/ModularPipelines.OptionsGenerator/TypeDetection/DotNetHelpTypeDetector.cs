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

            // Check for [default: False] or [default: True] - indicates boolean
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

            // Check for <PLACEHOLDER> pattern - indicates string value
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

            // Check for numeric default value
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

            // No type indicators found - check if it looks like a flag
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
            // Numeric placeholders
            "NUMBER" or "COUNT" or "N" or "LEVEL" => CliOptionType.Int,
            "VERBOSITY" => CliOptionType.String, // Often an enum in practice

            // Common string placeholders
            "PATH" or "FILE" or "DIRECTORY" or "DIR" or "FOLDER" => CliOptionType.String,
            "NAME" or "VALUE" or "VERSION" or "URL" or "URI" => CliOptionType.String,
            "FRAMEWORK" or "RUNTIME" or "RID" or "TFM" => CliOptionType.String,
            "CONFIGURATION" or "CONFIG" => CliOptionType.String,
            "OUTPUT" or "INPUT" or "SOURCE" or "TARGET" => CliOptionType.String,

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
}
