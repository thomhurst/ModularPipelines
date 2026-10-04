using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for pip (Python package manager) CLI.
/// pip uses argparse-style help format.
///
/// pip help format (pip --help):
/// Usage:
///   pip &lt;command&gt; [options]
///
/// Commands:
///   install                     Install packages.
///   download                    Download packages.
///   uninstall                   Uninstall packages.
///   freeze                      Output installed packages in requirements format.
///   inspect                     Inspect the python environment.
///   list                        List installed packages.
///   show                        Show information about installed packages.
///   ...
///
/// Subcommand help (pip install --help):
/// Usage:
///   pip install [options] &lt;requirement specifier&gt; [package-index-options] ...
///
/// Description:
///   Install packages from:
///   - PyPI and other indexes
///   ...
///
/// Install Options:
///   -r, --requirement &lt;file&gt;    Install from the given requirements file.
///   -c, --constraint &lt;file&gt;     Constrain versions using the given constraints file.
///   ...
/// </summary>
public partial class PipCliScraper : CliScraperBase
{
    // These optparse append actions (and --group's appending callback) omit repeatability from help.
    private static readonly HashSet<string> RepeatableOptions = new(StringComparer.Ordinal)
    {
        "--trusted-host", "--exists-action", "--use-feature", "--use-deprecated", "--group",
    };

    public PipCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<PipCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "pip";

    public override string NamespacePrefix => "Pip";

    public override string TargetNamespace => "ModularPipelines.Python";

    public override string OutputDirectory => "src/ModularPipelines.Python";

    /// <inheritdoc />
    protected override IReadOnlyList<CliOptionDefinition> ParseGlobalOptions(string helpText) =>
        ParseOptions(helpText, globalsOnly: true);

    /// <inheritdoc />
    protected override IEnumerable<string> GetAdditionalUsageSynopses(string[] commandPath, string helpText)
    {
        if (commandPath is not ["pip", "install" or "download" or "wheel" or "lock"])
        {
            return [];
        }

        // pip's requirement command accepts editable projects and dependency groups as
        // input sources even when the usage summary omits their standalone forms.
        return ParseOptions(helpText)
            .Where(static option => !option.IsFlag && option.SwitchName is "--editable" or "--group")
            .Select(option => $"{string.Join(" ", commandPath)} [options] {option.SwitchName} <{option.PropertyName}>");
    }

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "debug"
    };

    /// <summary>
    /// Extracts subcommand names from pip help text.
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
    /// Parses a pip command from its help text.
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

        usage = NormalizeParentGroupUsage(commandParts, usage);
        var description = ExtractDescription(helpText);
        var options = ParseOptions(helpText);
        var enums = options
            .Where(o => o.EnumDefinition is not null)
            .Select(o => o.EnumDefinition!)
            .ToList();
        var positionalArguments = GetPositionalArguments(usage, options);

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

    private static UsageSynopsisParseResult NormalizeParentGroupUsage(
        IReadOnlyList<string> commandParts,
        UsageSynopsisParseResult usage)
    {
        var normalized = commandParts is ["cache" or "config" or "index"]
            ? usage with
            {
                HasOperandTokens = false,
                PositionalArguments = [],
                UnparsedOperandTokens = [],
            }
            : usage;
        return normalized with
        {
            PositionalArguments = NormalizePackageIndexArguments(normalized.PositionalArguments),
            RequirednessCandidates = [.. normalized.RequirednessCandidates.Select(candidate => candidate with
            {
                PositionalArguments = NormalizePackageIndexArguments(candidate.PositionalArguments),
            })],
        };
    }

    private static IReadOnlyList<CliPositionalArgument> NormalizePackageIndexArguments(
        IReadOnlyList<CliPositionalArgument> arguments)
    {
        var hasPackageIndexLabel = arguments.Any(argument =>
            argument.PropertyName.Equals("PackageIndexOptions", StringComparison.Ordinal));
        if (!hasPackageIndexLabel)
        {
            return arguments;
        }

        return arguments
                .Where(argument => !argument.PropertyName.Equals(
                    "PackageIndexOptions",
                    StringComparison.Ordinal))
                .Select(argument => argument.PropertyName.Equals(
                    "RequirementSpecifier",
                    StringComparison.Ordinal)
                    ? argument with
                    {
                        CSharpType = argument.IsRequired
                            ? "IEnumerable<string>"
                            : "IEnumerable<string>?",
                        IsVariadic = true,
                    }
                    : argument)
                .ToArray();
    }

    /// <inheritdoc />
    protected override UsageSynopsisParseResult NormalizeUsageSynopsis(
        CliCommandDefinition command,
        UsageSynopsisParseResult usage) =>
        NormalizeParentGroupUsage(command.CommandParts, usage);

    /// <summary>
    /// Extracts description from help text.
    /// </summary>
    private static string? ExtractDescription(string helpText)
    {
        // Look for "Description:" section
        var descMatch = DescriptionSectionPattern().Match(helpText);
        if (descMatch.Success)
        {
            var sectionStart = descMatch.Index + descMatch.Length;
            var sectionEnd = helpText.Length;

            var nextSection = NextSectionPattern().Match(helpText, sectionStart);
            if (nextSection.Success)
            {
                sectionEnd = nextSection.Index;
            }

            var section = helpText.Substring(sectionStart, sectionEnd - sectionStart);
            var lines = section.Split('\n');

            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                if (!string.IsNullOrEmpty(trimmed) && trimmed.Length > 10)
                {
                    return trimmed;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Parses options from pip help text.
    /// Format: -r, --requirement &lt;file&gt;    Install from the given requirements file.
    ///         --no-deps                    Don't install package dependencies.
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(string helpText, bool globalsOnly = false)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find all Options sections (e.g., "Install Options:", "General Options:")
        var optionsSectionMatches = OptionsSectionPattern().Matches(helpText);
        foreach (Match optionsMatch in optionsSectionMatches)
        {
            var isGeneral = optionsMatch.Groups["section"].Value.Equals("General Options", StringComparison.OrdinalIgnoreCase);
            if ((globalsOnly && !isGeneral) || (!globalsOnly && isGeneral && GlobalOptions.Count > 0))
            {
                continue;
            }

            var sectionStart = optionsMatch.Index + optionsMatch.Length;
            var sectionEnd = helpText.Length;

            var nextSection = NextSectionPattern().Match(helpText, sectionStart);
            if (nextSection.Success)
            {
                sectionEnd = nextSection.Index;
            }

            var section = helpText.Substring(sectionStart, sectionEnd - sectionStart);
            var lines = section.Split('\n');

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var match = PipOptionPattern().Match(line);
                if (!match.Success)
                {
                    continue;
                }

                var shortForm = match.Groups["short"].Value.Trim();
                var longForm = match.Groups["long"].Value.Trim();
                var valueHint = match.Groups["value"].Value.Trim();

                if (!seenOptions.Add(longForm))
                {
                    continue;
                }

                var description = AccumulateWrappedDescription(lines, ref i, match.Groups["desc"], IsOptionRow);

                var propertyName = NormalizePropertyName(longForm);
                if (propertyName is null)
                {
                    continue;
                }

                var isFlag = string.IsNullOrEmpty(valueHint);
                var acceptsMultipleValues = IsRepeatableValueOption(description, isFlag)
                    || RepeatableOptions.Contains(longForm);
                var scalarType = isFlag ? "bool?" : "string?";
                var csharpType = AsCSharpType(scalarType, acceptsMultipleValues);

                options.Add(new CliOptionDefinition
                {
                    SwitchName = longForm,
                    ShortForm = string.IsNullOrEmpty(shortForm) ? null : shortForm,
                    PropertyName = propertyName,
                    CSharpType = csharpType,
                    Description = description,
                    IsFlag = isFlag,
                    IsRequired = false,
                    AcceptsMultipleValues = acceptsMultipleValues,
                    IsKeyValue = false,
                    IsNumeric = false,
                    ValueSeparator = " ",
                    EnumDefinition = null,
                    IsSecret = longForm == "--proxy" || GeneratorUtils.IsSecretOption(propertyName, isFlag)
                });
            }
        }

        return options;
    }

    private static bool IsOptionRow(string line) => PipOptionPattern().IsMatch(line);

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
    [GeneratedRegex(@"Commands:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex CommandsSectionPattern();

    /// <summary>
    /// Matches command lines: "  install                     Install packages."
    /// </summary>
    [GeneratedRegex(@"^\s{2,}(?<name>[\w-]+)\s{2,}", RegexOptions.Multiline)]
    private static partial Regex CommandLinePattern();

    /// <summary>
    /// Matches "Description:" section.
    /// </summary>
    [GeneratedRegex(@"Description:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex DescriptionSectionPattern();

    /// <summary>
    /// Matches Options sections like "Install Options:", "General Options:", etc.
    /// </summary>
    [GeneratedRegex(@"^(?<section>(?:\w+[ \t]+)*Options):[ \t]*\r?\n", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex OptionsSectionPattern();

    /// <summary>
    /// Matches next section headers.
    /// </summary>
    [GeneratedRegex(@"\n[A-Z][\w\s]*:\s*\n")]
    private static partial Regex NextSectionPattern();

    /// <summary>
    /// Matches pip-style option lines:
    /// -r, --requirement &lt;file&gt;    Install from the given requirements file.
    /// --no-deps                    Don't install package dependencies.
    /// </summary>
    [GeneratedRegex(@"^[ \t]*(?:(?<short>-\w),[ \t]*)?(?<long>--[\w-]+)(?:[ \t]+(?<value><[^>]+>|\[[^\]]+\]))?(?:[ \t]{2,}(?<desc>.*))?[ \t]*\r?$", RegexOptions.Multiline)]
    private static partial Regex PipOptionPattern();

    #endregion
}
