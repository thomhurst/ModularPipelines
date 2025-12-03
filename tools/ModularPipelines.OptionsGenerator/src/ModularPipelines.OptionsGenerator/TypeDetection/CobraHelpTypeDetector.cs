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
            // Try to find this option in the help text
            var match = FindCobraOptionMatch(helpText, optionName);
            if (match is not null)
            {
                var typeHint = match.Groups["type"].Value.Trim();
                var detectedType = MapCobraTypeToCliType(typeHint);

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
                if (IsValidTypeHint(typeHint))
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

    private static bool IsValidTypeHint(string typeHint)
    {
        if (string.IsNullOrWhiteSpace(typeHint))
            return true; // Empty is valid (boolean)

        // Known Cobra type hints
        var knownTypes = new[]
        {
            "string", "int", "int32", "int64", "uint", "uint32", "uint64",
            "float", "float32", "float64", "bool", "boolean",
            "list", "array", "strings", "stringarray", "stringslice",
            "map", "stringtostring",
            "duration", "bytes"
        };

        return knownTypes.Contains(typeHint.ToLowerInvariant());
    }

    private static CliOptionType MapCobraTypeToCliType(string cobraType)
    {
        var normalized = cobraType.Trim().ToLowerInvariant();

        return normalized switch
        {
            // Empty means boolean flag
            "" => CliOptionType.Bool,

            // Explicit boolean
            "bool" or "boolean" => CliOptionType.Bool,

            // Integer types
            "int" or "int32" or "int64" or "uint" or "uint32" or "uint64" => CliOptionType.Int,

            // Float types
            "float" or "float32" or "float64" => CliOptionType.Decimal,

            // Array/list types
            "list" or "array" or "strings" or "stringarray" or "stringslice" => CliOptionType.StringList,

            // Map types
            "map" or "stringtostring" => CliOptionType.KeyValue,

            // Duration and other string-like types
            "duration" or "string" or "bytes" => CliOptionType.String,

            // Default to string for unknown types
            _ => CliOptionType.String
        };
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
}
