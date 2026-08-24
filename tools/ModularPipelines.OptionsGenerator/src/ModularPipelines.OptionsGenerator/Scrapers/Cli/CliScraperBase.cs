using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Generators;
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
    private readonly ConcurrentDictionary<string, CliCommandGroupAlias> _commandGroupAliases =
        new(StringComparer.OrdinalIgnoreCase);

    protected readonly ICliCommandExecutor Executor;
    protected readonly IHelpTextCache HelpCache;
    protected readonly ILogger Logger;

    /// <summary>
    /// Global options parsed from the root help text. These are emitted on the generated
    /// base options class and placed before subcommands at execution time.
    /// </summary>
    protected IReadOnlyList<CliOptionDefinition> GlobalOptions { get; private set; } = [];

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

    /// <summary>
    /// The operating-system family used to run this scraper in generation automation.
    /// </summary>
    public virtual CliGenerationPlatform GenerationPlatform => CliGenerationPlatform.Linux;

    /// <inheritdoc />
    public virtual bool IncludeInGenerationMatrix => true;

    /// <inheritdoc />
    public virtual bool GenerateCommandFacade => true;

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
    /// Arguments used to query the installed CLI version.
    /// </summary>
    protected virtual string VersionArguments => "--version";

    /// <summary>
    /// Converts a CLI command segment into its generated C# identifier.
    /// Override for tool-specific compound names that cannot be inferred from separators.
    /// </summary>
    protected virtual string NormalizeCommandIdentifier(string commandPart) => ToPascalCase(commandPart);

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
    /// Maximum depth for command path exploration.
    /// Prevents infinite loops from malformed help text or regex issues.
    /// Defaults to 10 levels deep (e.g., "tool a b c d e f g h i j").
    /// </summary>
    protected virtual int MaxCommandDepth => 10;

    /// <summary>
    /// Additional subcommand names to skip (case-insensitive).
    /// Override to add CLI-specific skip patterns.
    /// </summary>
    protected virtual IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Global options that are documented but absent from the installed CLI's help output.
    /// </summary>
    protected virtual IReadOnlyList<CliOptionDefinition> SupplementalGlobalOptions => [];

    /// <summary>
    /// The validated union of scraped and supplemental global options.
    /// </summary>
    protected IReadOnlyList<CliOptionDefinition> EffectiveGlobalOptions =>
        CliGlobalOptionMerger.Merge(GlobalOptions, SupplementalGlobalOptions);

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
        return await Executor.IsAvailableAsync(
            ExecutablePath,
            VersionArguments,
            cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task<string?> GetVersionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await Executor.ExecuteAsync(ExecutablePath, VersionArguments, cancellationToken);
            if (!result.Success)
            {
                Logger.LogWarning(
                    "Could not determine installed {Tool} version: command exited with {ExitCode}",
                    ToolName,
                    result.ExitCode);
                return null;
            }

            return ParseVersionOutput(result);
        }
        catch (Exception ex) when (ex is not (OutOfMemoryException or StackOverflowException))
        {
            Logger.LogWarning(ex, "Could not determine installed {Tool} version", ToolName);
            return null;
        }
    }

    /// <summary>
    /// Parses successful version-command output into stable coverage metadata.
    /// </summary>
    protected virtual string? ParseVersionOutput(CliCommandResult result)
    {
        var version = result.CombinedOutput.ReplaceLineEndings(" ").Trim();
        return version.Length switch
        {
            0 => null,
            > 500 => version[..500],
            _ => version,
        };
    }

    /// <summary>
    /// Tracks state for parallel scraping workers using a countdown pattern.
    /// Thread-safe without locks by using atomic operations and a completion signal.
    /// </summary>
    private sealed class WorkCoordinator
    {
        private int _outstandingWork;
        private readonly Channel<string[]> _workChannel;

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
            }
        }
    }

    /// <summary>
    /// Main scraping orchestration - streams commands as they are discovered.
    /// Uses parallel discovery with configurable concurrency for faster scraping.
    /// </summary>
    public virtual async IAsyncEnumerable<CliCommandDefinition> ScrapeAsync(
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        _commandGroupAliases.Clear();

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
        var visitedPaths = new ConcurrentDictionary<string, byte>(StringComparer.OrdinalIgnoreCase);

        // Start discovery with root path - increment BEFORE adding to channel
        visitedPaths.TryAdd(ToolName, 0);
        coordinator.IncrementWork();
        await workChannel.Writer.WriteAsync([ToolName], cancellationToken);

        // Start worker tasks
        var workerTasks = Enumerable.Range(0, MaxParallelism)
            .Select(_ => ProcessWorkQueueAsync(
                workChannel,
                commandChannel,
                coordinator,
                visitedPaths,
                cancellationToken))
            .ToList();

        // Always complete the result channel, including when a worker faults. Without this,
        // the consumer can wait forever after an unexpected traversal failure.
        _ = CompleteCommandChannelAsync(workerTasks, commandChannel);

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
        ConcurrentDictionary<string, byte> visitedPaths,
        CancellationToken cancellationToken)
    {
        // ReadAllAsync handles channel completion cleanly - no polling needed
        await foreach (var path in workChannel.Reader.ReadAllAsync(cancellationToken))
        {
            try
            {
                await ProcessPathAsync(
                    path,
                    workChannel,
                    commandChannel,
                    coordinator,
                    visitedPaths,
                    cancellationToken);
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
        ConcurrentDictionary<string, byte> visitedPaths,
        CancellationToken cancellationToken)
    {
        if (ShouldSkipDeepPath(path))
        {
            return;
        }

        var helpText = await GetHelpTextAsync(path, cancellationToken);
        if (string.IsNullOrEmpty(helpText))
        {
            return;
        }

        helpText = NormalizeHelpText(helpText);

        var commandGroupAlias = DetectCommandGroupAlias(path, helpText);
        if (commandGroupAlias is not null)
        {
            _commandGroupAliases.TryAdd(commandGroupAlias.Alias, commandGroupAlias);
            Logger.LogInformation(
                "Skipping alias command tree {Alias}; using canonical tree {Canonical}",
                commandGroupAlias.Alias,
                commandGroupAlias.CanonicalCommand);
            return;
        }

        if (path.Length == 1)
        {
            GlobalOptions = ParseGlobalOptions(helpText);
        }

        if (ShouldSkipPath(path, helpText))
        {
            return;
        }

        if (!HelpMatchesCommandPath(path, helpText))
        {
            Logger.LogWarning(
                "Ignoring help that does not describe requested command: {Command}",
                string.Join(" ", path));
            return;
        }

        var subcommands = ExtractSubcommands(path, helpText).ToList();
        try
        {
            ValidateSubcommandDiscovery(path, helpText, subcommands);
        }
        catch (Exception ex) when (ex is not (OutOfMemoryException or StackOverflowException))
        {
            Logger.LogWarning(ex, "Failed to validate subcommand discovery: {Command}", string.Join(" ", path));
            return;
        }

        await ParseAndWriteCommandAsync(path, helpText, subcommands, commandChannel, cancellationToken);
        await EnqueueSubcommandsAsync(
            path,
            subcommands,
            workChannel,
            coordinator,
            visitedPaths,
            cancellationToken);
    }

    private bool ShouldSkipDeepPath(string[] path)
    {
        if (path.Length <= MaxCommandDepth)
        {
            return false;
        }

        Logger.LogWarning("Skipping command path that exceeds max depth ({MaxDepth}): {Path}",
            MaxCommandDepth, string.Join(" ", path));
        return true;
    }

    private bool ShouldSkipPath(string[] path, string helpText)
    {
        if (!ShouldSkipBasedOnHelpText(helpText))
        {
            return false;
        }

        Logger.LogDebug("Skipping command based on help text filter: {Command}", string.Join(" ", path));
        return true;
    }

    private void ValidateSubcommandDiscovery(
        string[] path,
        string helpText,
        IReadOnlyCollection<string> subcommands)
    {
        if (subcommands.Count != 0 || !HelpDeclaresCommandGroup(helpText))
        {
            return;
        }

        throw new InvalidOperationException(
            $"{string.Join(' ', path)} help declares a command group, but no child commands were extracted. "
            + "Update the shared command-section parser or the tool adapter before generating partial output.");
    }

    private async Task ParseAndWriteCommandAsync(
        string[] path,
        string helpText,
        IReadOnlyCollection<string> subcommands,
        Channel<CliCommandDefinition> commandChannel,
        CancellationToken cancellationToken)
    {
        var usage = ParseUsageSynopsis(path, helpText);
        if (subcommands.Count > 0)
        {
            usage = UsageSynopsisParser.RemoveCommandGroupPlaceholders(usage);
        }

        LogUsageSynopsisSelection(path, usage);
        if (ShouldSkipCommand(path, helpText, subcommands, usage))
        {
            return;
        }

        var command = await TryParseCommandAsync(path, helpText, usage, cancellationToken);
        if (command is null)
        {
            return;
        }

        command.ValidateOperandCoverage(
            usage.HasOperandTokens,
            usage.Synopsis,
            usage.PositionalArguments);
        await commandChannel.Writer.WriteAsync(command, cancellationToken);
    }

    private bool ShouldSkipCommand(
        string[] path,
        string helpText,
        IReadOnlyCollection<string> subcommands,
        UsageSynopsisParseResult usage) =>
        (!HasOptions(helpText) && !usage.HasOperandTokens)
        || (path.Length == 1 && subcommands.Count > 0);

    private async Task<CliCommandDefinition?> TryParseCommandAsync(
        string[] path,
        string helpText,
        UsageSynopsisParseResult usage,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = await ParseCommandAsync(path, helpText, usage, cancellationToken);
            if (command is null)
            {
                return null;
            }

            ValidateOptionShapes(command, helpText);
            ValidateArgumentGroups(command);
            return command;
        }
        catch (Exception ex) when (ex is not (OutOfMemoryException or StackOverflowException))
        {
            Logger.LogWarning(ex, "Failed to parse command: {Command}", string.Join(" ", path));
            return null;
        }
    }

    private void LogUsageSynopsisSelection(
        string[] commandPath,
        UsageSynopsisParseResult usage)
    {
        if (usage.MatchedSynopsisCount <= 1)
        {
            return;
        }

        if (usage.HasAmbiguousMatch)
        {
            Logger.LogWarning(
                "Multiple equally ranked usage synopses matched {Command}; selected: {Synopsis}",
                string.Join(" ", commandPath),
                usage.Synopsis);
            return;
        }

        Logger.LogDebug(
            "Selected usage synopsis for {Command} from {Count} matching candidates: {Synopsis}",
            string.Join(" ", commandPath),
            usage.MatchedSynopsisCount,
            usage.Synopsis);
    }

    private async Task EnqueueSubcommandsAsync(
        string[] path,
        IEnumerable<string> subcommands,
        Channel<string[]> workChannel,
        WorkCoordinator coordinator,
        ConcurrentDictionary<string, byte> visitedPaths,
        CancellationToken cancellationToken)
    {
        foreach (var subcommand in subcommands)
        {
            if (IsSkippableSubcommand(subcommand))
            {
                continue;
            }

            var childPath = path.Append(subcommand).ToArray();
            ValidateChildCommandPath(childPath);
            if (!visitedPaths.TryAdd(string.Join(' ', childPath), 0))
            {
                continue;
            }

            // Increment before writing to avoid completing the work queue before the child is visible.
            coordinator.IncrementWork();
            await workChannel.Writer.WriteAsync(childPath, cancellationToken);
        }
    }

    private static async Task CompleteCommandChannelAsync(
        IReadOnlyCollection<Task> workerTasks,
        Channel<CliCommandDefinition> commandChannel)
    {
        Exception? failure = null;

        try
        {
            await Task.WhenAll(workerTasks);
        }
        catch (Exception ex)
        {
            failure = ex;
        }

        commandChannel.Writer.TryComplete(failure);
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
            GenerateCommandFacade = GenerateCommandFacade,
            Commands = [],
            CommandGroupAliases = _commandGroupAliases.Values
                .OrderBy(alias => alias.Alias, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            GlobalOptions = GlobalOptions,
            SupplementalGlobalOptions = SupplementalGlobalOptions,
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
    /// Extracts subcommand names from help text for a specific command path.
    /// Adapters that need the path can override this overload while existing adapters
    /// continue to use the help-only hook.
    /// </summary>
    protected virtual IEnumerable<string> ExtractSubcommands(
        string[] commandPath,
        string helpText) => ExtractSubcommands(helpText);

    /// <summary>
    /// Extracts subcommand names from help text.
    /// Each CLI has different formatting.
    /// </summary>
    protected virtual IEnumerable<string> ExtractSubcommands(string helpText) => [];

    /// <summary>
    /// Removes terminal formatting that changes the text shape consumed by scraper parsers.
    /// Some CLIs emit ANSI sequences even when output is redirected and <c>NO_COLOR</c> is set.
    /// </summary>
    protected static string NormalizeHelpText(string helpText)
    {
        var withoutAnsi = AnsiEscapeSequencePattern().Replace(helpText, string.Empty);
        return ManPageOverstrikePattern().Replace(withoutAnsi, string.Empty);
    }

    /// <summary>
    /// Parses a command from its help text into a CliCommandDefinition.
    /// Each CLI has different option formatting - must be implemented per CLI type.
    /// </summary>
    protected abstract Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken);

    /// <summary>
    /// Parses a command using the synopsis result already computed by shared traversal.
    /// Override when a scraper consumes positional operands.
    /// </summary>
    protected virtual Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        UsageSynopsisParseResult usage,
        CancellationToken cancellationToken) =>
        ParseCommandAsync(commandPath, helpText, cancellationToken);

    #endregion

    #region Virtual Hooks - Can Override

    /// <summary>
    /// Parses options from root help that must appear before a subcommand.
    /// </summary>
    protected virtual IReadOnlyList<CliOptionDefinition> ParseGlobalOptions(string helpText) => [];

    /// <summary>
    /// Supplies extra usage synopses when a CLI omits operands from its primary usage text.
    /// </summary>
    protected virtual IEnumerable<string> GetAdditionalUsageSynopses(
        string[] commandPath,
        string helpText) => [];

    /// <summary>
    /// Detects a top-level command group whose help resolves to a canonical alias target.
    /// </summary>
    protected virtual CliCommandGroupAlias? DetectCommandGroupAlias(
        string[] commandPath,
        string helpText) => null;

    /// <summary>
    /// Returns whether the help output belongs to the requested command path.
    /// </summary>
    protected virtual bool HelpMatchesCommandPath(string[] commandPath, string helpText) => true;

    /// <summary>
    /// Parses positional operands through the shared usage/synopsis model.
    /// </summary>
    protected UsageSynopsisParseResult ParseUsageSynopsis(
        string[] commandPath,
        string helpText) =>
        UsageSynopsisParser.Parse(
            helpText,
            commandPath,
            GetAdditionalUsageSynopses(commandPath, helpText));

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
    /// Returns whether help declares a parent command group that must have discoverable children.
    /// Requiring both a usage placeholder and a command-section heading avoids confusing ordinary
    /// positional operands named "command" with command-tree nodes.
    /// </summary>
    protected virtual bool HelpDeclaresCommandGroup(string helpText) =>
        CommandGroupUsagePattern().IsMatch(helpText)
        && CommandSectionHeadingPattern().IsMatch(helpText);

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
    /// Validates a discovered child command path before it is added to the traversal queue.
    /// </summary>
    protected virtual void ValidateChildCommandPath(string[] commandPath)
    {
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
            .Select(NormalizeCommandIdentifier);

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
    /// Delegates to <see cref="GeneratorUtils.ToPascalCase"/> for consistent behavior.
    /// </summary>
    protected static string ToPascalCase(string input) => GeneratorUtils.ToPascalCase(input);

    /// <summary>
    /// Returns whether an option description requires an explicit Boolean value.
    /// </summary>
    protected static bool HelpDeclaresExplicitBooleanValue(string description) =>
        ExplicitBooleanValuePattern().IsMatch(description);

    /// <summary>
    /// Returns whether help describes an option as repeatable.
    /// </summary>
    protected static bool HelpDeclaresRepeatableOption(
        string helpText,
        string switchName,
        string description)
    {
        if (DescriptionDeclaresRepeatableOption(description))
        {
            return true;
        }

        var optionPattern = $@"(?<![\w-]){Regex.Escape(switchName)}(?![\w-])";
        var lines = helpText.ReplaceLineEndings("\n").Split('\n');

        for (var index = 0; index < lines.Length; index++)
        {
            if (!OptionLinePattern().IsMatch(lines[index])
                || !Regex.IsMatch(lines[index], optionPattern, RegexOptions.IgnoreCase))
            {
                continue;
            }

            var end = index + 1;
            while (end < lines.Length
                   && !string.IsNullOrWhiteSpace(lines[end])
                   && !OptionLinePattern().IsMatch(lines[end]))
            {
                end++;
            }

            if (RepeatableValuePattern().IsMatch(string.Join('\n', lines[index..end])))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns whether an option description identifies a repeatable value.
    /// </summary>
    protected static bool DescriptionDeclaresRepeatableOption(string description) =>
        RepeatableValuePattern().IsMatch(description);

    /// <summary>
    /// Parses indentation-based argument declarations into a reusable nested group model.
    /// The adapter only recognizes one tool-specific declaration line; traversal,
    /// documentation boundaries, group classification, and flattening stay shared.
    /// </summary>
    protected static CliArgumentGroup ParseArgumentGroups(
        string section,
        Func<string, CliArgumentDefinition?> parseArgument) =>
        CliArgumentGroupParser.Parse(section, parseArgument);

    private static void ValidateOptionShapes(CliCommandDefinition command, string helpText)
    {
        foreach (var option in command.Options)
        {
            var description = option.Description ?? string.Empty;
            if (HelpDeclaresExplicitBooleanValue(description) && option.IsFlag)
            {
                throw new InvalidOperationException(
                    $"{command.FullCommand} {option.SwitchName} declares explicit true/false values, "
                    + "but the parsed model marks it as a presence-only flag.");
            }

            if (!option.IsFlag
                && HelpDeclaresRepeatableOption(helpText, option.SwitchName, description)
                && !option.AcceptsMultipleValues)
            {
                throw new InvalidOperationException(
                    $"{command.FullCommand} {option.SwitchName} is documented as repeatable, "
                    + "but the parsed model is scalar.");
            }
        }
    }

    private static void ValidateArgumentGroups(CliCommandDefinition command)
    {
        if (command.ArgumentGroups.Count == 0)
        {
            return;
        }

        var emittedSwitches = command.Options
            .Select(option => option.SwitchName)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var missingSwitches = command.ArgumentGroups
            .SelectMany(group => group.FlattenArguments())
            .Select(argument => argument.SwitchName)
            .Where(switchName => !emittedSwitches.Contains(switchName))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (missingSwitches.Length != 0)
        {
            throw new InvalidOperationException(
                $"{command.FullCommand} declares grouped arguments that were swallowed or omitted: "
                + string.Join(", ", missingSwitches));
        }
    }

    /// <summary>
    /// Pattern to match option lines (e.g., "-f, --flag" or "--option").
    /// </summary>
    [GeneratedRegex(
        @"^[ \t]*(?:-\w(?:[ \t]+[^,\s]+)?[ \t]*,[ \t]*)?--[\w-]+(?:[ \t]|,|$)",
        RegexOptions.Multiline)]
    protected static partial Regex OptionLinePattern();

    [GeneratedRegex(
        @"^[ \t]*Usage:?[ \t]*(?:[^\r\n]*\r?\n[ \t]*){0,2}[^\r\n]*(?:<command>|\[command\])[^\r\n]*\r?$",
        RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex CommandGroupUsagePattern();

    [GeneratedRegex(@"^[ \t]*[A-Z][A-Z0-9 _/-]*COMMANDS:?[ \t]*\r?$", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex CommandSectionHeadingPattern();

    [GeneratedRegex(
        @"(?:[\[{(<]\s*true\s*(?:\||/|or)\s*false\s*[\]})>]|(?:boolean|bool)\s+value|true\s+or\s+false|allowed\s+values?\s*:\s*(?:true\s*,\s*false|false\s*,\s*true)(?=\s*(?:[.)]|$)))",
        RegexOptions.IgnoreCase)]
    private static partial Regex ExplicitBooleanValuePattern();

    [GeneratedRegex(
        @"\b(?:one\s+or\s+more|zero\s+or\s+more|multiple\s+(?:times|values)|more\s+than\s+once|repeat(?:able|ed|edly)?)\b",
        RegexOptions.IgnoreCase)]
    private static partial Regex RepeatableValuePattern();

    [GeneratedRegex(@"\x1B(?:\][^\x07\x1B]*(?:\x07|\x1B\\)|\[[0-?]*[ -/]*[@-~])")]
    private static partial Regex AnsiEscapeSequencePattern();

    [GeneratedRegex(@".\x08")]
    private static partial Regex ManPageOverstrikePattern();

    #endregion
}
