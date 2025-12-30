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
    protected readonly IHelpTextCache HelpCache;
    protected readonly ILogger Logger;

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
    /// Maximum parallelism for concurrent command discovery.
    /// Defaults to Environment.ProcessorCount.
    /// </summary>
    protected virtual int MaxParallelism => Environment.ProcessorCount;

    /// <summary>
    /// The base options class name (e.g., "HelmOptions", "GcloudOptions").
    /// </summary>
    protected virtual string BaseOptionsClassName => $"{NamespacePrefix}Options";

    /// <summary>
    /// Whether to skip deprecated commands (identified by "DEPRECATED" in help text).
    /// Defaults to false (include deprecated commands).
    /// </summary>
    protected virtual bool SkipDeprecatedCommands => false;

    /// <summary>
    /// Whether to skip experimental commands (identified by "EXPERIMENTAL" or "BETA" in help text).
    /// Defaults to false (include experimental commands).
    /// </summary>
    protected virtual bool SkipExperimentalCommands => false;

    /// <summary>
    /// Additional subcommand names to skip (case-insensitive).
    /// Override to add CLI-specific skip patterns.
    /// </summary>
    protected virtual IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Regex patterns to match against command descriptions for skipping.
    /// Commands matching any pattern will be skipped.
    /// </summary>
    protected virtual IReadOnlyList<string> SkipDescriptionPatterns => [];

    #endregion

    protected CliScraperBase(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(executor);
        ArgumentNullException.ThrowIfNull(helpCache);
        ArgumentNullException.ThrowIfNull(logger);

        Executor = executor;
        HelpCache = helpCache;
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
    /// Tracks state for parallel scraping workers using a countdown pattern.
    /// Thread-safe without locks by using atomic operations and a completion signal.
    /// </summary>
    private sealed class WorkCoordinator
    {
        private int _outstandingWork;
        private readonly Channel<string[]> _workChannel;
        private readonly TaskCompletionSource _completionSignal = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public WorkCoordinator(Channel<string[]> workChannel)
        {
            _workChannel = workChannel;
        }

        /// <summary>
        /// Increments the outstanding work counter.
        /// Call this BEFORE adding work to the channel.
        /// </summary>
        public void IncrementWork()
        {
            Interlocked.Increment(ref _outstandingWork);
        }

        /// <summary>
        /// Decrements the outstanding work counter.
        /// When it reaches 0, signals completion and closes the work channel.
        /// Call this AFTER the work item has been fully processed.
        /// </summary>
        public void DecrementWork()
        {
            var remaining = Interlocked.Decrement(ref _outstandingWork);
            if (remaining == 0)
            {
                _workChannel.Writer.TryComplete();
                _completionSignal.TrySetResult();
            }
        }

        /// <summary>
        /// Gets a task that completes when all work is done.
        /// </summary>
        public Task CompletionTask => _completionSignal.Task;

        /// <summary>
        /// Current count of outstanding work items (for diagnostics).
        /// </summary>
        public int OutstandingWork => Volatile.Read(ref _outstandingWork);
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

        // Coordinator handles completion signaling atomically
        var coordinator = new WorkCoordinator(workChannel);

        // Start discovery with root path - increment BEFORE adding to channel
        coordinator.IncrementWork();
        await workChannel.Writer.WriteAsync([ToolName], cancellationToken);

        // Start worker tasks
        var workerTasks = Enumerable.Range(0, MaxParallelism)
            .Select(_ => ProcessWorkQueueAsync(workChannel, commandChannel, coordinator, cancellationToken))
            .ToList();

        // Background task to complete command channel when all work is done
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
    /// Exits cleanly when the work channel is completed.
    /// </summary>
    private async Task ProcessWorkQueueAsync(
        Channel<string[]> workChannel,
        Channel<CliCommandDefinition> commandChannel,
        WorkCoordinator coordinator,
        CancellationToken cancellationToken)
    {
        // ReadAllAsync handles channel completion cleanly - no polling needed
        await foreach (var path in workChannel.Reader.ReadAllAsync(cancellationToken))
        {
            try
            {
                await ProcessPathAsync(path, workChannel, commandChannel, coordinator, cancellationToken);
            }
            finally
            {
                // Decrement AFTER fully processing (including enqueueing children)
                coordinator.DecrementWork();
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
        WorkCoordinator coordinator,
        CancellationToken cancellationToken)
    {
        var helpText = await GetHelpTextAsync(path, cancellationToken);
        if (string.IsNullOrEmpty(helpText))
        {
            return;
        }

        // Check if this command should be skipped based on help text content
        if (ShouldSkipBasedOnHelpText(helpText))
        {
            var cmdPath = string.Join(" ", path);
            Logger.LogDebug("Skipping command based on help text filter: {Command}", cmdPath);
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
        // IMPORTANT: Increment BEFORE adding to channel to avoid race conditions
        var subcommands = ExtractSubcommands(helpText);
        foreach (var subcommand in subcommands)
        {
            if (IsSkippableSubcommand(subcommand))
            {
                continue;
            }

            var childPath = path.Append(subcommand).ToArray();
            coordinator.IncrementWork();
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

        if (HelpCache.TryGet(cacheKey, out var cached))
        {
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
            HelpCache.Set(cacheKey, helpText);
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
    /// Default subcommands to always skip.
    /// </summary>
    private static readonly HashSet<string> DefaultSkipSubcommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "help", "completion", "version", "__complete", "__completeNoDesc"
    };

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

        // Check default skip list
        if (DefaultSkipSubcommands.Contains(subcommand))
        {
            return true;
        }

        // Check additional skip list from derived class
        if (AdditionalSkipSubcommands.Contains(subcommand))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if a command should be skipped based on its help text content.
    /// Looks for deprecated/experimental markers based on configuration.
    /// </summary>
    protected virtual bool ShouldSkipBasedOnHelpText(string helpText)
    {
        if (string.IsNullOrWhiteSpace(helpText))
        {
            return false;
        }

        // Check for deprecated commands
        if (SkipDeprecatedCommands)
        {
            if (helpText.Contains("DEPRECATED", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        // Check for experimental/beta commands
        if (SkipExperimentalCommands)
        {
            if (helpText.Contains("EXPERIMENTAL", StringComparison.OrdinalIgnoreCase) ||
                helpText.Contains("BETA", StringComparison.OrdinalIgnoreCase) ||
                helpText.Contains("(beta)", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        // Check custom skip patterns
        foreach (var pattern in SkipDescriptionPatterns)
        {
            if (Regex.IsMatch(helpText, pattern, RegexOptions.IgnoreCase))
            {
                return true;
            }
        }

        return false;
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

        if (input.Length == 1)
        {
            return char.ToUpperInvariant(input[0]).ToString();
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
