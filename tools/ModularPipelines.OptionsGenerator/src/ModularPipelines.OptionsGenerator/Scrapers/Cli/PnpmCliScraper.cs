using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for the pnpm package manager. pnpm 12 prints clap-style help:
///
/// Add a package
///
/// Usage: pnpm add [OPTIONS] &lt;PACKAGE_NAMES&gt;...
///
/// Options:
///   -D, --save-dev
///           Install the specified packages as devDependencies
///
///       --sbom-format &lt;FORMAT&gt;
///           The SBOM output format (required)
///
///           [possible values: cyclonedx, spdx]
///
/// The root help lists commands under a Commands: section. Some versions repeat
/// parent catalogs for nested commands; pnpm 12.4.1 instead exposes audit and stage
/// subcommands through their variadic PARAMS operand.
/// </summary>
public partial class PnpmCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<PnpmCliScraper> logger) : CliScraperBase(executor, helpCache, logger)
{
    private static readonly string[] ExcludedChildCommands =
    [
        "pnpm audit signatures", "pnpm stage approve", "pnpm stage download",
        "pnpm stage list", "pnpm stage publish", "pnpm stage reject", "pnpm stage view",
    ];

    public override string ToolName => "pnpm";

    public override string NamespacePrefix => "Pnpm";

    public override string TargetNamespace => "ModularPipelines.Node";

    public override string OutputDirectory => "src/ModularPipelines.Node";

    public override CliToolDefinition CreateToolDefinition() =>
        base.CreateToolDefinition() with
        {
            CommandCoverage = new CliCommandCoveragePolicy
            {
                SentinelCommands = ["pnpm audit", "pnpm stage"],
                Exclusions =
                [
                    .. ExcludedChildCommands.Select(command => new CliCommandCoverageExclusion
                    {
                        Command = command,
                        Reason = "pnpm 12.4.1 no longer lists this path in a Commands section or exposes a distinct child synopsis. "
                                 + "The parent command documents [PARAMS]... instead; use its generated Params operand.",
                    }),
                ],
            },
        };

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "-v", "help", "root", "bin", "env"
    };

    /// <summary>
    /// Extracts subcommand names from the Commands section of pnpm help text.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        var seenCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Commands are listed in the "Commands:" section
        var commandsSectionMatch = CommandsSectionPattern().Match(helpText);
        if (commandsSectionMatch.Success)
        {
            var sectionStart = commandsSectionMatch.Index + commandsSectionMatch.Length;
            var sectionEnd = helpText.Length;

            var nextSection = NextSectionPattern().Match(helpText, sectionStart);
            if (nextSection.Success)
            {
                sectionEnd = nextSection.Index;
            }

            var section = helpText[sectionStart..sectionEnd];
            var lines = section.Split('\n');

            foreach (var line in lines)
            {
                var match = SubcommandLinePattern().Match(line);
                if (match.Success)
                {
                    AddSubcommand(match.Groups["name"].Value);
                }
            }
        }

        return subcommands;

        void AddSubcommand(string candidate)
        {
            var commandName = candidate.Trim();
            if (!string.IsNullOrEmpty(commandName)
                && IsValidCommand(commandName)
                && seenCommands.Add(commandName))
            {
                subcommands.Add(commandName);
            }
        }
    }

    /// <inheritdoc />
    protected override IEnumerable<string> ExtractSubcommands(
        string[] commandPath,
        string helpText)
    {
        var subcommands = ExtractSubcommands(helpText).ToList();

        // pnpm returns the parent command-group help for leaf commands. For example,
        // `pnpm stage download --help` repeats the stage subcommand catalog. Once the
        // requested leaf appears in that catalog, rediscovering it would recursively
        // combine every sibling command with every other sibling command.
        var repeatsParentCatalog = commandPath.Length > 1
                                   && subcommands.Contains(commandPath[^1], StringComparer.OrdinalIgnoreCase);

        return repeatsParentCatalog ? [] : subcommands;
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

        // Commands should be lowercase and contain only letters, digits, and hyphens
        return name.All(c => char.IsLower(c) || char.IsDigit(c) || c == '-');
    }

    /// <summary>
    /// Parses a pnpm command from its help text.
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
        var commandParts = commandPath.Skip(1).ToArray(); // Skip tool name

        if (commandParts.Length == 0)
        {
            return Task.FromResult<CliCommandDefinition?>(null);
        }

        usage = NormalizeParentGroupUsage(commandParts, usage);

        // Parse description from help text
        var description = ExtractDescription(helpText);

        var className = GenerateClassName(commandPath);

        // Parse options from the help text; clap lists required ones in the usage line
        var options = ApplyUsageRequiredOptions(ParseOptions(helpText, className), usage);

        // Extract enums from options
        var enums = options
            .Where(o => o.EnumDefinition is not null)
            .Select(o => o.EnumDefinition!)
            .ToList();
        var positionalArguments = GetPositionalArguments(usage);

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

    private static UsageSynopsisParseResult NormalizeParentGroupUsage(
        IReadOnlyList<string> commandParts,
        UsageSynopsisParseResult usage)
    {
        // Older help includes child synopsis lines such as "pnpm stage publish ...".
        // Only those literal child paths are group syntax; the parent's own operands remain.
        if (commandParts is ["stage"] or ["audit"]
            && ChildCommandSynopsisPattern().IsMatch(usage.Synopsis ?? ""))
        {
            usage = usage with
            {
                HasOperandTokens = false,
                PositionalArguments = [],
                UnparsedOperandTokens = [],
            };
        }

        return usage with
        {
            PositionalArguments = [.. usage.PositionalArguments.Select(argument => argument with { Phase = CommandLinePhase.Passthrough })],
        };
    }

    [GeneratedRegex(@"^pnpm\s+(?:stage|audit)\s+[a-z][a-z-]*(?:\s|$)", RegexOptions.IgnoreCase)]
    private static partial Regex ChildCommandSynopsisPattern();

    /// <inheritdoc />
    protected override UsageSynopsisParseResult NormalizeUsageSynopsis(
        CliCommandDefinition command,
        UsageSynopsisParseResult usage) =>
        NormalizeParentGroupUsage(command.CommandParts, usage);

    /// <summary>
    /// Extracts the summary pnpm prints above the usage line.
    /// </summary>
    private static string? ExtractDescription(string helpText) =>
        ExtractSummaryAboveUsage(helpText.Split('\n'));

    /// <summary>
    /// Parses the Options section. A row either carries its description inline after two or
    /// more spaces, or ends at the declaration and is described on the lines beneath it,
    /// followed by [possible values] and other trailers, as clap prints for pnpm 12.
    /// </summary>
    private static List<CliOptionDefinition> ParseOptions(string helpText, string className)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find "Options:" section
        var optionsMatch = OptionsSectionPattern().Match(helpText);
        if (!optionsMatch.Success)
        {
            return options;
        }

        var sectionStart = optionsMatch.Index + optionsMatch.Length;
        var sectionEnd = helpText.Length;

        // Find where this section ends
        var nextSection = NextSectionPattern().Match(helpText, sectionStart);
        if (nextSection.Success)
        {
            sectionEnd = nextSection.Index;
        }

        var section = helpText[sectionStart..sectionEnd];
        var lines = section.Split('\n');

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var match = PnpmOptionPattern().Match(line);
            if (!match.Success)
            {
                continue;
            }

            var longSwitch = match.Groups["long"];
            var primarySwitch = longSwitch.Success ? longSwitch : match.Groups["short"];
            var switchName = primarySwitch.Value.Trim();
            // Clap reserves the "-x, " prefix even when a declaration has only a short switch.
            var switchColumn = GetColumn(line, primarySwitch.Index) + (longSwitch.Success ? 0 : 4);

            // Consume the row's block before deciding whether to keep the row, so the prose of
            // a skipped or duplicate option is never re-read as declarations.
            var block = string.IsNullOrWhiteSpace(match.Groups["desc"].Value)
                ? ReadClapOptionBlock(lines, ref i, switchColumn)
                : SplitPossibleValuesTrailer(
                    AccumulateWrappedDescription(lines, ref i, match.Groups["desc"], IsOptionRow));

            var propertyName = NormalizePropertyName(switchName);
            if (switchName is "--help" or "-h"
                || propertyName is null
                || !seenOptions.Add(switchName))
            {
                continue;
            }

            options.Add(CreateOption(match, className, propertyName, switchName, block));
        }

        return options;
    }

    private static CliOptionDefinition CreateOption(
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

        return new CliOptionDefinition
        {
            SwitchName = switchName,
            ShortForm = match.Groups["long"].Success && !string.IsNullOrEmpty(shortForm) ? shortForm : null,
            PropertyName = propertyName,
            CSharpType = isFlag
                ? "bool?"
                : AsCSharpType($"{enumDefinition?.EnumName ?? "string"}?", acceptsMultipleValues),
            Description = GetOptionDescription(block, enumDefinition is not null),
            IsFlag = isFlag,
            ValueArity = optionalValue ? CliOptionValueArity.Optional : CliOptionValueArity.Required,
            IsRequired = false,
            AcceptsMultipleValues = acceptsMultipleValues,
            IsKeyValue = false,
            IsNumeric = false,
            ValueSeparator = attachedOptionalValue ? "=" : " ",
            EnumDefinition = enumDefinition,
            IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag)
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

    private static bool IsOptionRow(string line) => PnpmOptionPattern().IsMatch(line);

    /// <summary>
    /// Checks if help text indicates the command has options.
    /// </summary>
    protected override bool HasOptions(string helpText)
    {
        return helpText.Contains("Options:") ||
               helpText.Contains("--");
    }

    #region Regex Patterns

    /// <summary>
    /// Matches "Commands:" section header.
    /// </summary>
    [GeneratedRegex(@"Commands?:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex CommandsSectionPattern();

    /// <summary>
    /// Matches "Options:" section.
    /// </summary>
    [GeneratedRegex(@"Options:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex OptionsSectionPattern();

    /// <summary>
    /// Matches next section headers.
    /// </summary>
    [GeneratedRegex(@"\n[A-Z][\w\s]*:\s*\n")]
    private static partial Regex NextSectionPattern();

    /// <summary>
    /// Matches subcommand lines.
    /// </summary>
    [GeneratedRegex(@"^\s{2,}(?<name>[\w-]+)\s{2,}", RegexOptions.Multiline)]
    private static partial Regex SubcommandLinePattern();

    /// <summary>
    /// Matches an option declaration row: the switches, an optional value hint such as
    /// <c>&lt;CPU&gt;...</c> or <c>[=&lt;COLOR&gt;]</c>, and an inline description when the
    /// layout carries one after two or more spaces.
    /// </summary>
    [GeneratedRegex(@"^\s*(?:(?<short>-\w)(?:,\s*(?<long>--[\w-]+))?|(?<long>--[\w-]+))(?:\s*(?<value><[^>]+>|\[[^\]]+\]))?(?<multi>\.\.\.)?(?:\s{2,}(?<desc>.*))?\s*$", RegexOptions.Multiline)]
    private static partial Regex PnpmOptionPattern();

    #endregion
}
