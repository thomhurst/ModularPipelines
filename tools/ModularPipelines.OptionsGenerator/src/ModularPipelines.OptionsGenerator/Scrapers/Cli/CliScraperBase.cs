using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Threading.Channels;
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
    /// Maximum parallelism for concurrent command discovery.
    /// Defaults to Environment.ProcessorCount.
    /// </summary>
    protected virtual int MaxParallelism => Environment.ProcessorCount;

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
    /// Tracks state for parallel scraping workers.
    /// </summary>
    private sealed class WorkerState
    {
        public int ActiveWorkers;
        public int PendingWork;
        public readonly object Lock = new();
    }

    /// <summary>
    /// Main scraping orchestration - streams commands as they are discovered.
    /// Uses parallel discovery with configurable concurrency for faster scraping.
    /// </summary>
    public virtual async IAsyncEnumerable<CliCommandDefinition> ScrapeAsync(
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Logger.LogInformation("Discovering {Tool} commands via CLI (executable: {Path}, parallelism: {Parallelism})...",
            ToolName, ExecutablePath, MaxParallelism);

        // Check availability first
        if (!await IsAvailableAsync(cancellationToken))
        {
            Logger.LogError("{Tool} is not available on this system (tried: {Path})",
                ToolName, ExecutablePath);
            yield break;
        }

        // Channel for discovered commands to be yielded
        var commandChannel = Channel.CreateUnbounded<CliCommandDefinition>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false
        });

        // Channel for paths to explore (work queue)
        var workChannel = Channel.CreateUnbounded<string[]>(new UnboundedChannelOptions
        {
            SingleReader = false,
            SingleWriter = false
        });

        // Track worker state
        var state = new WorkerState();

        // Start discovery with root path
        await workChannel.Writer.WriteAsync([ToolName], cancellationToken);
        Interlocked.Increment(ref state.PendingWork);

        // Start worker tasks
        var workerTasks = Enumerable.Range(0, MaxParallelism)
            .Select(_ => ProcessWorkQueueAsync(workChannel, commandChannel, state, cancellationToken))
            .ToList();

        // Background task to complete channels when all work is done
        _ = Task.Run(async () =>
        {
            await Task.WhenAll(workerTasks);
            commandChannel.Writer.Complete();
        }, cancellationToken);

        // Yield commands as they're discovered
        var commandCount = 0;
        await foreach (var command in commandChannel.Reader.ReadAllAsync(cancellationToken))
        {
            commandCount++;
            Logger.LogInformation("Yielding command {Count}: {Command}", commandCount, command.FullCommand);
            yield return command;
        }

        Logger.LogInformation("Finished scraping {Tool}. Total commands: {Count}", ToolName, commandCount);
    }

    /// <summary>
    /// Worker that processes paths from the work queue in parallel.
    /// </summary>
    private async Task ProcessWorkQueueAsync(
        Channel<string[]> workChannel,
        Channel<CliCommandDefinition> commandChannel,
        WorkerState state,
        CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            string[]? path;

            // Try to get work from the queue
            lock (state.Lock)
            {
                if (state.PendingWork == 0 && state.ActiveWorkers == 0)
                {
                    // No more work and no active workers - we're done
                    workChannel.Writer.TryComplete();
                    return;
                }
            }

            try
            {
                // Wait for work with a timeout to check completion condition
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(TimeSpan.FromMilliseconds(100));

                if (!await workChannel.Reader.WaitToReadAsync(cts.Token))
                {
                    // Channel completed
                    return;
                }

                if (!workChannel.Reader.TryRead(out path))
                {
                    continue;
                }
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                // Timeout - check if we should exit
                lock (state.Lock)
                {
                    if (state.PendingWork == 0 && state.ActiveWorkers == 0)
                    {
                        workChannel.Writer.TryComplete();
                        return;
                    }
                }
                continue;
            }

            Interlocked.Increment(ref state.ActiveWorkers);

            try
            {
                await ProcessPathAsync(path, workChannel, commandChannel, state, cancellationToken);
            }
            finally
            {
                Interlocked.Decrement(ref state.ActiveWorkers);
                Interlocked.Decrement(ref state.PendingWork);
            }
        }
    }

    /// <summary>
    /// Processes a single path - gets help, parses command, enqueues subcommands.
    /// </summary>
    private async Task ProcessPathAsync(
        string[] path,
        Channel<string[]> workChannel,
        Channel<CliCommandDefinition> commandChannel,
        WorkerState state,
        CancellationToken cancellationToken)
    {
        var currentDepth = path.Length - 1; // -1 because tool name is at position 0

        if (currentDepth >= MaxDiscoveryDepth)
        {
            return;
        }

        var helpText = await GetHelpTextAsync(path, cancellationToken);
        if (string.IsNullOrEmpty(helpText))
        {
            return;
        }

        // If this command has options, parse and write to channel
        if (HasOptions(helpText) && path.Length > 1)
        {
            try
            {
                var command = await ParseCommandAsync(path, helpText, cancellationToken);
                if (command is not null)
                {
                    await commandChannel.Writer.WriteAsync(command, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                var cmdPath = string.Join(" ", path);
                Logger.LogWarning(ex, "Failed to parse command: {Command}", cmdPath);
            }
        }

        // Enqueue subcommands for parallel processing
        var subcommands = ExtractSubcommands(helpText);
        foreach (var subcommand in subcommands)
        {
            if (IsSkippableSubcommand(subcommand))
            {
                continue;
            }

            var childPath = path.Append(subcommand).ToArray();
            Interlocked.Increment(ref state.PendingWork);
            await workChannel.Writer.WriteAsync(childPath, cancellationToken);
        }
    }

    /// <summary>
    /// Creates a tool definition for metadata purposes (used by generators).
    /// </summary>
    public virtual CliToolDefinition CreateToolDefinition()
    {
        return new CliToolDefinition
        {
            ToolName = ToolName,
            NamespacePrefix = NamespacePrefix,
            TargetNamespace = TargetNamespace,
            OutputDirectory = OutputDirectory,
            Commands = [],
            Errors = []
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
        // Skip flag-like names (e.g., "--tls", "--tlsverify", "-h")
        // These are CLI flags that sometimes appear in help output sections
        if (subcommand.StartsWith('-'))
        {
            return true;
        }

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
