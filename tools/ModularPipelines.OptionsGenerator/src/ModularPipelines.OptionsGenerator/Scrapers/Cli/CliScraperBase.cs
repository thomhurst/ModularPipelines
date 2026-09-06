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
    private static readonly string[] DefaultUsageSynopsisHeadings = ["usage"];
    private const int TabWidth = 8;
    private readonly CliScrapeProvenance _scrapeProvenance = new();
    private readonly HashSet<string> _knownCommandGroups = new(StringComparer.OrdinalIgnoreCase);

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
    /// Section headings that can introduce positional-operand syntax.
    /// </summary>
    protected virtual IReadOnlyList<string> UsageSynopsisHeadings => DefaultUsageSynopsisHeadings;

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
    /// Returns whether tool-specific syntax proves an option is scalar despite repeatability prose
    /// elsewhere in the same help block.
    /// </summary>
    protected virtual bool ShouldTreatOptionAsScalar(
        IReadOnlyList<string> commandParts,
        string switchName) => false;

    /// <summary>
    /// Global options that are documented but absent from the installed CLI's help output.
    /// </summary>
    protected virtual IReadOnlyList<CliOptionDefinition> SupplementalGlobalOptions => [];

    /// <summary>
    /// Gets whether inherited tool-wide options must be emitted before subcommands.
    /// </summary>
    protected virtual bool GlobalOptionsBeforeSubcommands => true;

    /// <summary>
    /// Gets whether generic command-group operands remain executable arguments after child discovery.
    /// </summary>
    protected virtual bool PreserveCommandGroupPlaceholders => false;

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

        if (path.Length == 1)
        {
            GlobalOptions = ParseGlobalOptions(helpText);
        }

        if (ShouldSkipPath(path, helpText))
        {
            _scrapeProvenance.DiscardLeafHelp(path);
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
        var declaresCommandGroup = HelpDeclaresCommandGroup(helpText);
        PreserveGroupHelp(path, subcommands, declaresCommandGroup);
        if (!TryValidateSubcommandDiscovery(path, helpText, subcommands))
        {
            return;
        }

        await ParseAndWriteCommandAsync(path, helpText, subcommands, commandChannel, cancellationToken);
        DiscardLeafHelp(path, subcommands, declaresCommandGroup);

        await EnqueueSubcommandsAsync(
            path,
            subcommands,
            workChannel,
            coordinator,
            visitedPaths,
            cancellationToken);
    }

    private void PreserveGroupHelp(
        string[] path,
        IReadOnlyCollection<string> subcommands,
        bool declaresCommandGroup)
    {
        if (subcommands.Count > 0 || declaresCommandGroup)
        {
            _scrapeProvenance.PreserveGroupHelp(path);
        }
    }

    private bool TryValidateSubcommandDiscovery(
        string[] path,
        string helpText,
        IReadOnlyCollection<string> subcommands)
    {
        try
        {
            ValidateSubcommandDiscovery(path, helpText, subcommands);
            return true;
        }
        catch (Exception ex) when (ex is not (OutOfMemoryException or StackOverflowException))
        {
            Logger.LogWarning(ex, "Failed to validate subcommand discovery: {Command}", string.Join(" ", path));
            return false;
        }
    }

    private void DiscardLeafHelp(
        string[] path,
        IReadOnlyCollection<string> subcommands,
        bool declaresCommandGroup)
    {
        if (subcommands.Count == 0
            && !declaresCommandGroup
            && !_knownCommandGroups.Contains(string.Join(' ', path)))
        {
            _scrapeProvenance.DiscardLeafHelp(path);
        }
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
        LogUsageSynopsisSelection(path, usage);
        if (ShouldSkipCommand(path, helpText, subcommands, usage))
        {
            return;
        }

        // Once child commands have been discovered, generic Command/Subcommand operands
        // select one of those children rather than representing an executable argument.
        // Handle this centrally so individual adapters cannot leave synthetic operands on
        // command groups such as docker compose, docker context, or minikube addons.
        if (!PreserveCommandGroupPlaceholders
            && subcommands.Any(IsTraversableSubcommand))
        {
            usage = UsageSynopsisParser.RemoveCommandGroupPlaceholders(usage);
        }

        var command = await TryParseCommandAsync(path, helpText, usage, cancellationToken);
        if (command is null)
        {
            return;
        }

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
            usage = NormalizeUsageSynopsis(command, usage);
            command = command with
            {
                HasOperandTakingUsage = usage.HasOperandTokens,
                UsagePositionalArguments = usage.PositionalArguments,
                RequiredAlternativeGroups = ResolveRequiredAlternativeGroups(command, usage),
            };
            command.ValidateOperandCoverage(
                usage.HasOperandTokens,
                usage.Synopsis,
                usage.PositionalArguments);
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
            if (!IsTraversableSubcommand(subcommand))
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

    private bool IsTraversableSubcommand(string subcommand) =>
        IsValidDiscoveredSubcommand(subcommand) && !IsSkippableSubcommand(subcommand);

    /// <summary>
    /// Validates a subcommand name before traversal queues its command path.
    /// </summary>
    protected virtual bool IsValidDiscoveredSubcommand(string subcommand) =>
        !string.IsNullOrWhiteSpace(subcommand);

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
            GlobalOptions = GlobalOptions,
            SupplementalGlobalOptions = SupplementalGlobalOptions,
            GlobalOptionsBeforeSubcommands = GlobalOptionsBeforeSubcommands,
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
            if (!string.IsNullOrEmpty(cached))
            {
                _scrapeProvenance.RecordCacheHit(commandPath, cached);
            }

            return cached;
        }

        // Build the arguments: everything after the tool name, plus --help
        var args = commandPath.Length > 1
            ? string.Join(" ", commandPath.Skip(1)) + " --help"
            : "--help";

        var result = await ExecuteAndRecordHelpCommandAsync(
            commandPath,
            ExecutablePath,
            args,
            cancellationToken);

        if (!ShouldAcceptHelpResult(commandPath, result))
        {
            Logger.LogWarning(
                "Ignoring failed help command for {Command}; exit code {ExitCode}",
                cacheKey,
                result.ExitCode);
            return null;
        }

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

    private protected async Task<CliCommandResult> ExecuteAndRecordHelpCommandAsync(
        IReadOnlyList<string> commandPath,
        string executablePath,
        string arguments,
        CancellationToken cancellationToken,
        string? workingDirectory = null,
        bool preserveRawHelp = false)
    {
        var result = await Executor.ExecuteAsync(
            executablePath,
            arguments,
            cancellationToken,
            workingDirectory);
        _scrapeProvenance.Record(commandPath, arguments, result, preserveRawHelp);
        return result;
    }

    internal Task<string?> WriteCoverageFailureDiagnosticsAsync(
        string outputDirectory,
        CommandCoverageEvaluation coverage,
        CancellationToken cancellationToken) =>
        _scrapeProvenance.WriteCoverageFailureDiagnosticsAsync(
            outputDirectory,
            coverage,
            cancellationToken);

    internal void PreserveRawHelpForCommandGroups(IEnumerable<string> commandGroups)
    {
        _knownCommandGroups.Clear();
        _knownCommandGroups.UnionWith(commandGroups);
    }

    /// <summary>
    /// Returns whether output from a help invocation is safe to parse.
    /// Some CLIs intentionally return non-zero exit codes for valid help, so adapters
    /// can opt into stricter validation when partial failure output is misleading.
    /// </summary>
    protected virtual bool ShouldAcceptHelpResult(
        IReadOnlyList<string> commandPath,
        CliCommandResult result) => true;

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
            GetAdditionalUsageSynopses(commandPath, helpText),
            UsageSynopsisHeadings);

    /// <summary>
    /// Lets a tool associate ambiguous usage operands with named options using its help metadata.
    /// </summary>
    protected virtual UsageSynopsisParseResult NormalizeUsageSynopsis(
        CliCommandDefinition command,
        UsageSynopsisParseResult usage) =>
        usage;

    private static IReadOnlyList<CliRequiredAlternativeGroup> ResolveRequiredAlternativeGroups(
        CliCommandDefinition command,
        UsageSynopsisParseResult usage)
    {
        if (usage.RequiredAlternativeGroups.Count == 0)
        {
            return command.RequiredAlternativeGroups;
        }

        return
        [
            .. command.RequiredAlternativeGroups,
            .. usage.RequiredAlternativeGroups
                .Select(group => TryResolveRequiredAlternativeGroup(command, group))
                .OfType<CliRequiredAlternativeGroup>(),
        ];
    }

    private static CliRequiredAlternativeGroup? TryResolveRequiredAlternativeGroup(
        CliCommandDefinition command,
        UsageRequiredAlternativeGroup group)
    {
        var members = group.Members
            .Select(member => TryResolveRequiredAlternativeMember(command, member))
            .ToArray();
        if (members.Any(static member => member is null))
        {
            // Synopsis inference can reference an inherited, global, or filtered switch.
            // Discard that inferred constraint without dropping the command itself.
            return null;
        }

        return new CliRequiredAlternativeGroup
        {
            Members = members
                .Select(static member => member!)
                .DistinctBy(GetRequiredAlternativeIdentity, StringComparer.Ordinal)
                .ToArray(),
        };
    }

    private static CliRequiredAlternativeMember? TryResolveRequiredAlternativeMember(
        CliCommandDefinition command,
        UsageRequiredAlternativeMember member)
    {
        if (member.OptionSwitch is { } optionSwitch)
        {
            var optionIndex = CliOptionDefinition.FindIndexBySwitch(command.Options, optionSwitch);
            if (optionIndex < 0)
            {
                return null;
            }

            return new CliRequiredAlternativeMember
            {
                PropertyName = command.Options[optionIndex].PropertyName,
                OptionSwitch = command.Options[optionIndex].SwitchName,
            };
        }

        if (member.PositionalPropertyName is { } positionalPropertyName)
        {
            var argumentIndex = Enumerable.Range(0, command.PositionalArguments.Count).FirstOrDefault(index =>
                command.PositionalArguments[index].PropertyName.Equals(
                    positionalPropertyName,
                    StringComparison.OrdinalIgnoreCase),
                -1);
            if (argumentIndex < 0)
            {
                return null;
            }

            return new CliRequiredAlternativeMember
            {
                PropertyName = command.PositionalArguments[argumentIndex].PropertyName,
                PositionalArgumentPhase = command.PositionalArguments[argumentIndex].Phase,
                PositionalArgumentPositionIndex = command.PositionalArguments[argumentIndex].PositionIndex,
            };
        }

        return null;
    }

    private static string GetRequiredAlternativeIdentity(CliRequiredAlternativeMember member) =>
        member.OptionSwitch is { } optionSwitch
            ? $"option:{optionSwitch}"
            : $"operand:{member.PositionalArgumentPhase}:{member.PositionalArgumentPositionIndex}";

    /// <summary>
    /// Returns true positional operands, excluding values syntactically owned by an option switch.
    /// </summary>
    protected static IReadOnlyList<CliPositionalArgument> GetPositionalArguments(
        UsageSynopsisParseResult usage) =>
        usage.PositionalArguments
            .Where(argument => argument.AssociatedOptionSwitch is null)
            .ToArray();

    /// <summary>
    /// Returns true positional operands, retaining operands that follow presence-only flags.
    /// </summary>
    protected static IReadOnlyList<CliPositionalArgument> GetPositionalArguments(
        UsageSynopsisParseResult usage,
        IReadOnlyList<CliOptionDefinition> options) =>
        usage.PositionalArguments
            .Where(argument => argument.AssociatedOptionSwitch is null
                               || !options.Any(option =>
                                   !option.IsFlag
                                   && (option.SwitchName.Equals(
                                           argument.AssociatedOptionSwitch,
                                           StringComparison.OrdinalIgnoreCase)
                                       || option.ShortForm?.Equals(
                                           argument.AssociatedOptionSwitch,
                                           StringComparison.OrdinalIgnoreCase) == true)))
            .Select(argument => argument with { AssociatedOptionSwitch = null })
            .ToArray();

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
    protected internal static bool HelpDeclaresRepeatableOption(
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
            var declaration = lines[index];
            if (!OptionLinePattern().IsMatch(declaration)
                || !Regex.IsMatch(declaration, optionPattern, RegexOptions.IgnoreCase))
            {
                continue;
            }

            // Blank lines and option rows bound the block, never indentation: gcloud puts
            // repeatability notes at the flag column, and column-0 headers rely on blank
            // separation as before. Wrapped prose that starts with a switch is kept only once the
            // row's inline prose has fixed the description column; this lookahead does not know
            // the tool's layout, so while the column is unknown any option-looking line ends the
            // block (a sibling row, a nested row, or a one-word description's neighbour alike),
            // and plain prose beneath a descriptionless row establishes the column instead.
            var descriptionColumn = GetInlineDescriptionColumn(declaration);
            var start = index;
            while (index + 1 < lines.Length)
            {
                var candidate = lines[index + 1];
                var looksLikeOptionRow = OptionLinePattern().IsMatch(candidate);
                if ((looksLikeOptionRow && descriptionColumn is null)
                    || !IsContinuationLine(candidate, declarationIndentation: null, descriptionColumn, looksLikeOptionRow))
                {
                    break;
                }

                index++;
                descriptionColumn ??= GetIndentation(candidate);
            }

            if (RepeatableValuePattern().IsMatch(string.Join('\n', lines, start, index - start + 1)))
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
    /// Returns whether a value-taking, non-Boolean option is described as repeatable.
    /// </summary>
    protected static bool IsRepeatableValueOption(
        string description,
        bool isFlag,
        bool isBoolean = false) =>
        !isFlag && !isBoolean && DescriptionDeclaresRepeatableOption(description);

    /// <summary>
    /// Converts a scalar C# type into its repeatable collection representation when needed.
    /// </summary>
    protected static string AsCSharpType(string scalarType, bool acceptsMultipleValues) =>
        acceptsMultipleValues
            ? $"IEnumerable<{scalarType.TrimEnd('?')}>?"
            : scalarType;

    /// <summary>
    /// Counts the leading whitespace columns of a help line. A tab advances to the next
    /// eight-column stop, matching how the terminal rendered the aligned help text.
    /// </summary>
    protected internal static int GetIndentation(string line)
    {
        var contentIndex = line.AsSpan().IndexOfAnyExcept(' ', '\t');
        return GetColumn(line, contentIndex < 0 ? line.Length : contentIndex);
    }

    /// <summary>
    /// Returns the rendered column at which character <paramref name="index"/> of
    /// <paramref name="line"/> starts, expanding tabs to eight-column stops.
    /// </summary>
    protected internal static int GetColumn(string line, int index)
    {
        var column = 0;
        var end = Math.Min(index, line.Length);
        for (var position = 0; position < end; position++)
        {
            column = line[position] == '\t'
                ? column + TabWidth - (column % TabWidth)
                : column + 1;
        }

        return column;
    }

    /// <summary>
    /// Returns whether <paramref name="line"/> continues the description of the option
    /// declared at <paramref name="declarationIndentation"/> instead of starting the next
    /// help row. Formatters wrap prose at or beyond the block's description column, so a
    /// row that looks like an option declaration but starts at or after that column is
    /// still wrapped prose (for example a wrapped mention of <c>--flag=value</c>).
    /// </summary>
    /// <param name="line">The candidate continuation line.</param>
    /// <param name="declarationIndentation">
    /// Column where the option declaration starts, or <see langword="null"/> when only blank
    /// lines and option rows bound the block.
    /// </param>
    /// <param name="descriptionColumn">
    /// Column where the declaration's inline description starts, or <see langword="null"/>
    /// when the description only begins on a following line. Until that column is known any
    /// line deeper than the declaration is accepted, because the first wrapped line is what
    /// establishes the column.
    /// </param>
    /// <param name="looksLikeOptionRow">Whether the scraper's option pattern matches <paramref name="line"/>.</param>
    protected internal static bool IsContinuationLine(
        string line,
        int? declarationIndentation,
        int? descriptionColumn,
        bool looksLikeOptionRow)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            return false;
        }

        var indentation = GetIndentation(line);
        var wrappedAtDescriptionColumn = descriptionColumn is null || indentation >= descriptionColumn;
        return (!looksLikeOptionRow || wrappedAtDescriptionColumn)
               && (declarationIndentation is not { } floor || indentation > floor);
    }

    /// <summary>
    /// Returns the column where an option row's inline description starts, or
    /// <see langword="null"/> when the row carries no description.
    /// </summary>
    private static int? GetDescriptionColumn(string declaration, Group? inlineDescription)
    {
        if (inlineDescription is not { } group || string.IsNullOrWhiteSpace(group.Value))
        {
            return null;
        }

        var leadingWhitespace = group.Value.Length - group.Value.TrimStart().Length;
        return GetColumn(declaration, group.Index + leadingWhitespace);
    }

    /// <summary>
    /// Returns the column where a generic option row's inline description starts, or
    /// <see langword="null"/> when the row carries no prose. The row is split into segments at
    /// runs of two or more blanks or at a single tab; switch segments and single-token value
    /// hints that are followed by more text are skipped, so a padded hint
    /// (<c>--env  stringArray   Set …</c>), a second switch form
    /// (<c>-i CODES    --include=CODES    Consider …</c>) and a tab-aligned row
    /// (<c>\t--env stringArray\tSet …</c>) all resolve to the prose column.
    /// </summary>
    protected internal static int? GetInlineDescriptionColumn(string line)
    {
        var position = line.Length - line.TrimStart().Length;
        if (position == line.Length)
        {
            return null;
        }

        var segments = new List<(int Start, string Text)>();
        foreach (Match separator in InlineSegmentSeparatorPattern().Matches(line, position))
        {
            segments.Add((position, line[position..separator.Index]));
            position = separator.Index + separator.Length;
        }

        segments.Add((position, line[position..].TrimEnd()));

        foreach (var (start, text) in segments)
        {
            var isSwitch = text.Length == 0 || text[0] == '-';
            if (!isSwitch && !LooksLikeValueHint(text))
            {
                return GetColumn(line, start);
            }
        }

        return null;
    }

    /// <summary>
    /// Returns whether a row segment is a value hint rather than prose. Prose contains blanks; a
    /// single token (<c>stringArray</c>, <c>String</c>, <c>&lt;value&gt;</c>, <c>PATH</c>) is a
    /// hint wherever it sits, so a row whose description starts on the next line leaves the
    /// column unknown until that line establishes it. A one-word description is only misread
    /// when something wraps beneath it, and then the wrapped line sets the column instead.
    /// </summary>
    private static bool LooksLikeValueHint(string text) => !text.Any(char.IsWhiteSpace);

    /// <summary>
    /// Joins an option row's inline description with the prose wrapped beneath it, advancing
    /// <paramref name="declarationIndex"/> past every consumed line so callers never re-read
    /// wrapped prose as a declaration. Returns an empty string when the row has no description.
    /// </summary>
    /// <param name="lines">The help text lines.</param>
    /// <param name="declarationIndex">Index of the option row; advanced to the last consumed line.</param>
    /// <param name="inlineDescription">
    /// Regex group holding the row's inline description, or <see langword="null"/> when the
    /// scraper did not capture one.
    /// </param>
    /// <param name="looksLikeOptionRow">Returns whether a line matches the scraper's option pattern.</param>
    protected internal static string AccumulateWrappedDescription(
        IReadOnlyList<string> lines,
        ref int declarationIndex,
        Group? inlineDescription,
        Func<string, bool> looksLikeOptionRow)
    {
        var declaration = lines[declarationIndex];
        var declarationIndentation = GetIndentation(declaration);
        var descriptionColumn = GetDescriptionColumn(declaration, inlineDescription);
        var parts = new List<string>();
        if (descriptionColumn is not null && inlineDescription is { } group)
        {
            parts.Add(group.Value.Trim());
        }

        while (declarationIndex + 1 < lines.Count)
        {
            var candidate = lines[declarationIndex + 1];
            if (!IsContinuationLine(
                    candidate,
                    declarationIndentation,
                    descriptionColumn,
                    looksLikeOptionRow(candidate)))
            {
                break;
            }

            parts.Add(candidate.Trim());
            declarationIndex++;

            // A row whose prose only starts on the next line (picocli, argparse, git) reveals its
            // description column there, so later wrapped lines get the same column-aware rule.
            descriptionColumn ??= GetIndentation(candidate);
        }

        return string.Join(' ', parts);
    }

    /// <summary>
    /// Parses indentation-based argument declarations into a reusable nested group model.
    /// The adapter only recognizes one tool-specific declaration line; traversal,
    /// documentation boundaries, group classification, and flattening stay shared.
    /// </summary>
    protected static CliArgumentGroup ParseArgumentGroups(
        string section,
        Func<string, CliArgumentDefinition?> parseArgument) =>
        CliArgumentGroupParser.Parse(section, parseArgument);

    private void ValidateOptionShapes(CliCommandDefinition command, string helpText)
    {
        foreach (var option in command.Options)
        {
            var description = option.Description ?? string.Empty;
            var isBoolean = option.CSharpType is "bool" or "bool?";
            if (HelpDeclaresExplicitBooleanValue(description) && option.IsFlag)
            {
                throw new InvalidOperationException(
                    $"{command.FullCommand} {option.SwitchName} declares explicit true/false values, "
                    + "but the parsed model marks it as a presence-only flag.");
            }

            if (!option.IsFlag
                && !isBoolean
                && HelpDeclaresRepeatableOption(helpText, option.SwitchName, description)
                && !ShouldTreatOptionAsScalar(command.CommandParts, option.SwitchName)
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
        @"^[ \t]*(?:-\w(?:[ \t]+[^,\s]+)?[ \t]*,[ \t]*)?--[\w-]+(?:[ \t]|,|=|$)",
        RegexOptions.Multiline)]
    protected static partial Regex OptionLinePattern();

    /// <summary>
    /// Separates the segments of a generic option row: a run of two or more blanks, or a
    /// single tab, which tab-aligned help uses as its column separator.
    /// </summary>
    [GeneratedRegex(@"[ \t]{2,}|\t")]
    private static partial Regex InlineSegmentSeparatorPattern();

    [GeneratedRegex(
        @"^[ \t]*Usage:?[ \t]*(?:[^\r\n]*\r?\n[ \t]*){0,2}[^\r\n]*(?:<command>|\[command\])[^\r\n]*\r?$",
        RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex CommandGroupUsagePattern();

    [GeneratedRegex(@"^[ \t]*[A-Z][A-Z0-9 _/-]*COMMANDS:?[ \t]*\r?$", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex CommandSectionHeadingPattern();

    [GeneratedRegex(
        @"(?:[\[{(<]\s*true\s*(?:\||/|or)\s*false\s*[\]})>]|(?:boolean|bool)\s+value|true\s+or\s+false|allowed\s+values?\s*:\s*(?:(?:true\s*,\s*false|false\s*,\s*true)|(?:0\s*,\s*1\s*,\s*f\s*,\s*false\s*,\s*n\s*,\s*no\s*,\s*t\s*,\s*true\s*,\s*y\s*,\s*yes))(?=\s*(?:[.)]|$)))",
        RegexOptions.IgnoreCase)]
    private static partial Regex ExplicitBooleanValuePattern();

    private const string OperationalCountPhrasePattern =
        @"(?:[\w-]+\s+){0,2}(?:attempts?|times?|retries?)\b";

    private const string RepeatableItemCountPattern =
        @"(?:one|zero)\s+or\s+more\s+(?!" + OperationalCountPhrasePattern + @")[\w-]+";

    private const string RepeatableValueRegex =
        @"\b(?:"
        + @"repeatable"
        + @"|(?:can|may|must|should)\s+be\s+repeated"
        + @"|(?:is|are)\s+repeated"
        + @"|multiples?\s+(?:are\s+)?supported\s+by\s+passing\s+--?[\w-]+\s+multiple\s+times"
        + @"|\A" + RepeatableItemCountPattern
        + @"|(?:specifications?|lists?)\s+of\s+" + RepeatableItemCountPattern
        + @"|(?:can|may|must|should)\s+be\s+"
        + @"(?:specified|supplied|provided|used|passed|set|given)\s+"
        + @"(?:(?:one|zero)\s+or\s+more\s+times|multiple\s+times|more\s+than\s+once)"
        + @"|(?:accepts?|specify|supply|provide|use|pass|set|give|supports?|takes?|contains?)\s+"
        + @"(?:multiple\s+times|more\s+than\s+once|"
        + RepeatableItemCountPattern
        + @"|multiple\s+[\w-]+)"
        + @"|(?:an?\s+)?array\s+of\s+[\w-]+)\b";

    [GeneratedRegex(RepeatableValueRegex, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex RepeatableValuePattern();

    [GeneratedRegex(@"\x1B(?:\][^\x07\x1B]*(?:\x07|\x1B\\)|\[[0-?]*[ -/]*[@-~])")]
    private static partial Regex AnsiEscapeSequencePattern();

    [GeneratedRegex(@".\x08")]
    private static partial Regex ManPageOverstrikePattern();

    #endregion
}
