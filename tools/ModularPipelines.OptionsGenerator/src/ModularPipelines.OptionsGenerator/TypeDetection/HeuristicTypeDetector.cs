using Microsoft.Extensions.Logging;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// Fallback detector that uses heuristics based on option names and descriptions.
/// This is the lowest priority detector, used when all others fail.
/// </summary>
public class HeuristicTypeDetector : IOptionTypeDetector
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
    /// </summary>
    private static readonly string[] BooleanExactNames =
    [
        "verbose", "quiet", "silent", "debug", "force", "yes", "no",
        "dry-run", "dryrun", "help", "version", "interactive",
        "detach", "tty", "privileged", "rm", "init", "recursive",
        "all", "cached", "staged", "untracked", "ignored"
    ];

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
    /// These patterns suggest counts, limits, sizes, or other numerical quantities.
    /// Note: "level" is intentionally excluded as it often refers to verbosity levels which are enums/strings.
    /// </summary>
    private static readonly string[] NumericNamePatterns =
    [
        "count", "limit", "max", "min", "size", "length", "depth",
        "retries", "timeout", "interval", "port", "number"
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
        "true", "false", "yes", "no", "0", "1"
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
        }

        // Check option name patterns for booleans
        if (BooleanPrefixes.Any(p => optionName.StartsWith(p, StringComparison.OrdinalIgnoreCase)) ||
            BooleanSuffixes.Any(s => optionName.EndsWith(s, StringComparison.OrdinalIgnoreCase)) ||
            BooleanExactNames.Contains(optionName, StringComparer.OrdinalIgnoreCase))
        {
            return CreateResult(CliOptionType.Bool, $"Option name pattern suggests boolean: {optionName}");
        }

        // Check description patterns for booleans
        if (BooleanDescriptionPatterns.Any(p => description.Contains(p)))
        {
            return CreateResult(CliOptionType.Bool, "Description suggests boolean flag");
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

    private OptionTypeDetectionResult CreateResult(CliOptionType type, string reason)
    {
        _logger.LogDebug("Heuristic detection: {Type} - {Reason}", type, reason);
        return new OptionTypeDetectionResult
        {
            Type = type,
            Confidence = 50, // Lower confidence for heuristic guessing
            Source = Name,
            Notes = reason
        };
    }

    private static bool IsBooleanAcceptedValues(string acceptedValues)
    {
        var count = BooleanLiteralValues.Count(v => acceptedValues.Contains(v));
        return count >= 2; // At least 2 boolean-like values
    }
}
