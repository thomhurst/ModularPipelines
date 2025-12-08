using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// Base class for CLI-first scrapers that parse --help output directly.
/// Uses Template Method pattern - provides the orchestration logic while
/// derived classes implement CLI-specific parsing.
/// </summary>
public abstract partial class CliScraperBase : ICliScraper
{
    protected readonly ICliCommandExecutor Executor;
    protected readonly ILogger Logger;

    /// <summary>
    /// Cache for help text output to avoid redundant CLI calls.
    /// </summary>
    private readonly ConcurrentDictionary<string, string> _helpCache = new();

    #region Abstract Properties - Must Implement

    /// <summary>
    /// The tool name for display and matching (e.g., "helm", "gcloud").
    /// </summary>
    public abstract string ToolName { get; }

    /// <summary>
    /// The namespace prefix for generated classes (e.g., "Helm", "Gcloud").
    /// </summary>
    public abstract string NamespacePrefix { get; }

    /// <summary>
    /// The target namespace for generated options (e.g., "ModularPipelines.Helm").
    /// </summary>
    public abstract string TargetNamespace { get; }

    /// <summary>
    /// The output directory relative to the repository root.
    /// </summary>
    public abstract string OutputDirectory { get; }

    #endregion

    #region Virtual Properties - Can Override

    /// <summary>
    /// The executable path/name to use when running the CLI.
    /// Override for tools like gcloud.cmd on Windows.
    /// Defaults to ToolName.
    /// </summary>
    protected virtual string ExecutablePath => ToolName;

    /// <summary>
    /// Maximum depth for recursive command discovery.
    /// Override for CLIs with deeper nesting (e.g., gcloud = 5).
    /// Defaults to 3.
    /// </summary>
    protected virtual int MaxDiscoveryDepth => 3;

    /// <summary>
    /// The base options class name (e.g., "HelmOptions", "GcloudOptions").
    /// </summary>
    protected virtual string BaseOptionsClassName => $"{NamespacePrefix}Options";

    #endregion

    protected CliScraperBase(ICliCommandExecutor executor, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(executor);
        ArgumentNullException.ThrowIfNull(logger);

        Executor = executor;
        Logger = logger;
    }

    #region Template Method - Main Orchestration

    /// <summary>
    /// Checks if the CLI tool is available on the system.
    /// Uses ExecutablePath for the actual check.
    /// </summary>
    public virtual async Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default)
    {
        return await Executor.IsAvailableAsync(ExecutablePath, cancellationToken);
    }

    /// <summary>
    /// Main scraping orchestration - Template Method pattern.
    /// Provides the algorithm structure; derived classes implement parsing details.
    /// </summary>
    public virtual async Task<CliToolDefinition> ScrapeAsync(CancellationToken cancellationToken = default)
    {
        var commands = new List<CliCommandDefinition>();
        var errors = new List<ScrapingError>();

        Logger.LogInformation("Discovering {Tool} commands via CLI (executable: {Path})...",
            ToolName, ExecutablePath);

        // Step 1: Check availability
        if (!await IsAvailableAsync(cancellationToken))
        {
            Logger.LogError("{Tool} is not available on this system (tried: {Path})",
                ToolName, ExecutablePath);
            errors.Add(new ScrapingError
            {
                Source = ToolName,
                Message = $"{ToolName} is not available. Tried executable: {ExecutablePath}"
            });
            return CreateToolDefinition(commands, errors);
        }

        try
        {
            // Step 2: Discover all commands recursively
            var commandPaths = await DiscoverCommandsAsync(
                [ToolName], cancellationToken, MaxDiscoveryDepth);
            Logger.LogInformation("Discovered {Count} commands for {Tool}",
                commandPaths.Count, ToolName);

            // Step 3: Parse each command
            foreach (var commandPath in commandPaths)
            {
                try
                {
                    var helpText = await GetHelpTextAsync(commandPath, cancellationToken);
                    if (string.IsNullOrEmpty(helpText))
                    {
                        continue;
                    }

                    var command = await ParseCommandAsync(commandPath, helpText, cancellationToken);
                    if (command is not null)
                    {
                        commands.Add(command);
                        Logger.LogDebug("Parsed command: {Command}", command.FullCommand);
                    }
                }
                catch (Exception ex)
                {
                    var cmdPath = string.Join(" ", commandPath);
                    Logger.LogWarning(ex, "Failed to parse command: {Command}", cmdPath);
                    errors.Add(new ScrapingError
                    {
                        Source = cmdPath,
                        Message = ex.Message,
                        Exception = ex
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to discover {Tool} commands", ToolName);
            errors.Add(new ScrapingError
            {
                Source = ToolName,
                Message = ex.Message,
                Exception = ex
            });
        }

        Logger.LogInformation("Scraped {Count} commands for {Tool} with {Errors} errors",
            commands.Count, ToolName, errors.Count);

        return CreateToolDefinition(commands, errors);
    }

    /// <summary>
    /// Creates the final tool definition with all discovered commands.
    /// </summary>
    protected virtual CliToolDefinition CreateToolDefinition(
        List<CliCommandDefinition> commands,
        List<ScrapingError> errors)
    {
        return new CliToolDefinition
        {
            ToolName = ToolName,
            NamespacePrefix = NamespacePrefix,
            TargetNamespace = TargetNamespace,
            OutputDirectory = OutputDirectory,
            Commands = commands,
            Errors = errors
        };
    }

    #endregion

    #region Help Text & Discovery

    /// <summary>
    /// Gets help text for a command, using cache if available.
    /// Uses ExecutablePath for execution.
    /// </summary>
    protected virtual async Task<string?> GetHelpTextAsync(
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

        var result = await Executor.ExecuteAsync(ExecutablePath, args, cancellationToken);

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
    /// Discovers all subcommands by recursively parsing help output.
    /// </summary>
    protected virtual async Task<List<string[]>> DiscoverCommandsAsync(
        string[] parentPath,
        CancellationToken cancellationToken,
        int maxDepth)
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

    #endregion

    #region Abstract Methods - Must Implement

    /// <summary>
    /// Extracts subcommand names from help text.
    /// Each CLI has different formatting - must be implemented per CLI type.
    /// </summary>
    protected abstract IEnumerable<string> ExtractSubcommands(string helpText);

    /// <summary>
    /// Parses a command from its help text into a CliCommandDefinition.
    /// Each CLI has different option formatting - must be implemented per CLI type.
    /// </summary>
    protected abstract Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken);

    #endregion

    #region Virtual Hooks - Can Override

    /// <summary>
    /// Checks if help text indicates the command has options/flags.
    /// Override if the CLI has a different pattern for leaf commands.
    /// </summary>
    protected virtual bool HasOptions(string helpText)
    {
        return helpText.Contains("--") ||
               helpText.Contains("Options:") ||
               helpText.Contains("Flags:") ||
               helpText.Contains("Global Flags:") ||
               OptionLinePattern().IsMatch(helpText);
    }

    /// <summary>
    /// Checks if a subcommand should be skipped (e.g., "help", "completion").
    /// Override to add CLI-specific skip patterns.
    /// </summary>
    protected virtual bool IsSkippableSubcommand(string subcommand)
    {
        var lowerName = subcommand.ToLowerInvariant();
        return lowerName is "help" or "completion" or "version" or "__complete" or "__completenoDesc";
    }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Generates a class name from command path parts.
    /// </summary>
    protected string GenerateClassName(string[] commandParts)
    {
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

    #endregion
}
