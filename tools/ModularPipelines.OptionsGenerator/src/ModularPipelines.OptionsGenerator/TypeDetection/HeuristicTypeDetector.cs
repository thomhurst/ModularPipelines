using System.Collections.Frozen;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// Fallback detector that uses heuristics based on option names and descriptions.
/// This is the lowest priority detector, used when all others fail.
/// </summary>
public partial class HeuristicTypeDetector : IOptionTypeDetector
{
    #region Pattern Constants

    /// <summary>
    /// Option name prefixes that typically indicate boolean flags.
    /// These prefixes suggest toggling behavior (enable/disable, include/exclude).
    /// Examples: --no-cache, --enable-feature, --skip-tests
    /// </summary>
    private static readonly string[] BooleanPrefixes =
    [
        "no-", "disable-", "enable-", "with-", "without-",
        "skip-", "force-", "allow-", "deny-", "include-", "exclude-"
    ];

    /// <summary>
    /// Option name suffixes that typically indicate boolean flags.
    /// These suffixes suggest mode modifiers or behavioral toggles.
    /// Examples: --build-only, --recursive, --verbose
    /// </summary>
    private static readonly string[] BooleanSuffixes =
    [
        "-only", "-all", "-recursive", "-verbose", "-quiet", "-silent",
        "-interactive", "-yes", "-no", "-force"
    ];

    /// <summary>
    /// Exact option names that are commonly used as boolean flags across CLI tools.
    /// Includes common flags for verbosity, confirmation, and operational modes.
    /// Uses FrozenSet for O(1) case-insensitive lookup.
    /// </summary>
    private static readonly FrozenSet<string> BooleanExactNames = new[]
    {
        "verbose", "quiet", "silent", "debug", "force", "yes", "no",
        "dry-run", "dryrun", "help", "version", "interactive",
        "detach", "tty", "privileged", "rm", "init", "recursive",
        "all", "cached", "staged", "untracked", "ignored"
    }.ToFrozenSet(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Description text patterns that indicate boolean behavior.
    /// When these phrases appear in an option's description, it suggests a boolean flag.
    /// </summary>
    private static readonly string[] BooleanDescriptionPatterns =
    [
        "enable", "disable", "turn on", "turn off",
        "whether to", "if true", "if set", "when set",
        "flag to", "activate", "deactivate"
    ];

    /// <summary>
    /// Option name patterns that indicate numeric/integer values.
    /// These patterns suggest counts, limits, or other numerical quantities.
    /// Note: "level" is intentionally excluded as it often refers to verbosity levels which are enums/strings.
    /// </summary>
    private static readonly string[] NumericNamePatterns =
    [
        "count", "limit", "max", "min", "length", "depth",
        "retries", "port", "number"
    ];

    /// <summary>
    /// Option name patterns whose values commonly include units.
    /// These remain strings unless stronger evidence, such as a numeric default, proves otherwise.
    /// </summary>
    private static readonly string[] UnitBearingNamePatterns =
    [
        "timeout", "duration", "interval", "size"
    ];

    /// <summary>
    /// Description patterns that explicitly require a bare integer.
    /// These provide stronger evidence than a potentially unit-bearing option name.
    /// </summary>
    private static readonly string[] NumericDescriptionPatterns =
    [
        "integer", "whole number", "number of seconds", "number of milliseconds", "number of bytes"
    ];

    /// <summary>
    /// Option name patterns that indicate list/array values.
    /// These patterns suggest options that can be specified multiple times.
    /// Common in Docker, Kubernetes, and other container-related CLI tools.
    /// </summary>
    private static readonly string[] ListNamePatterns =
    [
        "add-", "-add", "exclude", "include", "label", "env", "volume",
        "mount", "publish", "expose", "cap-", "device", "dns"
    ];

    /// <summary>
    /// Option name patterns that indicate key-value pair options.
    /// These patterns suggest configuration or metadata settings.
    /// </summary>
    private static readonly string[] KeyValueNamePatterns =
    [
        "env", "label", "annotation", "sysctl"
    ];

    /// <summary>
    /// Description patterns that indicate key-value pair format.
    /// When these patterns appear in descriptions, the option likely accepts key=value or key:value format.
    /// </summary>
    private static readonly string[] KeyValueDescriptionPatterns =
    [
        "key=value", "key:value"
    ];

    /// <summary>
    /// Literal values that represent boolean true/false in CLI contexts.
    /// Used to detect boolean options from their accepted values.
    /// </summary>
    private static readonly string[] BooleanLiteralValues =
    [
        "true", "false", "yes", "no", "on", "off", "0", "1"
    ];

    /// <summary>
    /// Description patterns that strongly indicate the option accepts a value (not a boolean).
    /// These patterns override boolean detection when present.
    /// High confidence patterns - explicit value examples or "set to X" language.
    /// </summary>
    private static readonly string[] ValueDescriptionPatternsHighConfidence =
    [
        "set to '",      // "Set to 'hcl2' to run in HCL2 mode"
        "set to \"",     // Same with double quotes
        "defaults to '", // "Defaults to 'json'"
        "defaults to \"",
        "one of:",       // "One of: json, yaml, xml"
        "valid values:", // "Valid values: debug, info, warn"
        "allowed values:",
        "possible values:",
        "accepted values:"
    ];

    /// <summary>
    /// Description patterns that indicate the option can be specified multiple times.
    /// This strongly indicates a value-accepting option, not a boolean.
    /// </summary>
    private static readonly string[] MultiValueDescriptionPatterns =
    [
        "can be used multiple times",
        "may be specified multiple times",
        "can be specified multiple times",
        "can be repeated",
        "may be repeated",
        "multiple times",
        "repeatable"
    ];

    /// <summary>
    /// Description patterns that moderately suggest a value option.
    /// Lower confidence than explicit patterns above.
    /// </summary>
    private static readonly string[] ValueDescriptionPatternsMediumConfidence =
    [
        "specifies the",   // "Specifies the output format"
        "specify the",     // "Specify the path"
        "path to",         // "Path to the config file"
        "name of",         // "Name of the resource"
        "location of",     // "Location of the file"
        "format for"       // "Format for the output"
    ];

    #endregion

    private readonly ILogger<HeuristicTypeDetector> _logger;

    public int Priority => 300; // Lowest priority
    public string Name => "Heuristic";

    public HeuristicTypeDetector(ILogger<HeuristicTypeDetector> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;
    }

    public bool CanHandle(string toolName)
    {
        return true; // Can handle any tool as fallback
    }

    public Task<OptionTypeDetectionResult> DetectTypeAsync(
        OptionDetectionContext context,
        CancellationToken cancellationToken = default)
    {
        var result = AnalyzeOption(context);
        return Task.FromResult(result);
    }

    private OptionTypeDetectionResult AnalyzeOption(OptionDetectionContext context)
    {
        var optionName = context.OptionName.TrimStart('-');
        var description = context.Description?.ToLowerInvariant() ?? "";
        var defaultValue = context.DefaultValue?.ToLowerInvariant() ?? "";
        var acceptedValues = context.AcceptedValues?.ToLowerInvariant() ?? "";

        // Check default value first
        if (!string.IsNullOrEmpty(defaultValue))
        {
            if (defaultValue is "true" or "false" or "yes" or "no" or "0" or "1")
            {
                return CreateResult(CliOptionType.Bool, "Default value is boolean-like");
            }

            if (int.TryParse(defaultValue, out _))
            {
                return CreateResult(CliOptionType.Int, "Default value is numeric");
            }
        }

        // Check accepted values for boolean indicators
        if (!string.IsNullOrEmpty(acceptedValues))
        {
            if (IsBooleanAcceptedValues(acceptedValues))
            {
                return CreateResult(CliOptionType.Bool, "Accepted values indicate boolean");
            }

            var enumResult = TryCreateEnumResult(context.AcceptedValues!, "Accepted values");
            if (enumResult is not null)
            {
                return enumResult;
            }
        }

        var descriptionEnumResult = TryDetectEnumFromDescription(context.Description);
        if (descriptionEnumResult is not null)
        {
            return descriptionEnumResult;
        }

        if (NumericDescriptionPatterns.Any(pattern => description.Contains(pattern)))
        {
            return CreateResult(CliOptionType.Int, "Description explicitly indicates an integer value", 85);
        }

        // Check for strong value-indicating patterns FIRST (before boolean checks)
        // These patterns override any potential boolean detection based on name alone
        if (MultiValueDescriptionPatterns.Any(p => description.Contains(p)))
        {
            return CreateResult(CliOptionType.StringList, "Description indicates multi-value option", 80);
        }

        if (ValueDescriptionPatternsHighConfidence.Any(p => description.Contains(p)))
        {
            return CreateResult(CliOptionType.String, "Description contains explicit value example", 85);
        }

        if (ValueDescriptionPatternsMediumConfidence.Any(p => description.Contains(p)))
        {
            return CreateResult(CliOptionType.String, "Description suggests value option", 70);
        }

        // Check option name patterns for booleans
        if (BooleanPrefixes.Any(p => optionName.StartsWith(p, StringComparison.OrdinalIgnoreCase)) ||
            BooleanSuffixes.Any(s => optionName.EndsWith(s, StringComparison.OrdinalIgnoreCase)) ||
            BooleanExactNames.Contains(optionName))
        {
            return CreateResult(CliOptionType.Bool, $"Option name pattern suggests boolean: {optionName}");
        }

        // Check description patterns for booleans
        if (BooleanDescriptionPatterns.Any(p => description.Contains(p)))
        {
            return CreateResult(CliOptionType.Bool, "Description suggests boolean flag");
        }

        // Durations and sizes frequently include units (for example, 5m0s or 10Gi).
        // A numeric default was already handled above and remains stronger evidence.
        if (UnitBearingNamePatterns.Any(p => optionName.Contains(p, StringComparison.OrdinalIgnoreCase)))
        {
            return CreateResult(CliOptionType.String, $"Option name may accept a unit-bearing value: {optionName}", 70);
        }

        // Check for numeric patterns
        if (NumericNamePatterns.Any(p => optionName.Contains(p, StringComparison.OrdinalIgnoreCase)))
        {
            return CreateResult(CliOptionType.Int, $"Option name suggests numeric: {optionName}");
        }

        // Check for list patterns
        if (ListNamePatterns.Any(p => optionName.Contains(p, StringComparison.OrdinalIgnoreCase)))
        {
            return CreateResult(CliOptionType.StringList, $"Option name suggests list: {optionName}");
        }

        // Check for key-value patterns
        if (KeyValueNamePatterns.Any(p => optionName.Contains(p, StringComparison.OrdinalIgnoreCase)) ||
            KeyValueDescriptionPatterns.Any(p => description.Contains(p)))
        {
            return CreateResult(CliOptionType.KeyValue, "Pattern suggests key-value");
        }

        // Default to string
        _logger.LogDebug("Defaulting {Option} to string (no patterns matched)", context.OptionName);
        return CreateResult(CliOptionType.String, "Default fallback to string");
    }

    private OptionTypeDetectionResult CreateResult(CliOptionType type, string reason, int confidence = 50)
    {
        _logger.LogDebug("Heuristic detection: {Type} - {Reason} (confidence: {Confidence})", type, reason, confidence);
        return new OptionTypeDetectionResult
        {
            Type = type,
            Confidence = confidence,
            Source = Name,
            Notes = reason
        };
    }

    private static bool IsBooleanAcceptedValues(string acceptedValues)
    {
        var values = BooleanValueSeparatorPattern()
            .Split(acceptedValues)
            .Select(value => value.Trim(' ', '"', '\'', '`'))
            .ToFrozenSet(StringComparer.OrdinalIgnoreCase);
        var count = BooleanLiteralValues.Count(values.Contains);
        return count >= 2; // At least 2 boolean-like values
    }

    private OptionTypeDetectionResult? TryDetectEnumFromDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return null;
        }

        var explicitValuesMatch = ExplicitValuesPattern().Match(description);
        if (explicitValuesMatch.Success)
        {
            return TryCreateEnumResult(explicitValuesMatch.Groups["values"].Value, "Explicit allowed values");
        }

        var parenthesizedValuesMatch = ParenthesizedValuesPattern().Match(description);
        return parenthesizedValuesMatch.Success
            ? TryCreateEnumResult(parenthesizedValuesMatch.Groups["values"].Value, "Parenthesized allowed values", 85)
            : null;
    }

    private OptionTypeDetectionResult? TryCreateEnumResult(string valuesText, string reason, int confidence = 95)
    {
        var values = ParseEnumValues(valuesText);
        if (values.Length < 2 || values.Length > 20)
        {
            return null;
        }

        _logger.LogDebug(
            "Heuristic detection: Enum - {Reason}: {Values} (confidence: {Confidence})",
            reason,
            string.Join(", ", values),
            confidence);

        return new OptionTypeDetectionResult
        {
            Type = CliOptionType.Enum,
            Confidence = confidence,
            Source = Name,
            Notes = $"{reason}: {string.Join(", ", values)}",
            EnumValues = values
        };
    }

    private static string[] ParseEnumValues(string valuesText)
    {
        return EnumValueSeparatorPattern()
            .Split(valuesText)
            .Select(value => LeadingConjunctionPattern().Replace(value.Trim(), ""))
            .Select(value => value.Trim('"', '\'', '`'))
            .Where(value => EnumValuePattern().IsMatch(value) && value.Any(char.IsLetter))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    [GeneratedRegex("""(?:one of|valid values|allowed values|possible values|accepted values)(?:\s+are)?\s*:?\s*(?<values>[\w.+/\-'`"]+(?:\s*(?:\||,)\s*(?:or\s+|and\s+)?[\w.+/\-'`"]+)+)""", RegexOptions.IgnoreCase)]
    private static partial Regex ExplicitValuesPattern();

    [GeneratedRegex("""(?:format|type|mode)\s*\((?<values>[\w.+/\-'`"]+(?:\s*(?:\||,)\s*(?:or\s+|and\s+)?[\w.+/\-'`"]+)+)\)""", RegexOptions.IgnoreCase)]
    private static partial Regex ParenthesizedValuesPattern();

    [GeneratedRegex(@"\s*(?:\||,)\s*")]
    private static partial Regex EnumValueSeparatorPattern();

    [GeneratedRegex(@"\s*(?:\||,|/)\s*")]
    private static partial Regex BooleanValueSeparatorPattern();

    [GeneratedRegex(@"^(?:or|and)\s+", RegexOptions.IgnoreCase)]
    private static partial Regex LeadingConjunctionPattern();

    [GeneratedRegex(@"^[A-Za-z0-9][A-Za-z0-9_-]{0,28}$")]
    private static partial Regex EnumValuePattern();
}
