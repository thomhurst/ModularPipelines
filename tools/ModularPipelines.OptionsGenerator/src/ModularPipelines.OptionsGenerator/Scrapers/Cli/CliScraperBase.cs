using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// Base class for CLI-first scrapers that parse --help output directly.
/// Provides common functionality for command discovery, caching, and parsing.
/// </summary>
public abstract partial class CliScraperBase : ICliScraper
{
    protected readonly ICliCommandExecutor Executor;
    protected readonly ILogger Logger;

    /// <summary>
    /// Cache for help text output to avoid redundant CLI calls.
    /// Key is the full command path (e.g., "helm repo add").
    /// </summary>
    private readonly ConcurrentDictionary<string, string> _helpCache = new();

    public abstract string ToolName { get; }
    public abstract string NamespacePrefix { get; }
    public abstract string TargetNamespace { get; }
    public abstract string OutputDirectory { get; }

    /// <summary>
    /// The base options class name (e.g., "HelmOptions", "DockerOptions").
    /// </summary>
    protected virtual string BaseOptionsClassName => $"{NamespacePrefix}Options";

    protected CliScraperBase(ICliCommandExecutor executor, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(executor);
        ArgumentNullException.ThrowIfNull(logger);

        Executor = executor;
        Logger = logger;
    }

    public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default)
    {
        return await Executor.IsAvailableAsync(ToolName, cancellationToken);
    }

    public abstract Task<CliToolDefinition> ScrapeAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets help text for a command, using cache if available.
    /// </summary>
    protected async Task<string?> GetHelpTextAsync(
        string[] commandPath,
        CancellationToken cancellationToken)
    {
        var cacheKey = string.Join(" ", commandPath);

        if (_helpCache.TryGetValue(cacheKey, out var cached))
        {
            Logger.LogDebug("Using cached help for: {Command}", cacheKey);
            return cached;
        }

        // Build the arguments: everything after the tool name, plus --help
        var args = commandPath.Length > 1
            ? string.Join(" ", commandPath.Skip(1)) + " --help"
            : "--help";

        var result = await Executor.ExecuteAsync(ToolName, args, cancellationToken);

        // Many CLIs output help to stderr when using --help
        var helpText = !string.IsNullOrEmpty(result.StandardOutput)
            ? result.StandardOutput
            : result.StandardError;

        if (!string.IsNullOrWhiteSpace(helpText))
        {
            _helpCache[cacheKey] = helpText;
            return helpText;
        }

        Logger.LogWarning("No help text for command: {Command}", cacheKey);
        return null;
    }

    /// <summary>
    /// Discovers all subcommands by parsing help output.
    /// </summary>
    protected async Task<List<string[]>> DiscoverCommandsAsync(
        string[] parentPath,
        CancellationToken cancellationToken,
        int maxDepth = 3)
    {
        var commands = new List<string[]>();
        var currentDepth = parentPath.Length - 1; // -1 because tool name is at position 0

        if (currentDepth >= maxDepth)
        {
            return commands;
        }

        var helpText = await GetHelpTextAsync(parentPath, cancellationToken);
        if (string.IsNullOrEmpty(helpText))
        {
            return commands;
        }

        // Check if this command has options (is a leaf command)
        if (HasOptions(helpText))
        {
            commands.Add(parentPath);
        }

        // Find subcommands
        var subcommands = ExtractSubcommands(helpText);

        foreach (var subcommand in subcommands)
        {
            // Skip common non-command entries
            if (IsSkippableSubcommand(subcommand))
            {
                continue;
            }

            var childPath = parentPath.Append(subcommand).ToArray();

            // Recursively discover child commands
            var childCommands = await DiscoverCommandsAsync(childPath, cancellationToken, maxDepth);
            commands.AddRange(childCommands);
        }

        return commands;
    }

    /// <summary>
    /// Checks if help text indicates the command has options/flags.
    /// </summary>
    protected virtual bool HasOptions(string helpText)
    {
        // Look for common option patterns
        return helpText.Contains("--") ||
               helpText.Contains("Options:") ||
               helpText.Contains("Flags:") ||
               helpText.Contains("Global Flags:") ||
               OptionLinePattern().IsMatch(helpText);
    }

    /// <summary>
    /// Extracts subcommand names from help text.
    /// Override in derived classes for CLI-specific parsing.
    /// </summary>
    protected abstract IEnumerable<string> ExtractSubcommands(string helpText);

    /// <summary>
    /// Checks if a subcommand should be skipped (e.g., "help", "completion").
    /// </summary>
    protected virtual bool IsSkippableSubcommand(string subcommand)
    {
        var lowerName = subcommand.ToLowerInvariant();
        return lowerName is "help" or "completion" or "version" or "__complete" or "__completenoDesc";
    }

    /// <summary>
    /// Parses a command from its help text into a CliCommandDefinition.
    /// </summary>
    protected abstract Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken);

    /// <summary>
    /// Generates a class name from command path parts.
    /// </summary>
    protected string GenerateClassName(string[] commandParts)
    {
        // Skip the tool name and convert to PascalCase
        var parts = commandParts
            .Skip(1) // Skip tool name
            .SelectMany(part => part.Split('-', StringSplitOptions.RemoveEmptyEntries))
            .Select(ToPascalCase);

        return $"{NamespacePrefix}{string.Join("", parts)}Options";
    }

    /// <summary>
    /// Normalizes a CLI option name to a C# property name.
    /// </summary>
    protected static string? NormalizePropertyName(string optionName)
    {
        // Skip if contains special characters that indicate it's an example
        if (optionName.Contains('=') || optionName.Contains('"') ||
            optionName.Contains('\'') || optionName.Contains(':'))
        {
            return null;
        }

        var cleaned = optionName.TrimStart('-');
        if (string.IsNullOrWhiteSpace(cleaned) || cleaned.All(c => c == '-' || c == '_'))
        {
            return null;
        }

        var parts = cleaned.Split(['-', '_'], StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
        {
            return null;
        }

        return string.Join("", parts.Select(ToPascalCase));
    }

    /// <summary>
    /// Converts a string to PascalCase.
    /// </summary>
    protected static string ToPascalCase(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        return char.ToUpperInvariant(input[0]) + input[1..].ToLowerInvariant();
    }

    /// <summary>
    /// Pattern to match option lines (e.g., "-f, --flag" or "--option").
    /// </summary>
    [GeneratedRegex(@"^\s*(?:-\w,\s*)?--[\w-]+", RegexOptions.Multiline)]
    protected static partial Regex OptionLinePattern();
}
