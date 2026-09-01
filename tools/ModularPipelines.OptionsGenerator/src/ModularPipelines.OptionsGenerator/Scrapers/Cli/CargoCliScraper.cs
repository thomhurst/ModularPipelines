using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Generators;
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
        var options = ParseOptions(helpText);
        var enums = options
            .Where(o => o.EnumDefinition is not null)
            .Select(o => o.EnumDefinition!)
            .ToList();

        var className = GenerateClassName(commandPath);

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
            PositionalArguments = usage.PositionalArguments,
            UsageSynopsis = usage.Synopsis,
            HasOperandTakingUsage = usage.HasOperandTokens,
            SubDomainGroup = null,
            Enums = enums
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    /// <summary>
    /// Extracts description from help text.
    /// </summary>
    private static string? ExtractDescription(string helpText)
    {
        var lines = helpText.Split('\n');

        // First non-empty line before "Usage:" is usually the description
        foreach (var line in lines)
        {
            var trimmed = line.Trim();

            if (string.IsNullOrWhiteSpace(trimmed))
            {
                continue;
            }

            if (trimmed.StartsWith("Usage:", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            if (trimmed.Length > 10 && !trimmed.Contains("--"))
            {
                return trimmed;
            }
        }

        return null;
    }

    /// <summary>
    /// Parses options from cargo help text.
    /// Format: -V, --version    Print version info
    ///         --list           List installed commands
    /// </summary>
    private static List<CliOptionDefinition> ParseOptions(string helpText)
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

            var match = CargoOptionDeclarationPattern().Match(lines[index]);
            if (!match.Success)
            {
                continue;
            }

            var shortForm = match.Groups["short"].Value.Trim();
            var longForm = match.Groups["long"].Value.Trim();
            var valueHint = match.Groups["value"].Value.Trim();

            if (string.IsNullOrEmpty(longForm) && string.IsNullOrEmpty(shortForm))
            {
                continue;
            }

            var switchName = !string.IsNullOrEmpty(longForm) ? longForm : shortForm;

            if (!seenOptions.Add(switchName))
            {
                continue;
            }

            var propertyName = NormalizePropertyName(switchName);
            if (propertyName is null)
            {
                continue;
            }

            var isFlag = string.IsNullOrEmpty(valueHint);
            var csharpType = isFlag ? "bool?" : "string?";

            // Handle common array-type options
            if (switchName is "--package" or "-p" or "--exclude" or "--features" or "-F")
            {
                csharpType = "IEnumerable<string>?";
            }

            options.Add(new CliOptionDefinition
            {
                SwitchName = switchName,
                ShortForm = string.IsNullOrEmpty(shortForm) ? null : shortForm,
                PropertyName = propertyName,
                CSharpType = csharpType,
                Description = GetOptionDescription(lines, index, match.Groups["desc"].Value),
                IsFlag = isFlag,
                IsRequired = false,
                AcceptsMultipleValues = csharpType.Contains("IEnumerable"),
                IsKeyValue = false,
                IsNumeric = false,
                ValueSeparator = " ",
                EnumDefinition = null,
                IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag)
            });
        }

        return options;
    }

    private static bool TryGetSectionHeading(string line, out string heading)
    {
        heading = line.Trim();
        return line.Length == line.TrimStart().Length
               && heading.EndsWith(':');
    }

    private static bool IsOptionSectionHeading(string heading) =>
        heading.Contains("option", StringComparison.OrdinalIgnoreCase)
        || heading.EndsWith("selection:", StringComparison.OrdinalIgnoreCase);

    private static string GetOptionDescription(
        string[] lines,
        int declarationIndex,
        string inlineDescription)
    {
        var declarationIndentation = lines[declarationIndex].TakeWhile(char.IsWhiteSpace).Count();
        var description = new List<string>();
        if (!string.IsNullOrWhiteSpace(inlineDescription))
        {
            description.Add(inlineDescription.Trim());
        }

        for (var index = declarationIndex + 1; index < lines.Length; index++)
        {
            var line = lines[index];
            var trimmed = line.Trim();
            if (string.IsNullOrEmpty(trimmed)
                || CargoOptionDeclarationPattern().IsMatch(line)
                || line.TakeWhile(char.IsWhiteSpace).Count() <= declarationIndentation)
            {
                break;
            }

            description.Add(trimmed);
        }

        return string.Join(' ', description);
    }

    /// <summary>
    /// Checks if help text indicates the command has options.
    /// </summary>
    protected override bool HasOptions(string helpText)
    {
        return helpText.Contains("Options:") || helpText.Contains("--");
    }

    #region Regex Patterns

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

    /// <summary>
    /// Matches cargo-style option lines:
    ///   -V, --version             Print version info
    ///       --list                List installed commands
    ///   -p, --package <SPEC>      Package to build
    /// </summary>
    [GeneratedRegex(@"^\s+(?:(?<short>-\w),\s+)?(?<long>--[\w-]+)(?:\s+<(?<value>[^>]+)>)?(?:\s{2,}(?<desc>.*))?\s*$")]
    private static partial Regex CargoOptionDeclarationPattern();

    #endregion
}
