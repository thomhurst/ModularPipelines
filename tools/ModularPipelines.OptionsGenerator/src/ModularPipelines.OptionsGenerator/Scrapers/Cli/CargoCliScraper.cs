using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Cargo - Rust's package manager and build tool.
/// Cargo uses a custom help format similar to clap-derive.
///
/// cargo help format (cargo --help):
/// Rust's package manager
///
/// Usage: cargo [OPTIONS] [COMMAND]
///
/// Commands:
///   build, b    Compile the current package
///   check, c    Analyze the current package and report errors
///   clean       Remove the target directory
///   ...
///
/// Options:
///   -V, --version             Print version info
///   --list                    List installed commands
///   ...
/// </summary>
public partial class CargoCliScraper : CliScraperBase
{
    public CargoCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<CargoCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "cargo";

    public override string NamespacePrefix => "Cargo";

    public override string TargetNamespace => "ModularPipelines.Rust";

    public override string OutputDirectory => "src/ModularPipelines.Rust";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "help", "version", "--version", "-V"
    };

    /// <summary>
    /// Extracts subcommand names from cargo help text.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        var seenCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find "Commands:" section
        var commandsSectionMatch = CommandsSectionPattern().Match(helpText);
        if (commandsSectionMatch.Success)
        {
            var sectionStart = commandsSectionMatch.Index + commandsSectionMatch.Length;
            var sectionEnd = helpText.Length;

            // Find where this section ends
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
    /// Parses a cargo command from its help text.
    /// </summary>
    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken) =>
        throw new InvalidOperationException("Shared traversal must pass its parsed synopsis.");

    protected override async Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        UsageSynopsisParseResult usage,
        CancellationToken cancellationToken)
    {
        var commandParts = commandPath.Skip(1).ToArray();

        if (commandParts.Length == 0)
        {
            return null;
        }

        var className = GenerateClassName(commandPath);
        var description = ExtractSummaryAboveUsage(helpText.Split('\n'));
        var options = ParseOptions(helpText, className);
        var manual = options.Any(static option => !option.IsFlag && !option.AcceptsMultipleValues)
            ? await GetManualHelpTextAsync(commandPath, cancellationToken).ConfigureAwait(false)
            : string.Empty;
        ApplyManualCollectionMetadata(options, manual);

        var enums = options
            .Where(o => o.EnumDefinition is not null)
            .Select(o => o.EnumDefinition!)
            .ToList();

        var command = new CliCommandDefinition
        {
            FullCommand = string.Join(" ", commandPath),
            CommandParts = commandParts,
            ClassName = className,
            ParentClassName = BaseOptionsClassName,
            ToolNamespacePrefix = NamespacePrefix,
            Description = description,
            DocumentationUrl = "https://doc.rust-lang.org/cargo/commands/",
            Options = options,
            PositionalArguments = GetPositionalArguments(usage, options),
            UsageSynopsis = usage.Synopsis,
            HasOperandTakingUsage = usage.HasOperandTokens,
            SubDomainGroup = null,
            Enums = enums
        };

        return command;
    }

    [GeneratedRegex(@"(?:^[ \t]*|,[ \t]*)--?[\w-]+[ =]+(?:<[^>\r\n]+>|\[[^\]\r\n]+\]|[^\s,]+?)(?:\.{3}|…)(?=\s|$)")]
    private static partial Regex ManualRepeatedValuePattern();

    private static void ApplyManualCollectionMetadata(List<CliOptionDefinition> options, string manual)
    {
        for (var index = 0; index < options.Count; index++)
        {
            var option = options[index];
            if (option.IsFlag || option.AcceptsMultipleValues)
            {
                continue;
            }

            // Cargo's manual uses NAME... where terse help omits the repetition marker.
            // -Z is also repeatable, despite neither help form declaring it:
            // https://doc.rust-lang.org/nightly/cargo/reference/unstable.html
            var repeated = option.SwitchName == "-Z"
                           || HelpDeclaresRepeatableOption(manual, option.SwitchName, option.Description ?? "")
                           || HelpOptionBlockMatches(manual, option.SwitchName,
                               block => ManualRepeatedValuePattern().IsMatch(block.Split('\n')[0]));
            var commaSeparated = !repeated && HelpOptionBlockMatches(manual, option.SwitchName, CommaSeparatedListPattern());
            if (repeated || commaSeparated)
            {
                options[index] = option with
                {
                    AcceptsMultipleValues = true,
                    CSharpType = AsCSharpType(option.CSharpType, acceptsMultipleValues: true),
                    CollectionSeparator = commaSeparated ? "," : null,
                };
            }
        }
    }

    /// <summary>
    /// Reads the installed Cargo manual, which documents repetition omitted by terse clap help.
    /// </summary>
    protected virtual async Task<string> GetManualHelpTextAsync(string[] commandPath, CancellationToken cancellationToken)
    {
        var arguments = "help " + string.Join(" ", commandPath.Skip(1));
        var result = await ExecuteAndRecordHelpCommandAsync(commandPath, ExecutablePath, arguments,
            cancellationToken, preserveRawHelp: true, helpKind: CliHelpKind.Manual).ConfigureAwait(false);
        if (!result.Success || string.IsNullOrWhiteSpace(result.StandardOutput))
        {
            throw new InvalidOperationException($"Unable to read installed Cargo manual for {string.Join(" ", commandPath)}.");
        }

        return result.StandardOutput;
    }

    /// <summary>
    /// Parses clap options under Cargo's arbitrary option-section headings.
    /// </summary>
    private static List<CliOptionDefinition> ParseOptions(string helpText, string className)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.Ordinal);
        var lines = helpText.Split('\n');
        var parsingOptionSection = false;

        for (var index = 0; index < lines.Length; index++)
        {
            if (TryGetSectionHeading(lines[index], out var sectionHeading))
            {
                parsingOptionSection = IsOptionSectionHeading(sectionHeading);
                continue;
            }

            if (!parsingOptionSection)
            {
                continue;
            }

            var option = ParseOption(lines, ref index, className, seenOptions);
            if (option is not null)
            {
                options.Add(option);
            }
        }

        return options;
    }

    private static CliOptionDefinition? ParseOption(
        string[] lines,
        ref int index,
        string className,
        HashSet<string> seenOptions)
    {
        var line = lines[index];
        var match = ClapOptionDeclarationPattern().Match(line);
        if (!match.Success)
        {
            return null;
        }

        var longSwitch = match.Groups["long"];
        var primarySwitch = longSwitch.Success ? longSwitch : match.Groups["short"];
        var switchName = primarySwitch.Value.Trim();
        var switchColumn = GetColumn(line, primarySwitch.Index) + (longSwitch.Success ? 0 : 4);
        var block = string.IsNullOrWhiteSpace(match.Groups["desc"].Value)
            ? ReadClapOptionBlock(lines, ref index, switchColumn)
            : SplitPossibleValuesTrailer(
                AccumulateWrappedDescription(lines, ref index, match.Groups["desc"], IsOptionRow));
        var propertyName = NormalizePropertyName(switchName);
        if (propertyName is null || !seenOptions.Add(switchName))
        {
            return null;
        }

        var value = match.Groups["value"];
        if (value.Value.StartsWith("[<", StringComparison.Ordinal))
        {
            // Cargo uses these brackets for a missing-value diagnostic, not a valid bare switch.
            var declaration = line[..value.Index] + value.Value[1..^1] + line[(value.Index + value.Length)..];
            match = ClapOptionDeclarationPattern().Match(declaration);
        }

        return CreateClapOption(match, className, propertyName, switchName, block);
    }

    private static bool TryGetSectionHeading(string line, out string heading)
    {
        heading = line.Trim();
        return line.Length == line.TrimStart().Length
               && heading.EndsWith(':');
    }

    private static readonly string[] NonOptionSectionHeadings =
    [
        "Arguments",
        "Commands",
        "Subcommands",
        "Usage",
        "Examples",
        "Environment",
        "Note",
        "Aliases",
        "Compatibility",
        "See also",
    ];

    /// <summary>
    /// clap groups options under arbitrary headings (<c>Options:</c>, <c>Manifest Options:</c>,
    /// <c>Package Selection:</c>, but also <c>Source:</c> and <c>Section:</c> on <c>cargo add</c>),
    /// so every heading is an option section except the positional, command and prose ones.
    /// </summary>
    private static bool IsOptionSectionHeading(string heading) =>
        !NonOptionSectionHeadings.Any(prefix => heading.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        && (!heading.Contains("command", StringComparison.OrdinalIgnoreCase)
            || heading.EndsWith("Options:", StringComparison.OrdinalIgnoreCase));

    private static bool IsOptionRow(string line) => ClapOptionDeclarationPattern().IsMatch(line);

    /// <summary>
    /// Checks if help text indicates the command has options.
    /// </summary>
    protected override bool HasOptions(string helpText)
    {
        return helpText.Contains("Options:") || helpText.Contains("--");
    }

    #region Regex Patterns

    [GeneratedRegex(@"\bcomma[ -]separated\s+list\b", RegexOptions.IgnoreCase)]
    private static partial Regex CommaSeparatedListPattern();

    /// <summary>
    /// Matches "Commands:" section.
    /// </summary>
    [GeneratedRegex(@"Commands:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex CommandsSectionPattern();

    /// <summary>
    /// Matches command lines: "  build, b    Compile..."
    /// </summary>
    [GeneratedRegex(@"^\s{2,}(?<name>[\w-]+)(?:,\s*\w)?\s{2,}", RegexOptions.Multiline)]
    private static partial Regex CommandLinePattern();

    /// <summary>
    /// Matches next section.
    /// </summary>
    [GeneratedRegex(@"\n[A-Z][\w\s]+:\s*\n")]
    private static partial Regex NextSectionPattern();

    #endregion
}
