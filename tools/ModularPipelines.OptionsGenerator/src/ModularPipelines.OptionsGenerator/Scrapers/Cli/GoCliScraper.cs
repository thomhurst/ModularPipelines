using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Go tooling CLI.
/// Go uses a custom help format.
///
/// go help format (go help):
/// Go is a tool for managing Go source code.
///
/// Usage:
///         go &lt;command&gt; [arguments]
///
/// The commands are:
///         bug         start a bug report
///         build       compile packages and dependencies
///         clean       remove object files and cached files
///         ...
///
/// Subcommand help (go help build):
/// usage: go build [-o output] [build flags] [packages]
///
/// Build compiles the packages named by the import paths...
///
/// The build flags are:
///         -a          force rebuilding of packages that are already up-to-date
///         -n          print the commands but do not run them
///         ...
/// </summary>
public partial class GoCliScraper : CliScraperBase
{
    private const string FlagProbeValue = "__modularpipelines_probe__";

    private const string ProseOptionLeadInPattern =
        @"(?:The|[A-Z][\w-]* also provides the|When using -[A-Za-z][\w-]*,\s+the)";

    private readonly ConcurrentDictionary<string, IReadOnlySet<string>> _unsupportedSharedBuildFlags =
        new(StringComparer.Ordinal);

    public GoCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<GoCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "go";

    public override string NamespacePrefix => "Go";

    public override string TargetNamespace => "ModularPipelines.Go";

    public override string OutputDirectory => "src/ModularPipelines.Go";

    protected override string VersionArguments => "version";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "help"
    };

    /// <summary>
    /// Gets help text for go commands using "go help &lt;command&gt;" format.
    /// </summary>
    protected override async Task<string?> GetHelpTextAsync(string[] commandPath, CancellationToken cancellationToken)
    {
        var helpText = await GetRawHelpTextAsync(commandPath, cancellationToken);
        if (string.IsNullOrWhiteSpace(helpText)
            || commandPath.Length != 2)
        {
            return helpText;
        }

        var additionalHelp = new List<string>();
        await AddTestFlagsHelpAsync(commandPath, additionalHelp, cancellationToken);
        await AddDocFlagsHelpAsync(commandPath, additionalHelp, cancellationToken);
        await AddSharedBuildFlagsHelpAsync(commandPath, helpText, additionalHelp, cancellationToken);

        return additionalHelp.Count == 0
            ? helpText
            : string.Join("\n\n", additionalHelp.Prepend(helpText.TrimEnd()));
    }

    private async Task AddDocFlagsHelpAsync(
        string[] commandPath,
        List<string> additionalHelp,
        CancellationToken cancellationToken)
    {
        if (commandPath is not [_, "doc"])
        {
            return;
        }

        var directHelp = await GetDirectHelpTextAsync("doc -h", cancellationToken);
        if (FlagLinePattern().IsMatch(directHelp ?? string.Empty))
        {
            additionalHelp.Add(directHelp!);
        }
    }

    private async Task AddTestFlagsHelpAsync(
        string[] commandPath,
        List<string> additionalHelp,
        CancellationToken cancellationToken)
    {
        if (commandPath is not [_, "test"])
        {
            return;
        }

        var testFlagHelp = await GetRawHelpTextAsync([ToolName, "testflag"], cancellationToken);
        if (GetTestFlagsHelp(testFlagHelp) is { } recognizedTestFlags)
        {
            additionalHelp.Add(recognizedTestFlags);
        }
    }

    private async Task AddSharedBuildFlagsHelpAsync(
        string[] commandPath,
        string helpText,
        List<string> additionalHelp,
        CancellationToken cancellationToken)
    {
        if (commandPath is [_, "build"])
        {
            return;
        }

        var buildHelp = await GetRawHelpTextAsync([ToolName, "build"], cancellationToken);
        if (string.IsNullOrWhiteSpace(buildHelp))
        {
            return;
        }

        var sharedBuildFlags = GetSharedBuildFlagsHelp(buildHelp);
        if (sharedBuildFlags is null)
        {
            return;
        }

        var isDocWithoutDirectFlags = IsDocWithoutDirectFlags(commandPath, additionalHelp);
        if (!ShouldIncludeSharedBuildFlags(
                commandPath,
                helpText,
                buildHelp,
                isDocWithoutDirectFlags))
        {
            return;
        }

        var sharedOptions = ParseOptions(["build"], sharedBuildFlags, usageSynopsis: null);
        var candidates = GetSharedBuildFlagCandidates(sharedOptions, isDocWithoutDirectFlags);
        var supportedFlags = await GetSupportedFlagsAsync(commandPath, candidates, cancellationToken);
        var unsupportedFlags = sharedOptions
            .Select(option => option.SwitchName)
            .Except(supportedFlags, StringComparer.Ordinal)
            .ToHashSet(StringComparer.Ordinal);

        _unsupportedSharedBuildFlags[string.Join(" ", commandPath)] = unsupportedFlags;
        additionalHelp.Add(sharedBuildFlags);
    }

    private static bool IsDocWithoutDirectFlags(
        IReadOnlyList<string> commandPath,
        IEnumerable<string> additionalHelp) =>
        commandPath is [_, "doc"]
        && !additionalHelp.Any(text => ContainsOption(text, "-C"));

    private static bool ShouldIncludeSharedBuildFlags(
        IReadOnlyList<string> commandPath,
        string helpText,
        string buildHelp,
        bool isDocWithoutDirectFlags) =>
        UsesSharedBuildFlags(commandPath, buildHelp)
        || BuildFlagsUsagePattern().IsMatch(helpText)
        || isDocWithoutDirectFlags;

    private static CliOptionDefinition[] GetSharedBuildFlagCandidates(
        IEnumerable<CliOptionDefinition> sharedOptions,
        bool isDocWithoutDirectFlags) =>
        isDocWithoutDirectFlags
            ? sharedOptions.Where(option => option.SwitchName == "-C").ToArray()
            : sharedOptions.ToArray();

    private async Task<IReadOnlySet<string>> GetSupportedFlagsAsync(
        IReadOnlyList<string> commandPath,
        IReadOnlyList<CliOptionDefinition> candidates,
        CancellationToken cancellationToken)
    {
        var supportedFlags = new HashSet<string>(StringComparer.Ordinal);
        var command = string.Join(" ", commandPath.Skip(1));

        foreach (var candidate in candidates)
        {
            var arguments = $"{command} {candidate.SwitchName}={FlagProbeValue} -h";
            var result = await Executor.ExecuteAsync(ExecutablePath, arguments, cancellationToken);
            if (!UnknownFlagPattern().IsMatch(result.CombinedOutput))
            {
                supportedFlags.Add(candidate.SwitchName);
            }
        }

        return supportedFlags;
    }

    private static bool ContainsOption(string helpText, string switchName) =>
        GoOptionLinePattern().Matches(helpText)
            .Any(match => match.Groups["flag"].Value == switchName);

    private static string? GetTestFlagsHelp(string? testFlagHelp)
    {
        const string marker = "The following flags are recognized by";
        var markerIndex = testFlagHelp?.IndexOf(marker, StringComparison.OrdinalIgnoreCase) ?? -1;
        return markerIndex < 0 ? null : testFlagHelp![markerIndex..].Trim();
    }

    private static string? GetSharedBuildFlagsHelp(string buildHelp)
    {
        var sharedCommandsMatch = SharedBuildCommandsPattern().Match(buildHelp);
        return sharedCommandsMatch.Success ? buildHelp[sharedCommandsMatch.Index..] : null;
    }

    private async Task<string?> GetRawHelpTextAsync(
        string[] commandPath,
        CancellationToken cancellationToken)
    {
        var cacheKey = string.Join(" ", commandPath);

        if (HelpCache.TryGet(cacheKey, out var cached))
        {
            return cached;
        }

        // Go uses "go help <command>" instead of "go <command> --help"
        var args = commandPath.Length > 1
            ? "help " + string.Join(" ", commandPath.Skip(1))
            : "help";

        var result = await ExecuteAndRecordHelpCommandAsync(
            commandPath,
            ExecutablePath,
            args,
            cancellationToken);

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

    private async Task<string?> GetDirectHelpTextAsync(
        string arguments,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"direct:{arguments}";
        if (HelpCache.TryGet(cacheKey, out var cached))
        {
            return cached;
        }

        var result = await Executor.ExecuteAsync(ExecutablePath, arguments, cancellationToken);
        var helpText = !string.IsNullOrEmpty(result.StandardOutput)
            ? result.StandardOutput
            : result.StandardError;

        if (!string.IsNullOrWhiteSpace(helpText))
        {
            HelpCache.Set(cacheKey, helpText);
            return helpText;
        }

        return null;
    }

    private static bool UsesSharedBuildFlags(
        IReadOnlyList<string> commandPath,
        string buildHelp)
    {
        if (commandPath.Count != 2)
        {
            return false;
        }

        var sharedCommandsMatch = SharedBuildCommandsPattern().Match(buildHelp);
        if (!sharedCommandsMatch.Success)
        {
            return false;
        }

        return CommandNamePattern().Matches(sharedCommandsMatch.Groups["commands"].Value)
            .Select(match => match.Value)
            .Contains(commandPath[1], StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Extracts subcommand names from go help text.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        var seenCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find "The commands are:" section
        var commandsSectionMatch = CommandsSectionPattern().Match(helpText);
        if (commandsSectionMatch.Success)
        {
            var sectionStart = commandsSectionMatch.Index + commandsSectionMatch.Length;
            var sectionEnd = helpText.Length;

            // Find where this section ends (next blank line or section)
            var nextSection = NextSectionPattern().Match(helpText, sectionStart);
            if (nextSection.Success)
            {
                sectionEnd = nextSection.Index;
            }

            var section = helpText.Substring(sectionStart, sectionEnd - sectionStart);
            var lines = section.Split('\n');

            foreach (var line in lines)
            {
                var match = CommandLinePattern().Match(line);
                if (match.Success)
                {
                    var commandName = match.Groups["name"].Value.Trim();
                    if (!string.IsNullOrEmpty(commandName) &&
                        IsValidCommand(commandName) &&
                        seenCommands.Add(commandName))
                    {
                        subcommands.Add(commandName);
                    }
                }
            }
        }

        return subcommands;
    }

    /// <summary>
    /// Checks if a string looks like a valid command name.
    /// </summary>
    private static bool IsValidCommand(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
        {
            return false;
        }

        return name.All(c => char.IsLower(c) || char.IsDigit(c) || c == '-');
    }

    /// <summary>
    /// Go exposes version as an ordinary root command even though the shared traversal
    /// treats that name as a utility node by default.
    /// </summary>
    protected override bool IsSkippableSubcommand(string subcommand) =>
        !subcommand.Equals("version", StringComparison.OrdinalIgnoreCase)
        && base.IsSkippableSubcommand(subcommand);

    /// <summary>
    /// Parses a go command from its help text.
    /// </summary>
    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken) =>
        throw new InvalidOperationException("Shared traversal must pass its parsed synopsis.");

    /// <inheritdoc />
    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        UsageSynopsisParseResult usage,
        CancellationToken cancellationToken)
    {
        var commandParts = commandPath.Skip(1).ToArray();

        if (commandParts.Length == 0)
        {
            return Task.FromResult<CliCommandDefinition?>(null);
        }

        var description = ExtractDescription(helpText);
        var options = ParseOptions(commandParts, helpText, usage.Synopsis);
        if (_unsupportedSharedBuildFlags.TryGetValue(string.Join(" ", commandPath), out var unsupportedFlags))
        {
            options.RemoveAll(option => unsupportedFlags.Contains(option.SwitchName));
        }

        var enums = options
            .Where(o => o.EnumDefinition is not null)
            .Select(o => o.EnumDefinition!)
            .ToList();
        var positionalArguments = AddOrderedEditOperations(
            commandParts,
            NormalizePositionalArguments(
                commandParts,
                GetPositionalArguments(usage, options)));

        var className = GenerateClassName(commandPath);

        var command = new CliCommandDefinition
        {
            FullCommand = string.Join(" ", commandPath),
            CommandParts = commandParts,
            ClassName = className,
            ParentClassName = BaseOptionsClassName,
            ToolNamespacePrefix = NamespacePrefix,
            Description = description,
            DocumentationUrl = null,
            Options = options,
            PositionalArguments = positionalArguments,
            UsageSynopsis = usage.Synopsis,
            HasOperandTakingUsage = usage.HasOperandTokens,
            SubDomainGroup = null,
            Enums = enums
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    private static IReadOnlyList<CliPositionalArgument> NormalizePositionalArguments(
        IReadOnlyList<string> commandParts,
        IReadOnlyList<CliPositionalArgument> arguments) =>
        arguments.Select(argument => NormalizePositionalArgument(commandParts, argument) with
        {
            Phase = CommandLinePhase.Passthrough,
        })
            .ToArray();

    private static IReadOnlyList<CliPositionalArgument> AddOrderedEditOperations(
        IReadOnlyList<string> commandParts,
        IReadOnlyList<CliPositionalArgument> arguments)
    {
        if (commandParts is not (["mod", "edit"] or ["work", "edit"]))
        {
            return arguments;
        }

        return
        [
            new CliPositionalArgument
            {
                PropertyName = "OrderedEdits",
                CSharpType = "IEnumerable<GoEditOperation>?",
                Description = "Editing operations rendered in the order supplied. Use this sequence when order across different edit switches matters.",
                Phase = CommandLinePhase.Normal,
                PositionIndex = 0,
                IsVariadic = true,
            },
            .. arguments,
        ];
    }

    private static CliPositionalArgument NormalizePositionalArgument(
        IReadOnlyList<string> commandParts,
        CliPositionalArgument argument)
    {
        if (commandParts is ["telemetry"] && argument.PropertyName == "Off")
        {
            return argument with
            {
                PropertyName = "Mode",
                Description = "The telemetry mode: off, local, or on.",
            };
        }

        if (commandParts is ["generate"] && argument.PropertyName == "FileGo")
        {
            argument = argument with
            {
                PropertyName = "Targets",
                Description = "The file or package targets.",
            };
        }

        if (commandParts is ["doc"])
        {
            return argument with
            {
                CSharpType = "IEnumerable<string>?",
                IsRequired = false,
                IsVariadic = true,
            };
        }

        if (argument.PropertyName is not (
            "Arguments" or "CliArguments" or "Packages" or "Modules" or "Moddirs" or "Targets"))
        {
            return argument;
        }

        return argument with
        {
            CSharpType = argument.IsRequired ? "IEnumerable<string>" : "IEnumerable<string>?",
            IsVariadic = true,
        };
    }

    /// <summary>
    /// Extracts description from help text.
    /// </summary>
    private static string? ExtractDescription(string helpText)
    {
        var lines = helpText.Split('\n');

        // Skip usage line and look for first descriptive paragraph
        var foundUsage = false;
        foreach (var line in lines)
        {
            var trimmed = line.Trim();

            if (trimmed.StartsWith("usage:", StringComparison.OrdinalIgnoreCase) ||
                trimmed.StartsWith("Usage:", StringComparison.OrdinalIgnoreCase))
            {
                foundUsage = true;
                continue;
            }

            if (!foundUsage)
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(trimmed))
            {
                continue;
            }

            // Skip flag lines
            if (trimmed.StartsWith('-'))
            {
                continue;
            }

            // Skip section headers
            if (trimmed.EndsWith(':'))
            {
                continue;
            }

            if (trimmed.Length > 10)
            {
                return trimmed;
            }
        }

        return null;
    }

    /// <summary>
    /// Parses options from go help text.
    /// Format: -flag    description
    ///         -flag value    description
    /// </summary>
    private static List<CliOptionDefinition> ParseOptions(
        IReadOnlyList<string> commandParts,
        string helpText,
        string? usageSynopsis)
    {
        var options = new List<CliOptionDefinition>();
        var normalizedHelpText = helpText.ReplaceLineEndings("\n");
        var paragraphs = ParagraphSeparatorPattern().Split(normalizedHelpText);
        var repeatableOptions = GetRepeatableOptions(paragraphs);
        AddUsageOptions(usageSynopsis ?? string.Empty, options, repeatableOptions);
        AddDocumentedOptions(normalizedHelpText.Split('\n'), options, repeatableOptions);
        AddProseOptions(paragraphs, options, repeatableOptions);
        ApplyCommandSpecificOptionShapes(commandParts, options);
        DisambiguatePropertyNames(options);
        return options;
    }

    private static void DisambiguatePropertyNames(List<CliOptionDefinition> options)
    {
        var usedPropertyNames = options
            .GroupBy(option => option.PropertyName, StringComparer.Ordinal)
            .Where(group => group.Count() == 1)
            .Select(group => group.Key)
            .ToHashSet(StringComparer.Ordinal);

        foreach (var group in options
                     .GroupBy(option => option.PropertyName, StringComparer.Ordinal)
                     .Where(group => group.Count() > 1)
            .OrderBy(group => group.Key, StringComparer.Ordinal))
        {
            foreach (var option in group.OrderBy(option => option.SwitchName, StringComparer.Ordinal))
            {
                var encodedSwitchName = EncodeSwitchName(option.SwitchName);
                var propertyName = encodedSwitchName;
                var suffix = 2;
                while (!usedPropertyNames.Add(propertyName))
                {
                    propertyName = $"{encodedSwitchName}{suffix++}";
                }

                var optionIndex = options.FindIndex(item =>
                    item.SwitchName.Equals(option.SwitchName, StringComparison.Ordinal));
                options[optionIndex] = option with
                {
                    PropertyName = propertyName,
                    IsSecret = GeneratorUtils.IsSecretOption(propertyName, option.IsFlag),
                };
            }
        }
    }

    private static string EncodeSwitchName(string switchName) =>
        string.Concat(switchName.TrimStart('-').Select(character => character switch
        {
            >= 'A' and <= 'Z' => $"Upper{character}",
            >= 'a' and <= 'z' => $"Lower{char.ToUpperInvariant(character)}",
            >= '0' and <= '9' => $"Digit{character}",
            '-' => "Hyphen",
            '_' => "Underscore",
            _ => $"Character{(int) character:X4}",
        }));

    private static void ApplyCommandSpecificOptionShapes(
        IReadOnlyList<string> commandParts,
        List<CliOptionDefinition> options)
    {
        if (commandParts is ["mod", "edit"])
        {
            ApplyValueOptionShape(options, "-module", "=");
            ApplyValueOptionShape(options, "-C", " ");
        }

        if (commandParts is ["tool"])
        {
            ApplyValueOptionShape(options, "-C", " ");
            ApplyValueOptionShape(options, "-overlay", " ");
        }

        if (commandParts is ["test"])
        {
            var argsIndex = options.FindIndex(option => option.SwitchName == "-args");
            if (argsIndex >= 0)
            {
                options[argsIndex] = options[argsIndex] with
                {
                    CSharpType = "IEnumerable<string>?",
                    IsFlag = false,
                    GroupValues = true,
                    ValueSeparator = " ",
                    Phase = CommandLinePhase.Terminal,
                };
            }
        }

        if (commandParts is ["get"])
        {
            ApplyOptionalValueOptionShape(options, "-u", "=");
        }

        if (commandParts is ["list"])
        {
            ApplyOptionalValueOptionShape(options, "-json", "=");
        }

        ApplyOptionPhase(options, "-C", CommandLinePhase.EarlyOperand);
    }

    private static void ApplyValueOptionShape(
        List<CliOptionDefinition> options,
        string switchName,
        string valueSeparator)
    {
        var optionIndex = options.FindIndex(option => option.SwitchName == switchName);
        if (optionIndex < 0)
        {
            return;
        }

        var option = options[optionIndex];
        options[optionIndex] = option with
        {
            CSharpType = AsCSharpType("string?", option.AcceptsMultipleValues),
            IsFlag = false,
            ValueSeparator = valueSeparator,
            IsSecret = GeneratorUtils.IsSecretOption(option.PropertyName, isFlag: false),
        };
    }

    private static void ApplyOptionalValueOptionShape(
        List<CliOptionDefinition> options,
        string switchName,
        string valueSeparator)
    {
        var optionIndex = options.FindIndex(option => option.SwitchName == switchName);
        if (optionIndex < 0)
        {
            return;
        }

        var option = options[optionIndex];
        options[optionIndex] = option with
        {
            CSharpType = "string?",
            IsFlag = false,
            ValueArity = CliOptionValueArity.Optional,
            ValueSeparator = valueSeparator,
            IsSecret = GeneratorUtils.IsSecretOption(option.PropertyName, isFlag: false),
        };
    }

    private static void ApplyOptionPhase(
        List<CliOptionDefinition> options,
        string switchName,
        CommandLinePhase phase)
    {
        var optionIndex = options.FindIndex(option => option.SwitchName == switchName);
        if (optionIndex >= 0)
        {
            options[optionIndex] = options[optionIndex] with { Phase = phase };
        }
    }

    private static IReadOnlySet<string> GetRepeatableOptions(IEnumerable<string> paragraphs)
    {
        var repeatableOptions = new HashSet<string>(StringComparer.Ordinal);
        foreach (var paragraph in paragraphs)
        {
            if (!DescriptionDeclaresRepeatableOption(paragraph))
            {
                continue;
            }

            foreach (Match match in GoOptionReferencePattern().Matches(paragraph))
            {
                repeatableOptions.Add(match.Groups["flag"].Value);
            }
        }

        return repeatableOptions;
    }

    private static void AddDocumentedOptions(
        string[] lines,
        List<CliOptionDefinition> options,
        IReadOnlySet<string> repeatableOptions)
    {
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var match = GoOptionLinePattern().Match(line);
            if (!match.Success)
            {
                continue;
            }

            var flagName = match.Groups["flag"].Value.Trim();
            var (valueHint, description) = ParseOptionLineRemainder(match.Groups["remainder"].Value);

            if (string.IsNullOrEmpty(flagName))
            {
                continue;
            }

            var optionKey = flagName.TrimStart('-');

            // Accumulate multi-line descriptions
            i = AccumulateMultiLineDescription(lines, i, ref description);

            var propertyName = NormalizePropertyName(optionKey);
            if (propertyName is null)
            {
                continue;
            }

            var acceptsMultipleValues = repeatableOptions.Contains(optionKey);
            var valueSeparator = GetValueSeparator(optionKey, description);
            var isFlag = string.IsNullOrEmpty(valueHint) && valueSeparator is null;

            AddOrMergeOption(options, new CliOptionDefinition
            {
                SwitchName = flagName,
                ShortForm = null,
                PropertyName = propertyName,
                CSharpType = AsCSharpType(isFlag ? "bool?" : "string?", acceptsMultipleValues),
                Description = description,
                IsFlag = isFlag,
                IsRequired = false,
                AcceptsMultipleValues = acceptsMultipleValues,
                IsKeyValue = false,
                IsNumeric = false,
                ValueSeparator = valueHint.Contains('=')
                    ? "="
                    : valueSeparator ?? " ",
                EnumDefinition = null,
                IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag)
            }, preferCandidateDescription: true);
        }
    }

    private static (string ValueHint, string Description) ParseOptionLineRemainder(string remainder)
    {
        if (string.IsNullOrWhiteSpace(remainder))
        {
            return (string.Empty, string.Empty);
        }

        if (LeadingDescriptionPattern().IsMatch(remainder))
        {
            return (string.Empty, remainder.Trim());
        }

        var inlineDescription = InlineOptionDescriptionPattern().Match(remainder);
        if (inlineDescription.Success)
        {
            return (
                inlineDescription.Groups["value"].Value.Trim(),
                inlineDescription.Groups["description"].Value.Trim());
        }

        return (remainder.Trim(), string.Empty);
    }

    private static void AddProseOptions(
        IEnumerable<string> paragraphs,
        List<CliOptionDefinition> options,
        IReadOnlySet<string> repeatableOptions)
    {
        foreach (var paragraph in paragraphs)
        {
            var normalizedParagraph = paragraph.Trim();
            foreach (Match sentenceMatch in ProseOptionSentencePattern().Matches(normalizedParagraph))
            {
                var sentence = sentenceMatch.Groups["sentence"].Value.Trim();
                var declarationMatch = ProseOptionParagraphPattern().Match(sentence);
                if (!declarationMatch.Success)
                {
                    continue;
                }

                var optionMatches = GoOptionReferencePattern().Matches(
                        declarationMatch.Groups["declarations"].Value)
                    .Cast<Match>()
                    .ToArray();
                foreach (var match in optionMatches)
                {
                    var optionKey = match.Groups["flag"].Value;

                    var propertyName = NormalizePropertyName(optionKey);
                    if (propertyName is null)
                    {
                        continue;
                    }

                    var acceptsMultipleValues = repeatableOptions.Contains(optionKey);
                    var detectedSeparator = GetValueSeparator(optionKey, normalizedParagraph);
                    var isFlag = !match.Groups["value"].Success && detectedSeparator is null;
                    AddOrMergeOption(options, new CliOptionDefinition
                    {
                        SwitchName = $"-{optionKey}",
                        PropertyName = propertyName,
                        CSharpType = AsCSharpType(isFlag ? "bool?" : "string?", acceptsMultipleValues),
                        Description = sentence.ReplaceLineEndings(" "),
                        IsFlag = isFlag,
                        AcceptsMultipleValues = acceptsMultipleValues,
                        ValueSeparator = match.Groups["separator"].Value == "="
                            ? "="
                            : detectedSeparator ?? " ",
                        IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag),
                    }, preferCandidateDescription: optionMatches.Length == 1);
                }
            }
        }
    }

    private static void AddUsageOptions(
        string helpText,
        List<CliOptionDefinition> options,
        IReadOnlySet<string> repeatableOptions)
    {
        foreach (Match match in GoUsageOptionPattern().Matches(helpText))
        {
            var flagName = match.Groups["flag"].Value;
            var optionKey = flagName.TrimStart('-');
            var propertyName = NormalizePropertyName(optionKey);
            if (propertyName is null)
            {
                continue;
            }

            var valueHint = match.Groups["value"].Value;
            var isFlag = string.IsNullOrEmpty(valueHint);
            var acceptsMultipleValues = repeatableOptions.Contains(optionKey);
            var valueSeparator = match.Groups["separator"].Value == "=" ? "=" : " ";
            AddOrMergeOption(options, new CliOptionDefinition
            {
                SwitchName = flagName,
                PropertyName = propertyName,
                CSharpType = AsCSharpType(isFlag ? "bool?" : "string?", acceptsMultipleValues),
                Description = $"The {flagName} option.",
                IsFlag = isFlag,
                AcceptsMultipleValues = acceptsMultipleValues,
                ValueSeparator = valueSeparator,
                IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag),
            });
        }
    }

    private static string? GetValueSeparator(string optionKey, string description)
    {
        var optionPattern = Regex.Escape(optionKey);
        if (Regex.IsMatch(description, $@"(?<![\w-])-{optionPattern}=\S"))
        {
            return "=";
        }

        return Regex.IsMatch(
            description,
            $@"(?<![\w-])-{optionPattern}\s+(?!(?:flag|flags|option|options)\b)\S+\s+(?:flag|option)\b")
            || description.Contains($"-{optionKey} flag's value", StringComparison.Ordinal)
                ? " "
                : null;
    }

    private static void AddOrMergeOption(
        List<CliOptionDefinition> options,
        CliOptionDefinition candidate,
        bool preferCandidateDescription = false)
    {
        var existingIndex = options.FindIndex(option =>
            option.SwitchName.Equals(candidate.SwitchName, StringComparison.Ordinal));

        if (existingIndex < 0)
        {
            options.Add(candidate);
            return;
        }

        var existing = options[existingIndex];
        var isFlag = existing.IsFlag && candidate.IsFlag;
        var acceptsMultipleValues = existing.AcceptsMultipleValues || candidate.AcceptsMultipleValues;
        options[existingIndex] = existing with
        {
            CSharpType = AsCSharpType(isFlag ? "bool?" : "string?", acceptsMultipleValues),
            Description = GetPreferredDescription(existing, candidate, preferCandidateDescription),
            IsFlag = isFlag,
            AcceptsMultipleValues = acceptsMultipleValues,
            ValueSeparator = existing.IsFlag && !candidate.IsFlag
                ? candidate.ValueSeparator
                : existing.ValueSeparator,
            IsSecret = GeneratorUtils.IsSecretOption(existing.PropertyName, isFlag),
        };
    }

    private static string? GetPreferredDescription(
        CliOptionDefinition existing,
        CliOptionDefinition candidate,
        bool preferCandidateDescription)
    {
        if (string.IsNullOrWhiteSpace(candidate.Description))
        {
            return existing.Description;
        }

        return preferCandidateDescription
               || string.IsNullOrWhiteSpace(existing.Description)
               || existing.Description == $"The {existing.SwitchName} option."
            ? candidate.Description
            : existing.Description;
    }

    /// <summary>
    /// Accumulates multi-line descriptions.
    /// </summary>
    private static int AccumulateMultiLineDescription(string[] lines, int currentIndex, ref string description)
    {
        var descriptionParts = new List<string>();
        var declarationIndentation = GetIndentationWidth(lines[currentIndex]);
        if (!string.IsNullOrEmpty(description))
        {
            descriptionParts.Add(description);
        }

        var nextIndex = currentIndex + 1;
        while (nextIndex < lines.Length)
        {
            var nextLine = lines[nextIndex];
            var trimmedNext = nextLine.Trim();

            if (string.IsNullOrWhiteSpace(trimmedNext))
            {
                break;
            }

            if (trimmedNext.EndsWith(':') && trimmedNext.Length < 30)
            {
                break;
            }

            var continuationIndentation = GetIndentationWidth(nextLine);
            if (continuationIndentation <= declarationIndentation)
            {
                break;
            }

            descriptionParts.Add(trimmedNext);
            nextIndex++;
        }

        description = string.Join(" ", descriptionParts);
        return nextIndex - 1;
    }

    private static int GetIndentationWidth(string line)
    {
        var width = 0;
        foreach (var character in line)
        {
            if (character == ' ')
            {
                width++;
            }
            else if (character == '\t')
            {
                width += 8 - (width % 8);
            }
            else
            {
                break;
            }
        }

        return width;
    }

    /// <summary>
    /// Checks if help text indicates the command has options.
    /// </summary>
    protected override bool HasOptions(string helpText)
    {
        return GoCommandUsagePattern().IsMatch(helpText) ||
               helpText.Contains("flags are:") ||
               helpText.Contains("Flags:") ||
               FlagLinePattern().IsMatch(helpText);
    }

    #region Regex Patterns

    /// <summary>
    /// Matches "The commands are:" section.
    /// </summary>
    [GeneratedRegex(@"The commands are:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex CommandsSectionPattern();

    /// <summary>
    /// Matches command lines: "	build       compile packages..."
    /// Go uses tabs for indentation, so we match either a tab or 4+ spaces.
    /// </summary>
    [GeneratedRegex(@"^(?:\t|\s{4,})(?<name>[\w-]+)\s{2,}", RegexOptions.Multiline)]
    private static partial Regex CommandLinePattern();

    /// <summary>
    /// Matches next section or blank line.
    /// </summary>
    [GeneratedRegex(@"\n\n|\n[A-Z]")]
    private static partial Regex NextSectionPattern();

    /// <summary>
    /// Matches go-style option lines:
    /// -a          force rebuilding...
    /// -n          print the commands...
    /// -o file     write output to file
    /// </summary>
    [GeneratedRegex(@"^[ \t]+(?<flag>-[A-Za-z][\w-]*)(?<remainder>[^\r\n]*)$", RegexOptions.Multiline)]
    private static partial Regex GoOptionLinePattern();

    [GeneratedRegex("""^\s(?<value>(?:'[^']*'|"[^"]*"|\S+))(?:\s{2,}(?<description>.*))?\s*$""")]
    private static partial Regex InlineOptionDescriptionPattern();

    [GeneratedRegex(@"^(?:\t|\s{2,})\S")]
    private static partial Regex LeadingDescriptionPattern();

    [GeneratedRegex(@"(?<![\w-])-(?<flag>[A-Za-z][\w-]*)(?:(?<separator>=)(?<value>[^\s,]+))?")]
    private static partial Regex GoOptionReferencePattern();

    [GeneratedRegex("^" + ProseOptionLeadInPattern + @"\s+(?<declarations>-.+?)\s+(?:editing flags|build flags|flag's|flags|flag|options|option)\b", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private static partial Regex ProseOptionParagraphPattern();

    [GeneratedRegex(@"(?:^|(?<=[.!?])\s+)(?<sentence>" + ProseOptionLeadInPattern + @"\s+-.+?(?=(?:[.!?]\s+" + ProseOptionLeadInPattern + @"\s+-)|$))", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private static partial Regex ProseOptionSentencePattern();

    [GeneratedRegex(@"(?:\r?\n){2,}")]
    private static partial Regex ParagraphSeparatorPattern();

    [GeneratedRegex(@"The build flags are shared by the (?<commands>.+?) commands:", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private static partial Regex SharedBuildCommandsPattern();

    [GeneratedRegex(@"\[build flags\]", RegexOptions.IgnoreCase)]
    private static partial Regex BuildFlagsUsagePattern();

    [GeneratedRegex(@"flag provided but not defined|unknown flag|unrecognized option", RegexOptions.IgnoreCase)]
    private static partial Regex UnknownFlagPattern();

    [GeneratedRegex(@"[a-z][a-z0-9-]*", RegexOptions.IgnoreCase)]
    private static partial Regex CommandNamePattern();

    [GeneratedRegex(@"^\s*usage:\s+go\s+", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex GoCommandUsagePattern();

    /// <summary>
    /// Matches options declared directly in a Go usage synopsis, including commands whose
    /// help does not expose a dedicated flags table.
    /// </summary>
    [GeneratedRegex(@"(?<![\w-])(?<flag>-[a-z][\w-]*)(?:(?<separator>=|[ \t]+)(?<value>(?!(?:flag|flags|option|options)\b)[A-Za-z][\w-]*))?", RegexOptions.IgnoreCase)]
    private static partial Regex GoUsageOptionPattern();

    /// <summary>
    /// Matches flag lines.
    /// </summary>
    [GeneratedRegex(@"^\s+-\w", RegexOptions.Multiline)]
    private static partial Regex FlagLinePattern();

    #endregion
}
