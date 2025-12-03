using Microsoft.Extensions.Logging;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// Fallback detector that uses heuristics based on option names and descriptions.
/// This is the lowest priority detector, used when all others fail.
/// </summary>
public class HeuristicTypeDetector : IOptionTypeDetector
{
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
        var booleanPrefixes = new[]
        {
            "no-", "disable-", "enable-", "with-", "without-",
            "skip-", "force-", "allow-", "deny-", "include-", "exclude-"
        };

        var booleanSuffixes = new[]
        {
            "-only", "-all", "-recursive", "-verbose", "-quiet", "-silent",
            "-interactive", "-yes", "-no", "-force"
        };

        var booleanExactNames = new[]
        {
            "verbose", "quiet", "silent", "debug", "force", "yes", "no",
            "dry-run", "dryrun", "help", "version", "interactive",
            "detach", "tty", "privileged", "rm", "init", "recursive",
            "all", "cached", "staged", "untracked", "ignored"
        };

        if (booleanPrefixes.Any(p => optionName.StartsWith(p, StringComparison.OrdinalIgnoreCase)) ||
            booleanSuffixes.Any(s => optionName.EndsWith(s, StringComparison.OrdinalIgnoreCase)) ||
            booleanExactNames.Contains(optionName, StringComparer.OrdinalIgnoreCase))
        {
            return CreateResult(CliOptionType.Bool, $"Option name pattern suggests boolean: {optionName}");
        }

        // Check description patterns for booleans
        var booleanDescPatterns = new[]
        {
            "enable", "disable", "turn on", "turn off",
            "whether to", "if true", "if set", "when set",
            "flag to", "activate", "deactivate"
        };

        if (booleanDescPatterns.Any(p => description.Contains(p)))
        {
            return CreateResult(CliOptionType.Bool, "Description suggests boolean flag");
        }

        // Check for numeric patterns
        var numericNamePatterns = new[]
        {
            "count", "limit", "max", "min", "size", "length", "depth",
            "level", "retries", "timeout", "interval", "port", "number"
        };

        if (numericNamePatterns.Any(p => optionName.Contains(p, StringComparison.OrdinalIgnoreCase)))
        {
            return CreateResult(CliOptionType.Int, $"Option name suggests numeric: {optionName}");
        }

        // Check for list patterns
        var listNamePatterns = new[]
        {
            "add-", "-add", "exclude", "include", "label", "env", "volume",
            "mount", "publish", "expose", "cap-", "device", "dns"
        };

        if (listNamePatterns.Any(p => optionName.Contains(p, StringComparison.OrdinalIgnoreCase)))
        {
            return CreateResult(CliOptionType.StringList, $"Option name suggests list: {optionName}");
        }

        // Check for key-value patterns
        if (optionName.Contains("env", StringComparison.OrdinalIgnoreCase) ||
            optionName.Contains("label", StringComparison.OrdinalIgnoreCase) ||
            optionName.Contains("annotation", StringComparison.OrdinalIgnoreCase) ||
            optionName.Contains("sysctl", StringComparison.OrdinalIgnoreCase) ||
            description.Contains("key=value") ||
            description.Contains("key:value"))
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
        var boolValues = new[] { "true", "false", "yes", "no", "0", "1" };
        var count = boolValues.Count(v => acceptedValues.Contains(v));
        return count >= 2; // At least 2 boolean-like values
    }
}
