using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// Base class for help text-based type detectors.
/// Provides caching and common parsing utilities.
/// </summary>
public abstract class HelpTextTypeDetectorBase : IOptionTypeDetector
{
    private const string HelpTextCacheKey = "HelpText";

    protected readonly ICliCommandExecutor Executor;
    protected readonly ILogger Logger;

    /// <inheritdoc />
    public abstract int Priority { get; }

    /// <inheritdoc />
    public abstract string Name { get; }

    /// <inheritdoc />
    public abstract bool CanHandle(string toolName);

    protected HelpTextTypeDetectorBase(ICliCommandExecutor executor, ILogger logger)
    {
        Executor = executor;
        Logger = logger;
    }

    /// <inheritdoc />
    public async Task<OptionTypeDetectionResult> DetectTypeAsync(
        OptionDetectionContext context,
        CancellationToken cancellationToken = default)
    {
        // Get or fetch help text (cached per command)
        var helpText = await GetOrFetchHelpTextAsync(context, cancellationToken);
        if (string.IsNullOrEmpty(helpText))
        {
            return OptionTypeDetectionResult.Unknown(Name);
        }

        // Parse the help text for this specific option
        return ParseOptionFromHelpText(helpText, context);
    }

    /// <summary>
    /// Gets the cached help text or fetches it from the CLI.
    /// </summary>
    protected async Task<string?> GetOrFetchHelpTextAsync(
        OptionDetectionContext context,
        CancellationToken cancellationToken)
    {
        var cacheKey = (HelpTextCacheKey, context.ToolName, string.Join("_", context.CommandPath));

        if (context.CommandCache.TryGetValue(cacheKey, out var cached) && cached is string cachedText)
        {
            return cachedText;
        }

        var helpText = await FetchHelpTextAsync(context, cancellationToken);
        if (helpText is not null)
        {
            context.CommandCache[cacheKey] = helpText;
        }

        return helpText;
    }

    /// <summary>
    /// Fetches the help text for the command.
    /// Override to customize the help command format.
    /// </summary>
    protected virtual async Task<string?> FetchHelpTextAsync(
        OptionDetectionContext context,
        CancellationToken cancellationToken)
    {
        // Build the help command (e.g., "docker container run --help")
        var arguments = string.Join(" ", context.CommandPath.Skip(1)) + " --help";
        var result = await Executor.ExecuteAsync(context.ToolName, arguments.Trim(), cancellationToken);

        if (result.Success || !string.IsNullOrEmpty(result.StandardOutput))
        {
            return result.CombinedOutput;
        }

        Logger.LogWarning("Failed to fetch help for {Tool} {Command}: {Error}",
            context.ToolName, string.Join(" ", context.CommandPath), result.StandardError);
        return null;
    }

    /// <summary>
    /// Parses the help text to detect the type of a specific option.
    /// Must be implemented by derived classes based on the CLI's help format.
    /// </summary>
    protected abstract OptionTypeDetectionResult ParseOptionFromHelpText(
        string helpText,
        OptionDetectionContext context);

    /// <summary>
    /// Finds the line(s) in help text that describe the given option.
    /// </summary>
    protected static string? FindOptionLine(string helpText, OptionDetectionContext context)
    {
        var lines = helpText.Split('\n');

        foreach (var optionName in context.AllNames)
        {
            // Look for lines containing this option name
            foreach (var line in lines)
            {
                // Match option at start of line or after whitespace (to avoid matching substrings)
                var pattern = $@"(^|\s){Regex.Escape(optionName)}(\s|,|=|$)";
                if (Regex.IsMatch(line, pattern, RegexOptions.IgnoreCase))
                {
                    return line;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Converts a detected type string (e.g., "string", "int", "list") to CliOptionType.
    /// </summary>
    protected static CliOptionType ParseTypeHint(string typeHint)
    {
        var normalized = typeHint.Trim().ToLowerInvariant();

        return normalized switch
        {
            // Boolean indicators
            "" => CliOptionType.Bool, // No type specified often means boolean flag
            "bool" or "boolean" => CliOptionType.Bool,

            // Numeric types
            "int" or "integer" or "number" or "uint" => CliOptionType.Int,
            "float" or "decimal" or "double" => CliOptionType.Decimal,

            // List/array types
            "list" or "array" or "strings" or "stringarray" => CliOptionType.StringList,

            // Map/dictionary types
            "map" or "dict" or "dictionary" or "stringtostring" => CliOptionType.KeyValue,

            // Everything else is a string
            _ => CliOptionType.String
        };
    }
}
