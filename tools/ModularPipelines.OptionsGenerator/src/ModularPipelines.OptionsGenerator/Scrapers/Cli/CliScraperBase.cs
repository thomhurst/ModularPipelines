using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
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
    private static readonly IReadOnlySet<string> DefaultIgnoredOptionSwitches =
        new[] { "--help" }.ToFrozenSet(StringComparer.Ordinal);
    private const int TabWidth = 8;
    private readonly CliScrapeProvenance _scrapeProvenance = new();
    private readonly HashSet<string> _knownCommandGroups = [with(StringComparer.OrdinalIgnoreCase)];
    private IReadOnlyList<CliOptionDefinition> _unfilteredGlobalOptions = [];

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
    /// Exact option switches and aliases excluded from generated options. Short switches are
    /// tool-specific: for example, <c>-h</c> can mean hostname instead of help.
    /// </summary>
    protected virtual IReadOnlySet<string> IgnoredOptionSwitches => DefaultIgnoredOptionSwitches;

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
        CliGlobalOptionMerger.Merge(GlobalOptions, FilterIgnoredOptions(SupplementalGlobalOptions));

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
    private sealed class WorkCoordinator(Channel<string[]> workChannel)
    {
        private int _outstandingWork;
        private readonly Channel<string[]> _workChannel = workChannel;

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
        if (Logger.IsEnabled(LogLevel.Information))
        {
            Logger.LogInformation("Discovering {Tool} commands via CLI (executable: {Path}, parallelism: {Parallelism})...",
                ToolName, ExecutablePath, MaxParallelism);
        }

        // Check availability first
        if (!await IsAvailableAsync(cancellationToken))
        {
            // The probe only exposes a boolean, so preserve the unavailable root without
            // inventing a timeout or a raw help response that was never observed.
            _scrapeProvenance.Record([ToolName], VersionArguments, new CliCommandResult
            {
                ExitCode = -1,
                ExecutionFailed = true,
                StandardOutput = string.Empty,
                StandardError = "The traversal availability probe failed.",
            });
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
            if (Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation("Yielding command {Count}: {Command}", commandCount, command.FullCommand);
            }
            yield return command;
        }

        if (Logger.IsEnabled(LogLevel.Information))
        {
            Logger.LogInformation("Finished scraping {Tool}. Total commands: {Count}", ToolName, commandCount);
        }
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
            _unfilteredGlobalOptions = ParseGlobalOptions(helpText);
            GlobalOptions = FilterIgnoredOptions(_unfilteredGlobalOptions);
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
        List<string> subcommands,
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
        List<string> subcommands,
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

        if (Logger.IsEnabled(LogLevel.Debug))
        {
            Logger.LogDebug("Skipping command based on help text filter: {Command}", string.Join(" ", path));
        }
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

            usage = NormalizeUsageSynopsis(command, usage);
            usage = UsageSynopsisParser.ResolveOptionUsage(usage, GetUsageOptions(command.Options));
            var requiredAlternatives = ResolveRequiredAlternativeGroups(command, usage);
            usage = RemoveIgnoredOptionValues(usage, command.Options);
            command = command with
            {
                UsageSynopsis = usage.Synopsis,
                HasOperandTakingUsage = usage.HasOperandTokens,
                UsagePositionalArguments = usage.PositionalArguments,
                RequiredAlternativeGroups = requiredAlternatives,
            };
            // Resolve choices against all parsed switches before pruning ignored alternatives.
            command = ApplyIgnoredOptionPolicy(command);
            ValidateOptionShapes(command, helpText);
            ValidateArgumentGroups(command);
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

        if (Logger.IsEnabled(LogLevel.Debug))
        {
            Logger.LogDebug(
                "Selected usage synopsis for {Command} from {Count} matching candidates: {Synopsis}",
                string.Join(" ", commandPath),
                usage.MatchedSynopsisCount,
                usage.Synopsis);
        }
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
            SupplementalGlobalOptions = FilterIgnoredOptions(SupplementalGlobalOptions),
            GlobalOptionsBeforeSubcommands = GlobalOptionsBeforeSubcommands,
            Errors = []
        };
    }

    /// <inheritdoc />
    public virtual Task<CliToolDefinition> CreateToolDefinitionAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(CreateToolDefinition());

    #endregion

    #region Help Text & Discovery

    private bool IsIgnoredOption(CliOptionDefinition option) =>
        option.GetSwitchNames().Any(IgnoredOptionSwitches.Contains);

    private IReadOnlyList<CliOptionDefinition> FilterIgnoredOptions(IReadOnlyList<CliOptionDefinition> options) =>
        options.Any(IsIgnoredOption) ? [.. options.Where(option => !IsIgnoredOption(option))] : options;

    private UsageSynopsisParseResult RemoveIgnoredOptionValues(
        UsageSynopsisParseResult usage, IReadOnlyList<CliOptionDefinition> options)
    {
        // Retain ignored global metadata for usage ownership without emitting it. A local
        // definition takes precedence over inherited definitions of the same switch.
        CliOptionDefinition[] parsedOptions = [.. options, .. _unfilteredGlobalOptions, .. SupplementalGlobalOptions];
        if (!parsedOptions.Any(IsIgnoredOption))
        {
            return usage;
        }

        var arguments = new List<CliPositionalArgument>();
        foreach (var argument in usage.PositionalArguments)
        {
            var owner = argument.AssociatedOptionSwitch is { } optionSwitch
                ? CliOptionDefinition.FindIndexBySwitch(parsedOptions, optionSwitch)
                : -1;
            if (owner < 0 || !IsIgnoredOption(parsedOptions[owner]))
            {
                arguments.Add(argument);
            }
            else if (parsedOptions[owner].IsFlag)
            {
                // A token following a flag is an operand, not a value owned by that flag.
                arguments.Add(argument with { AssociatedOptionSwitch = null });
            }
        }

        return usage with
        {
            PositionalArguments = arguments,
            HasOperandTokens = usage.HasOperandTokens && (arguments.Count > 0 || usage.UnparsedOperandTokens.Count > 0),
        };
    }

    /// <summary>
    /// Applies the shared option policy after parsing, preserving operand inference and discarding
    /// metadata owned exclusively by ignored options.
    /// </summary>
    protected CliCommandDefinition ApplyIgnoredOptionPolicy(CliCommandDefinition command)
    {
        var options = FilterIgnoredOptions(command.Options);
        if (ReferenceEquals(options, command.Options))
        {
            return command;
        }

        var ignoredOptions = command.Options.Where(IsIgnoredOption).ToArray();
        var ignoredEnumNames = ignoredOptions.Where(option => option.EnumDefinition is not null)
            .Select(option => option.EnumDefinition!.EnumName).ToHashSet(StringComparer.Ordinal);
        ignoredEnumNames.ExceptWith(options.Where(option => option.EnumDefinition is not null)
            .Select(option => option.EnumDefinition!.EnumName));
        // Operands and explicitly typed options can refer to an enum without owning its definition.
        ignoredEnumNames.ExceptWith(options.Select(option => option.CSharpType)
            .Concat(command.PositionalArguments.Select(argument => argument.CSharpType))
            .SelectMany(type => type.Split(['<', '>', '?', '[', ']', ',', '.', ' '], StringSplitOptions.RemoveEmptyEntries)));
        var ignoredProperties = ignoredOptions.Select(option => option.PropertyName).ToHashSet(StringComparer.Ordinal);
        ignoredProperties.ExceptWith(options.Select(option => option.PropertyName));
        ignoredProperties.ExceptWith(command.PositionalArguments.Select(argument => argument.PropertyName));
        var ignoredSwitches = ignoredOptions.SelectMany(option => option.GetSwitchNames()).ToHashSet(StringComparer.Ordinal);
        ignoredSwitches.ExceptWith(options.SelectMany(option => option.GetSwitchNames()));

        return command with
        {
            Options = options,
            Enums = [.. command.Enums.Where(definition => !ignoredEnumNames.Contains(definition.EnumName))],
            ArgumentGroups = FilterIgnoredArgumentGroups(command.ArgumentGroups, ignoredSwitches),
            RequiredAlternativeGroups = FilterIgnoredRequiredAlternativeGroups(
                command.RequiredAlternativeGroups, ignoredSwitches, ignoredProperties),
        };
    }

    private static IReadOnlyList<CliRequiredAlternativeGroup> FilterIgnoredRequiredAlternativeGroups(
        IReadOnlyList<CliRequiredAlternativeGroup> groups,
        IReadOnlySet<string> ignoredSwitches,
        IReadOnlySet<string> ignoredProperties) =>
        [.. groups.Select(group => group with
        {
            Members = [.. group.Members.Where(member => member.OptionSwitch is { } optionSwitch
                ? !ignoredSwitches.Contains(optionSwitch)
                : member.PositionalArgumentPhase is not null || member.PositionalArgumentPositionIndex is not null
                    || !ignoredProperties.Contains(member.PropertyName))],
            Groups = FilterIgnoredRequiredAlternativeGroups(group.Groups, ignoredSwitches, ignoredProperties),
        }).Where(group => group.Members.Count > 0 || group.Groups.Count > 0)];

    private static IReadOnlyList<CliArgumentGroup> FilterIgnoredArgumentGroups(
        IReadOnlyList<CliArgumentGroup> groups, IReadOnlySet<string> ignoredSwitches) =>
        [.. groups.Select(group => group with
        {
            Arguments = [.. group.Arguments.Where(argument => !ignoredSwitches.Contains(argument.SwitchName))],
            Groups = FilterIgnoredArgumentGroups(group.Groups, ignoredSwitches),
        }).Where(group => group.Arguments.Count > 0 || group.Groups.Count > 0)];

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
            LogRejectedHelp(result, cacheKey, failedCommand: true);
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

        LogRejectedHelp(result, cacheKey);
        return null;
    }

    private protected void LogRejectedHelp(CliCommandResult result, string command, bool failedCommand = false)
    {
        // ExecuteAndRecordHelpCommandAsync already reports unavailable help. All adapters
        // share this gate for empty output and stricter exit-code rejection.
        if (result.Unavailable)
        {
            return;
        }

        if (failedCommand)
        {
            Logger.LogWarning("Ignoring failed help command for {Command}; exit code {ExitCode}",
                command, result.ExitCode);
        }
        else
        {
            Logger.LogWarning("No help text for command: {Command}", command);
        }
    }

    private protected async Task<CliCommandResult> ExecuteAndRecordHelpCommandAsync(
        IReadOnlyList<string> commandPath,
        string executablePath,
        string arguments,
        CancellationToken cancellationToken,
        string? workingDirectory = null,
        bool preserveRawHelp = false,
        CliHelpKind helpKind = CliHelpKind.Help)
    {
        var result = await Executor.ExecuteAsync(
            executablePath,
            arguments,
            cancellationToken,
            workingDirectory);
        _scrapeProvenance.Record(commandPath, arguments, result, preserveRawHelp, helpKind);
        if (!result.Unavailable)
        {
            return result;
        }

        // Every scraper's help parsing treats blank output as "no help", so hand back an empty
        // result instead of the executor's placeholder text. The provenance keeps the path as
        // unavailable, and coverage validation fails the run instead of reporting a removal.
        Logger.LogWarning(
            "Help for {Command} is unavailable in this scrape ({Reason})",
            string.Join(' ', commandPath),
            result.TimedOut ? "timed out after all retries"
                : result.CircuitOpen ? "rejected by the circuit breaker"
                : "the process could not be executed");
        return new CliCommandResult
        {
            StandardOutput = string.Empty,
            StandardError = string.Empty,
            ExitCode = result.ExitCode,
            TimedOut = result.TimedOut,
            CircuitOpen = result.CircuitOpen,
            ExecutionFailed = result.ExecutionFailed,
        };
    }

    internal Task<string?> WriteCoverageFailureDiagnosticsAsync(
        string outputDirectory,
        CommandCoverageEvaluation coverage,
        CancellationToken cancellationToken) =>
        _scrapeProvenance.WriteCoverageFailureDiagnosticsAsync(
            outputDirectory,
            coverage,
            cancellationToken);

    /// <summary>
    /// Help paths whose invocation timed out after every retry or was rejected by the circuit
    /// breaker, or whose process could not execute during this scrape.
    /// </summary>
    internal IReadOnlyList<string> UnavailableHelpPaths => _scrapeProvenance.UnavailableHelpPaths;

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

        var groups = command.RequiredAlternativeGroups.ToList();
        foreach (var inferred in usage.RequiredAlternativeGroups
                     .Select(group => TryResolveRequiredAlternativeGroup(command, group))
                     .OfType<CliRequiredAlternativeGroup>())
        {
            var identities = GetAlternativeGroupIdentities(inferred).ToHashSet(StringComparer.Ordinal);
            // A richer required help constraint already enforces presence over these members.
            // Optional help constraints cannot replace a synopsis requirement.
            if (inferred.IsUsageFormChoice
                || !groups.Any(group => group.IsRequired && identities.SetEquals(GetAlternativeGroupIdentities(group))))
            {
                groups.Add(inferred);
            }
        }

        return groups;
    }

    private static IEnumerable<string> GetAlternativeGroupIdentities(CliRequiredAlternativeGroup group) =>
        group.Members.Select(GetRequiredAlternativeIdentity)
            .Concat(group.Groups.SelectMany(GetAlternativeGroupIdentities));

    private static CliRequiredAlternativeGroup? TryResolveRequiredAlternativeGroup(
        CliCommandDefinition command,
        UsageRequiredAlternativeGroup group)
    {
        var members = group.Members
            .Select(member => TryResolveRequiredAlternativeMember(command, member))
            .ToArray();
        var groups = group.Groups.Select(nested => TryResolveRequiredAlternativeGroup(command, nested)).ToArray();
        if (members.Any(static member => member is null) || groups.Any(static nested => nested is null))
        {
            // Synopsis inference can reference an inherited, global, or filtered switch.
            // Discard that inferred constraint without dropping the command itself.
            return null;
        }

        return new CliRequiredAlternativeGroup
        {
            IsChoice = group.IsChoice,
            IsUsageFormChoice = group.IsChoice && group.Groups.Count > 0,
            Members = [.. members
                .Select(member => member! with { IsRequired = !group.IsChoice })
                .DistinctBy(GetRequiredAlternativeIdentity, StringComparer.Ordinal)],
            Groups = [.. groups.Select(static nested => nested!)],
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
    /// Returns true positional operands, retaining operands that follow presence-only flags.
    /// </summary>
    protected IReadOnlyList<CliPositionalArgument> GetPositionalArguments(
        UsageSynopsisParseResult usage,
        IReadOnlyList<CliOptionDefinition> options)
    {
        var usageOptions = GetUsageOptions(options);
        return [.. UsageSynopsisParser.ResolveOptionUsage(usage, usageOptions).PositionalArguments
            .Where(argument => UsageSynopsisParser.IsPositionalSlot(argument, usageOptions))
            .Select(argument => argument with { AssociatedOptionSwitch = null })];
    }

    private IReadOnlyList<CliOptionDefinition> GetUsageOptions(IReadOnlyList<CliOptionDefinition> options)
    {
        var globalOptions = EffectiveGlobalOptions;
        return globalOptions.Count == 0 ? options : [.. options, .. globalOptions];
    }

    /// <summary>
    /// Marks the options a usage synopsis lists outside every optional group with a required
    /// value, such as clap's <c>--sbom-format &lt;FORMAT&gt;</c> beside <c>[OPTIONS]</c>, as
    /// required, so the generated constructor demands them.
    /// </summary>
    protected static List<CliOptionDefinition> ApplyUsageRequiredOptions(
        List<CliOptionDefinition> options,
        UsageSynopsisParseResult usage)
    {
        var requiredSwitches = usage.PositionalArguments
            .Where(argument => argument.IsRequired && argument.AssociatedOptionSwitch is not null)
            .Select(argument => argument.AssociatedOptionSwitch!)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        if (requiredSwitches.Count == 0)
        {
            return options;
        }

        return [.. options
            .Select(option => requiredSwitches.Contains(option.SwitchName)
                              || (option.ShortForm is not null && requiredSwitches.Contains(option.ShortForm))
                ? option with { IsRequired = true }
                : option)];
    }

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
    private static readonly HashSet<string> DefaultSkipSubcommands =
    [with(StringComparer.OrdinalIgnoreCase), "help", "completion", "version", "__complete", "__completeNoDesc"];

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
    /// Recognizes an explicit required marker outside quoted examples in the description.
    /// </summary>
    protected static bool DescriptionDeclaresRequiredOption(string description) =>
        ExplicitRequiredOptionPattern().Matches(description).Any(match => match.Groups["required"].Success);

    /// <summary>
    /// Returns whether help describes an option as repeatable.
    /// </summary>
    protected internal static bool HelpDeclaresRepeatableOption(
        string helpText,
        string switchName,
        string description) =>
        DescriptionDeclaresRepeatableOption(description)
        || HelpOptionBlockMatches(helpText, switchName, RepeatableValuePattern());

    private protected static bool HelpOptionBlockMatches(string helpText, string switchName, Regex pattern) =>
        HelpOptionBlockMatches(helpText, switchName, pattern.IsMatch);

    private protected static bool HelpOptionBlockMatches(string helpText, string switchName, Func<string, bool> matches)
    {
        var optionPattern = $@"(?<![\w-]){Regex.Escape(switchName)}(?![\w-])";
        var lines = helpText.ReplaceLineEndings("\n").Split('\n');

        for (var index = 0; index < lines.Length; index++)
        {
            var declaration = lines[index];
            if (!OptionLinePattern().IsMatch(declaration))
            {
                continue;
            }

            // Blank lines and option rows bound the block, never indentation: gcloud puts
            // repeatability notes at the flag column. Section headings also end a block; blank
            // separation is retained. Wrapped prose that starts with a switch is kept only when it
            // sits at the description column: the row's own inline prose fixes that column, and a
            // descriptionless row borrows the column its own help section lays its descriptions
            // out at. While the column is still unknown any option-looking line ends the block (a
            // sibling row, a nested row, or a one-word description's neighbour alike).
            var declarationIndentation = GetIndentation(declaration);
            var inlineDescriptionColumn = GetInlineDescriptionColumn(declaration);
            var descriptionColumn = inlineDescriptionColumn
                                    ?? GetSectionDescriptionColumn(lines, index, declarationIndentation);
            var start = index;
            index = GetLastDescriptionLine(lines, index, declarationIndentation, descriptionColumn);

            // Consume every declaration's block before looking for the requested switch. A
            // wrapped reference inside another option must never become a new declaration.
            var optionMatch = Regex.Match(declaration, optionPattern, RegexOptions.IgnoreCase);
            if (optionMatch.Success
                && (inlineDescriptionColumn is null || GetColumn(declaration, optionMatch.Index) < inlineDescriptionColumn)
                && matches(string.Join('\n', lines, start, index - start + 1)))
            {
                return true;
            }
        }

        return false;
    }

    private static int GetLastDescriptionLine(string[] lines, int index, int declarationIndentation, int? descriptionColumn)
    {
        while (index + 1 < lines.Length)
        {
            var candidate = lines[index + 1];
            var looksLikeOptionRow = OptionLinePattern().IsMatch(candidate);
            if (IsHelpSectionHeading(candidate, declarationIndentation)
                || (looksLikeOptionRow && descriptionColumn is null)
                || !IsContinuationLine(candidate, declarationIndentation: null, descriptionColumn, looksLikeOptionRow,
                    index + 2 < lines.Length ? lines[index + 2] : null, lines[index]))
            {
                break;
            }

            index++;
            descriptionColumn ??= GetIndentation(candidate);
        }

        return index;
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
    /// switch mention starting at or after that column can still be wrapped prose. A row with
    /// its own separated description starts another declaration even at that column.
    /// </summary>
    /// <remarks>
    /// This single-line overload cannot inspect detached descriptions or reference introductions
    /// on surrounding lines. The help-text scans supply that context to the private overload.
    /// </remarks>
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
        bool looksLikeOptionRow) =>
        IsContinuationLine(line, declarationIndentation, descriptionColumn, looksLikeOptionRow, nextLine: null, previousLine: null);

    private static bool IsContinuationLine(
        string line,
        int? declarationIndentation,
        int? descriptionColumn,
        bool looksLikeOptionRow,
        string? nextLine,
        string? previousLine,
        Func<string, bool>? optionRowPredicate = null,
        bool allowSameColumnDescription = false,
        Func<string, Group?>? captureInlineDescription = null)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            return false;
        }

        if (looksLikeOptionRow && StartsNestedOptionDescription(line, nextLine, previousLine, optionRowPredicate, captureInlineDescription))
        {
            return false;
        }

        var indentation = GetIndentation(line);
        var wrappedAtDescriptionColumn = descriptionColumn is null || indentation >= descriptionColumn;
        return (!looksLikeOptionRow || wrappedAtDescriptionColumn)
               && (declarationIndentation is not { } floor || indentation > floor
                   || (allowSameColumnDescription && indentation == floor && !looksLikeOptionRow
                       && !IsHelpSectionHeading(line, floor)));
    }

    private static bool StartsNestedOptionDescription(
        string line, string? nextLine, string? previousLine, Func<string, bool>? optionRowPredicate,
        Func<string, Group?>? captureInlineDescription) =>
        GetRowDescriptionColumn(line, nextLine, optionRowPredicate, captureInlineDescription) is not null
        && (previousLine is null || !SwitchReferenceIntroductionPattern().IsMatch(previousLine));

    // Require a reference phrase, not a terminal connector such as "and" or "with":
    // ordinary parent prose can end with those words immediately before a nested declaration.
    [GeneratedRegex(@"\b(?:(?:combine[ds]?|pair(?:ed|s)?) with|values? from|for example)\s*:?\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex SwitchReferenceIntroductionPattern();

    private static int? GetSectionDescriptionColumn(string[] lines, int declarationIndex, int declarationIndentation)
    {
        var start = declarationIndex;
        while (start > 0 && !string.IsNullOrWhiteSpace(lines[start - 1])
               && !IsHelpSectionHeading(lines[start - 1], declarationIndentation))
        {
            start--;
        }

        var end = declarationIndex + 1;
        while (end < lines.Length && !string.IsNullOrWhiteSpace(lines[end])
               && !IsHelpSectionHeading(lines[end], declarationIndentation))
        {
            end++;
        }

        return GetLayoutDescriptionColumn(lines[start..end]);
    }

    private static bool IsHelpSectionHeading(string line, int declarationIndentation)
    {
        if (string.IsNullOrWhiteSpace(line)
            || GetIndentation(line) > declarationIndentation
            || OptionLinePattern().IsMatch(line))
        {
            return false;
        }

        var text = line.Trim();
        var heading = text.TrimEnd(':');
        // Sentence punctuation and capitalization alone do not turn a repeatability note
        // into a section. Custom colon-ended headings must have a title-shaped label.
        return NamedHelpSectionPattern().IsMatch(heading)
               || NamedOptionSectionPattern().IsMatch(heading)
               || (!DescriptionDeclaresRepeatableOption(text)
                   && text.EndsWith(':') && TitleHelpSectionPattern().IsMatch(heading));
    }

    [GeneratedRegex(@"^[A-Z][A-Za-z0-9/-]*(?:[ \t]+(?:[A-Z][A-Za-z0-9/-]*|and|or|of|for|the))*$", RegexOptions.CultureInvariant)]
    private static partial Regex TitleHelpSectionPattern();

    [GeneratedRegex(@"^(?:Usage|Synopsis|Description|Examples?|Environment(?: Variables)?|Notes?|See Also|Exit (?:Status|Codes?)|Commands)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex NamedHelpSectionPattern();

    [GeneratedRegex(@"^(?:[\w/]+[ \t]+)*(?:Flags|Options|Arguments)$", RegexOptions.IgnoreCase)]
    private static partial Regex NamedOptionSectionPattern();

    /// <summary>
    /// Returns the column the help text lays option descriptions out at: the most common column
    /// across its option rows, taking each row's inline prose column or, for a row without inline
    /// prose, the indentation of the plain prose line beneath it. <see langword="null"/> when no
    /// row establishes one.
    /// </summary>
    protected internal static int? GetLayoutDescriptionColumn(IReadOnlyList<string> lines)
    {
        var columns = new List<int>();
        for (var index = 0; index < lines.Count; index++)
        {
            var line = lines[index];
            if (!OptionLinePattern().IsMatch(line))
            {
                continue;
            }

            if (GetRowDescriptionColumn(line, index + 1 < lines.Count ? lines[index + 1] : null) is { } known)
            {
                columns.Add(known);
            }
        }

        return columns.Count == 0
            ? null
            : columns
                .GroupBy(column => column)
                .OrderByDescending(group => group.Count())
                .ThenBy(group => group.Key)
                .First()
                .Key;
    }

    private static int? GetRowDescriptionColumn(
        string line, string? nextLine, Func<string, bool>? optionRowPredicate = null,
        Func<string, Group?>? captureInlineDescription = null)
    {
        var capturedDescription = captureInlineDescription?.Invoke(line);
        var column = capturedDescription is null
            ? GetInlineDescriptionColumn(line)
            : GetCapturedDescriptionColumn(line, capturedDescription);
        if (column is null
            && !string.IsNullOrWhiteSpace(nextLine)
            && !(optionRowPredicate?.Invoke(nextLine) ?? OptionLinePattern().IsMatch(nextLine))
            && (GetIndentation(nextLine) > GetIndentation(line)
                || (GetIndentation(nextLine) == GetIndentation(line)
                    && (capturedDescription is not null || IsOptionDeclarationSegment(line.TrimStart())))))
        {
            column = GetIndentation(nextLine);
        }

        return column;
    }

    /// <summary>
    /// Returns the column where an option row's inline description starts, or
    /// <see langword="null"/> when the row carries no description.
    /// </summary>
    private static int? GetCapturedDescriptionColumn(string declaration, Group? inlineDescription)
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

        for (var index = 0; index < segments.Count; index++)
        {
            var (start, text) = segments[index];
            if (text.Length == 0 || (text[0] == '-' && (index == 0 || IsOptionDeclarationSegment(text))))
            {
                continue;
            }

            // A capitalized word in a padded value column can name a tool-specific type.
            // Require a later segment so terminal one-word descriptions remain prose.
            var isPaddedValueHint = index < segments.Count - 1
                                    && char.IsUpper(text[0])
                                    && text.All(char.IsLetter);
            if (!isPaddedValueHint && !LooksLikeValueHint(text))
            {
                return GetColumn(line, start);
            }
        }

        return null;
    }

    private static bool IsOptionDeclarationSegment(string text)
    {
        var option = OptionSegmentPrefixPattern().Match(text);
        if (!option.Success)
        {
            return false;
        }

        var remainder = text[option.Length..].Trim();
        return remainder.Length == 0 || LooksLikeValueHint(remainder);
    }

    [GeneratedRegex(@"^--?[\w-]+(?:[ \t]*,[ \t]*--?[\w-]+)*(?:[ \t=]+|$)")]
    private static partial Regex OptionSegmentPrefixPattern();

    /// <summary>
    /// Returns whether a row segment is a typed or syntactic value hint rather than prose.
    /// Single-word descriptions remain prose; recognized types and placeholder sequences
    /// such as <c>stringArray</c>, <c>String</c>, <c>KEY VALUE</c>, and <c>&lt;value&gt;</c>
    /// leave the column unknown until an inline or wrapped description establishes it.
    /// </summary>
    private static bool LooksLikeValueHint(string text) =>
        ValueTypeHintPattern().IsMatch(text)
        || text.Split((char[]?) null, StringSplitOptions.RemoveEmptyEntries)
            .All(static token => PlaceholderTokenPattern().IsMatch(token));

    [GeneratedRegex(@"^(?:bool(?:ean)?|byte|char|decimal|double|duration|float(?:32|64)?|u?int(?:8|16|32|64)?|integer|long|number|object|path|string|time|timestamp)(?:Array|Slice|s)?$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex ValueTypeHintPattern();

    [GeneratedRegex(@"^(?:<[^>]+>|\[[^\]]+\]|\{[^}]+\}|[A-Z][A-Z0-9_:.=/|,-]*|\.\.\.|…)(?:\.\.\.|…)?$")]
    private static partial Regex PlaceholderTokenPattern();

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
        Func<string, bool> looksLikeOptionRow) =>
        AccumulateWrappedDescription(lines, ref declarationIndex, inlineDescription, looksLikeOptionRow, null);

    /// <summary>
    /// Accumulates wrapped prose using caller captures to distinguish nested declarations
    /// with tool-specific value syntax from option references in prose.
    /// </summary>
    /// <param name="lines">The help text lines.</param>
    /// <param name="declarationIndex">Index of the option row; advanced past consumed prose.</param>
    /// <param name="inlineDescription">The current row's captured inline description.</param>
    /// <param name="looksLikeOptionRow">Recognizes candidate option rows.</param>
    /// <param name="captureInlineDescription">
    /// Returns the description group for a recognized declaration, including an empty group
    /// for a declaration without prose, or null when the caller grammar does not match.
    /// </param>
    internal static string AccumulateWrappedDescription(
        IReadOnlyList<string> lines,
        ref int declarationIndex,
        Group? inlineDescription,
        Func<string, bool> looksLikeOptionRow,
        Func<string, Group?>? captureInlineDescription)
    {
        var declaration = lines[declarationIndex];
        var declarationIndentation = GetIndentation(declaration);
        var descriptionColumn = GetCapturedDescriptionColumn(declaration, inlineDescription);
        var allowSameColumnDescription = descriptionColumn is null
                                         && looksLikeOptionRow(declaration);
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
                    looksLikeOptionRow(candidate),
                    declarationIndex + 2 < lines.Count ? lines[declarationIndex + 2] : null,
                    lines[declarationIndex],
                    looksLikeOptionRow,
                    allowSameColumnDescription,
                    captureInlineDescription))
            {
                break;
            }

            var continuation = candidate.Trim();
            // Help formatters can wrap a long option reference at an internal hyphen.
            // Keep ordinary prose hyphens and standalone option terminators unchanged.
            if (parts.Count > 0
                && char.IsAsciiLetterOrDigit(continuation[0])
                && WrappedLongOptionPrefixPattern().IsMatch(parts[^1]))
            {
                parts[^1] += continuation;
            }
            else
            {
                parts.Add(continuation);
            }

            declarationIndex++;

            // A row whose prose only starts on the next line (picocli, argparse, git) reveals its
            // description column there, so later wrapped lines get the same column-aware rule.
            descriptionColumn ??= GetIndentation(candidate);
        }

        return string.Join(' ', parts);
    }

    [GeneratedRegex(@"(?<![\w/-])--[A-Za-z0-9][A-Za-z0-9_-]*-$")]
    private static partial Regex WrappedLongOptionPrefixPattern();

    /// <summary>
    /// Creates a typed option from a clap declaration and its parsed help block.
    /// </summary>
    protected static CliOptionDefinition CreateClapOption(
        Match match,
        string className,
        string propertyName,
        string switchName,
        ClapOptionBlock block)
    {
        var shortForm = match.Groups["short"].Value.Trim();
        var valueHint = match.Groups["value"].Value.Trim();
        var isFlag = string.IsNullOrEmpty(valueHint);
        var acceptsMultipleValues = match.Groups["multi"].Success
                                    || IsRepeatableValueOption(block.Description, isFlag, isBoolean: false);
        var attachedOptionalValue = valueHint.StartsWith("[=", StringComparison.Ordinal);
        var optionalValue = valueHint.StartsWith('[');
        var enumDefinition = isFlag || optionalValue
            ? null
            : TryCreateOptionEnum(className, propertyName, switchName, block.PossibleValues);
        var flagType = acceptsMultipleValues ? "int?" : "bool?";

        return new CliOptionDefinition
        {
            SwitchName = switchName,
            ShortForm = match.Groups["long"].Success && !string.IsNullOrEmpty(shortForm) ? shortForm : null,
            PropertyName = propertyName,
            CSharpType = isFlag
                ? flagType
                : AsCSharpType($"{enumDefinition?.EnumName ?? "string"}?", acceptsMultipleValues),
            Description = GetOptionDescription(block, enumDefinition is not null),
            IsFlag = isFlag,
            ValueArity = optionalValue ? CliOptionValueArity.Optional : CliOptionValueArity.Required,
            IsRequired = false,
            AcceptsMultipleValues = acceptsMultipleValues,
            IsKeyValue = false,
            IsNumeric = isFlag && acceptsMultipleValues,
            ValueSeparator = attachedOptionalValue ? "=" : " ",
            EnumDefinition = enumDefinition,
            IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag, block.Description)
        };
    }

    private static string GetOptionDescription(ClapOptionBlock block, bool hasEnum)
    {
        if (hasEnum || block.PossibleValues.Count == 0)
        {
            return block.Description;
        }

        var choices = string.Join(", ", block.PossibleValues.Select(value =>
            string.IsNullOrWhiteSpace(value.Description) ? value.Value : $"{value.Value}: {value.Description}"));
        return $"{block.Description} [possible values: {choices}]".Trim();
    }

    /// <summary>
    /// Matches an option declaration row: the switches, an optional value hint such as
    /// <c>&lt;CPU&gt;...</c> or <c>[=&lt;COLOR&gt;]</c>, and an inline description when the
    /// layout carries one after two or more spaces.
    /// </summary>
    [GeneratedRegex(@"^\s*(?:(?<short>-\w)(?:,\s*(?<long>--[\w-]+))?|(?<long>--[\w-]+))(?:\s*(?<value><[^>]+>|\[[^\]]+\]))?(?<multi>\.\.\.)?(?:\s{2,}(?<desc>.*))?\s*$", RegexOptions.Multiline)]
    protected static partial Regex ClapOptionDeclarationPattern();

    /// <summary>
    /// Returns the paragraph clap-style help prints above its <c>Usage:</c> line, or
    /// <see langword="null"/> when the help opens with the usage block.
    /// </summary>
    protected static string? ExtractSummaryAboveUsage(IReadOnlyList<string> lines)
    {
        var summary = new List<string>();
        foreach (var line in lines)
        {
            var trimmed = line.Trim();
            if (trimmed.StartsWith("Usage:", StringComparison.OrdinalIgnoreCase))
            {
                return summary.Count > 0 ? string.Join(' ', summary) : null;
            }

            if (trimmed.Length > 0)
            {
                summary.Add(trimmed);
            }
        }

        return null;
    }

    /// <summary>
    /// Reads the block clap prints beneath an option declaration: the description paragraph,
    /// then bracketed trailers such as <c>[possible values: a, b]</c> or <c>[default: x]</c>,
    /// or a <c>Possible values:</c> list whose <c>- value: text</c> entries document each value.
    /// The first line after the declaration fixes the description column and the block runs
    /// while lines stay at or beyond it, blank lines included, so it ends at the next
    /// declaration or heading. Advances <paramref name="index"/> to the last consumed line.
    /// </summary>
    /// <param name="lines">The help text lines.</param>
    /// <param name="index">Index of the declaration row; advanced to the last consumed line.</param>
    /// <param name="switchColumn">Column where the declaration's long switch starts.</param>
    protected static ClapOptionBlock ReadClapOptionBlock(
        IReadOnlyList<string> lines,
        ref int index,
        int switchColumn)
    {
        var prose = new List<string>();
        var possibleValues = new List<ClapPossibleValue>();
        int? descriptionColumn = null;
        var listingValues = false;
        var pendingTrailer = string.Empty;
        var startsParagraph = false;
        var lastProseIndex = -1;
        while (index + 1 < lines.Count)
        {
            var line = lines[index + 1];
            if (string.IsNullOrWhiteSpace(line))
            {
                index++;
                listingValues = false;
                startsParagraph = true;
                continue;
            }

            var indentation = GetIndentation(line);
            // Clap aligns every declaration's long switch, so indentation alone separates
            // declarations from descriptions even when wrapped prose starts with a switch.
            if (indentation <= switchColumn || indentation < descriptionColumn)
            {
                break;
            }

            descriptionColumn ??= indentation;
            index++;
            var text = line.Trim();
            if (TryReadClapTrailer(text, ref pendingTrailer, possibleValues, prose))
            {
                startsParagraph = false;
                continue;
            }

            if (text.Equals("Possible values:", StringComparison.OrdinalIgnoreCase))
            {
                listingValues = true;
                startsParagraph = false;
                continue;
            }

            if (!listingValues)
            {
                AppendClapProse(prose, text, startsParagraph, ref lastProseIndex);
                startsParagraph = false;
                continue;
            }

            var entry = ClapPossibleValueEntryPattern().Match(text);
            if (entry.Success)
            {
                var documentation = entry.Groups["doc"].Value.Trim();
                possibleValues.Add(new ClapPossibleValue(
                    entry.Groups["value"].Value,
                    documentation.Length > 0 ? documentation : null));
            }
            else if (possibleValues.Count > 0)
            {
                // Wrapped documentation of the previous value.
                var previous = possibleValues[^1];
                possibleValues[^1] = previous with { Description = $"{previous.Description} {text}".Trim() };
            }
        }

        if (pendingTrailer.Length > 0)
        {
            prose.Add(pendingTrailer);
        }

        return new ClapOptionBlock(string.Join(' ', prose), possibleValues);
    }

    private static void AppendClapProse(List<string> prose, string text, bool startsParagraph, ref int lastProseIndex)
    {
        // Wrapped lines stay in the same sentence; blank lines separate prose
        // paragraphs even when clap omits punctuation from the first paragraph.
        // Metadata trailers are annotations, so punctuation belongs to the preceding prose.
        if (startsParagraph && lastProseIndex >= 0 && !EndsWithSentencePunctuation(prose[lastProseIndex]))
        {
            prose[lastProseIndex] += ".";
        }

        lastProseIndex = prose.Count;
        prose.Add(text);
    }

    private static bool EndsWithSentencePunctuation(string text)
    {
        var content = text.AsSpan().TrimEnd("\"'`’”)]}»›");
        return !content.IsEmpty && ".!?:;。！？：；…؟۔।॥".Contains(content[^1]);
    }

    private static bool TryReadClapTrailer(
        string text,
        ref string pendingTrailer,
        List<ClapPossibleValue> possibleValues,
        List<string> prose)
    {
        if (pendingTrailer.Length == 0 && !ClapMetadataTrailerStartPattern().IsMatch(text))
        {
            return false;
        }

        // A trailer wrapped at the terminal width continues until its closing bracket.
        pendingTrailer = pendingTrailer.Length > 0 ? $"{pendingTrailer} {text}" : text;
        if (!text.EndsWith(']'))
        {
            return true;
        }

        var trailer = ClapTrailerPattern().Match(pendingTrailer);
        if (trailer.Success && IsPossibleValuesTrailer(trailer.Groups["name"].Value))
        {
            possibleValues.AddRange(ParsePossibleValuesList(trailer.Groups["value"].Value));
        }
        else if (!IsRepeatedClapDefault(trailer, prose))
        {
            prose.Add(pendingTrailer);
        }

        pendingTrailer = string.Empty;
        return true;
    }

    private static bool IsRepeatedClapDefault(Match trailer, IReadOnlyList<string> prose)
    {
        if (!trailer.Success || !trailer.Groups["name"].Value.Equals("default", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var value = trailer.Groups["value"].Value.Trim();
        var description = string.Join(' ', prose);
        foreach (Match annotation in ClapDefaultAnnotationStartPattern().Matches(description))
        {
            var remaining = description.AsSpan(annotation.Index + annotation.Length);
            if (!remaining.StartsWith(value, StringComparison.Ordinal))
            {
                continue;
            }

            // Match the entire literal value, including any nested brackets or parentheses.
            var closing = annotation.Groups["open"].Value[0] == '(' ? ')' : ']';
            var suffix = remaining[value.Length..].TrimStart();
            if (!suffix.IsEmpty && suffix[0] == closing)
            {
                return true;
            }
        }

        return false;
    }

    [GeneratedRegex(@"(?<open>[\[(])default\s*:\s*", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex ClapDefaultAnnotationStartPattern();

    /// <summary>
    /// Splits a trailing <c>[possible values: a, b]</c> from an inline description, where
    /// clap's aligned layout appends it to the description text, and removes repeated default trailers.
    /// </summary>
    protected static ClapOptionBlock SplitPossibleValuesTrailer(string description)
    {
        var match = InlinePossibleValuesPattern().Match(description);
        if (!match.Success)
        {
            return new ClapOptionBlock(RemoveRepeatedInlineClapDefaults(description), []);
        }

        var prose = string.Concat(description[..match.Index], description[(match.Index + match.Length)..]).Trim();
        return new ClapOptionBlock(RemoveRepeatedInlineClapDefaults(prose), [.. ParsePossibleValuesList(match.Groups["value"].Value)]);
    }

    private static string RemoveRepeatedInlineClapDefaults(string description)
    {
        var annotations = ClapDefaultAnnotationStartPattern().Matches(description);
        for (var i = annotations.Count - 1; i >= 0; i--)
        {
            var annotation = annotations[i];
            if (annotation.Groups["open"].Value != "[")
            {
                continue;
            }

            var trailerEnd = FindClapTrailerEnd(description.AsSpan(annotation.Index));
            if (trailerEnd < 0)
            {
                continue;
            }

            var suffix = description[(annotation.Index + trailerEnd + 1)..].TrimStart();
            if (!IsClapMetadataSuffix(suffix))
            {
                continue;
            }

            var trailer = ClapTrailerPattern().Match(description.Substring(annotation.Index, trailerEnd + 1));
            var prose = description[..annotation.Index].TrimEnd();
            if (IsRepeatedClapDefault(trailer, [prose]))
            {
                description = suffix.Length == 0 ? prose : $"{prose} {suffix}";
            }
        }

        return description;
    }

    private static bool IsClapMetadataSuffix(ReadOnlySpan<char> suffix)
    {
        suffix = suffix.TrimStart();
        while (!suffix.IsEmpty)
        {
            if (!ClapMetadataTrailerStartPattern().IsMatch(suffix))
            {
                return false;
            }

            var end = FindClapTrailerEnd(suffix);
            if (end < 0)
            {
                return false;
            }

            suffix = suffix[(end + 1)..].TrimStart();
        }

        return true;
    }

    private static int FindClapTrailerEnd(ReadOnlySpan<char> text)
    {
        var depth = 0;
        for (var i = 0; i < text.Length; i++)
        {
            if (text[i] == '[')
            {
                depth++;
            }
            else if (text[i] == ']' && --depth == 0)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Builds an enum for an option whose help enumerates its values. Returns
    /// <see langword="null"/> unless two to twenty distinct members remain; longer lists read
    /// as prose rather than a closed set.
    /// </summary>
    protected static CliEnumDefinition? TryCreateOptionEnum(
        string className,
        string propertyName,
        string switchName,
        IReadOnlyList<ClapPossibleValue> values) =>
        OptionEnumFactory.TryCreate(className, propertyName, switchName, values.Select(value => (value.Value, value.Description)));

    private static bool IsPossibleValuesTrailer(string trailerName) =>
        trailerName.Equals("possible values", StringComparison.OrdinalIgnoreCase);

    [GeneratedRegex(@"^\[(?:possible values|default|alias(?:es)?|env):", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex ClapMetadataTrailerStartPattern();

    private static IEnumerable<ClapPossibleValue> ParsePossibleValuesList(string list) =>
        list.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(value => new ClapPossibleValue(value, null));

    /// <summary>
    /// An option's description and enumerated values as printed beneath a clap declaration.
    /// </summary>
    protected sealed record ClapOptionBlock(string Description, IReadOnlyList<ClapPossibleValue> PossibleValues);

    /// <summary>
    /// One value from a clap <c>[possible values: ...]</c> trailer or <c>Possible values:</c> list.
    /// </summary>
    protected sealed record ClapPossibleValue(string Value, string? Description);

    /// <summary>
    /// Matches a clap trailer beneath an option description, such as
    /// <c>[possible values: cyclonedx, spdx]</c>, <c>[default: library]</c> or <c>[aliases: x]</c>.
    /// </summary>
    [GeneratedRegex(@"^\[(?<name>[a-z ]+):\s*(?<value>.*)\]$", RegexOptions.IgnoreCase)]
    private static partial Regex ClapTrailerPattern();

    /// <summary>
    /// Matches one entry of a clap <c>Possible values:</c> list: <c>- value: documentation</c>.
    /// </summary>
    [GeneratedRegex(@"^-\s+(?<value>[^\s:]+):?(?:\s+(?<doc>.*))?$")]
    private static partial Regex ClapPossibleValueEntryPattern();

    /// <summary>
    /// Matches a <c>[possible values: ...]</c> trailer embedded in inline description text.
    /// </summary>
    [GeneratedRegex(@"\s*\[possible values:\s*(?<value>[^\]]+)\]", RegexOptions.IgnoreCase)]
    private static partial Regex InlinePossibleValuesPattern();

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
            if (HelpDeclaresExplicitBooleanValue(description)
                && option.IsFlag
                && option.NegatedSwitchName is null)
            {
                throw new InvalidOperationException(
                    $"{command.FullCommand} {option.SwitchName} declares explicit true/false values, "
                    + "but the parsed model marks it as a presence-only flag.");
            }

            if (!option.IsFlag
                && !isBoolean
                && HelpDeclaresRepeatableOption(helpText, option.SwitchName, description)
                && !option.IsStructuredValue
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
        var emittedOperands = command.PositionalArguments
            .Select(argument => argument.PropertyName)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var missingArguments = command.ArgumentGroups
            .SelectMany(group => group.FlattenArguments())
            .Where(argument => argument.IsPositional
                ? !emittedOperands.Contains(NormalizePropertyName(argument.SwitchName) ?? argument.SwitchName)
                : !emittedSwitches.Contains(argument.SwitchName))
            .Select(argument => argument.SwitchName)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (missingArguments.Length != 0)
        {
            throw new InvalidOperationException(
                $"{command.FullCommand} declares grouped arguments that were swallowed or omitted: "
                + string.Join(", ", missingArguments));
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

    // Consume example sentences and quoted spans before looking for declarations.
    // Quoted punctuation does not end an example; possessive apostrophes do not open quotes.
    [GeneratedRegex("""
        \bfor\s+example\b (?: "(?:\\.|[^"\\])*" | (?<!\w)'(?:\\.|[^'\\])*' | `[^`]*` | [^.!?\r\n] )*
        | "(?:\\.|[^"\\])*" | (?<!\w)'(?:\\.|[^'\\])*' | `[^`]*`
        | (?<required>\(required\)(?=\s|[.!?]|$))
        """,
        RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex ExplicitRequiredOptionPattern();

    private const string OperationalCountPhrasePattern =
        @"(?:[\w-]+\s+){0,2}(?:attempts?|times?|retries?)\b";

    private const string RepeatableItemCountPattern =
        @"(?:one|zero)\s+or\s+more\s+(?!" + OperationalCountPhrasePattern + @")[\w-]+";

    private const string RepeatableValueRegex =
        @"\b(?:"
        + @"repeatable"
        + @"|repeat\s+to\s+add\s+more"
        + @"|repeat\s+or\s+comma-separate\s+for\s+multiple"
        + @"|(?:can|may|must|should)\s+be\s+repeated"
        + @"|(?:is|are)\s+repeated"
        + @"|(?:multiples?|multiple\s+[\w-]+)\s+(?:are\s+)?supported\s+by\s+passing\s+"
        + @"(?:--?[\w-]+|(?<exampleQuote>['""`])--?[\w-]+(?:\s+[^'""`\r\n]+)?\k<exampleQuote>)\s+multiple\s+times"
        + @"|\A" + RepeatableItemCountPattern
        // "Specifications of one or more endpoints" describes the option's values; "Expression is
        // a list of one or more restrictions" describes the grammar of one value, so an
        // article-led predicate keeps this alternative from matching.
        + @"|(?<!\b(?:is|are|as|be|being)\s+(?:an?|the)\s+)(?:specifications?|lists?)\s+of\s+" + RepeatableItemCountPattern
        + @"|(?:can|may|must|should)\s+be\s+"
        + @"(?:specified|supplied|provided|used|passed|set|given)\s+"
        + @"(?:(?:one|zero)\s+or\s+more\s+times|multiple\s+times|more\s+than\s+once)"
        + @"|(?:accepts?|specify|supply|provide|use|pass|set|give|supports?|takes?)\s+"
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
