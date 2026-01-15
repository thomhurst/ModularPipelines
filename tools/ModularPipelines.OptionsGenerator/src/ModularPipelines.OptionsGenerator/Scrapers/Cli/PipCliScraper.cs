using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
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
    public PipCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<PipCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "pip";

    public override string NamespacePrefix => "Pip";

    public override string TargetNamespace => "ModularPipelines.Python";

    public override string OutputDirectory => "src/ModularPipelines.Python";

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
        CancellationToken cancellationToken)
    {
        var commandParts = commandPath.Skip(1).ToArray();

        if (commandParts.Length == 0)
        {
            return Task.FromResult<CliCommandDefinition?>(null);
        }

        var description = ExtractDescription(helpText);
        var options = ParseOptions(helpText, commandParts);
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
            DocumentationUrl = null,
            Options = options,
            PositionalArguments = [],
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
    private List<CliOptionDefinition> ParseOptions(string helpText, string[] commandParts)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find all Options sections (e.g., "Install Options:", "General Options:")
        var optionsSectionMatches = OptionsSectionPattern().Matches(helpText);
        foreach (Match optionsMatch in optionsSectionMatches)
        {
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
                var description = match.Groups["desc"].Value.Trim();

                if (string.IsNullOrEmpty(longForm))
                {
                    if (!string.IsNullOrEmpty(shortForm))
                    {
                        longForm = shortForm;
                        shortForm = string.Empty;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (seenOptions.Contains(longForm))
                {
                    continue;
                }

                seenOptions.Add(longForm);

                // Accumulate multi-line descriptions
                i = AccumulateMultiLineDescription(lines, i, ref description);

                var propertyName = NormalizePropertyName(longForm);
                if (propertyName is null)
                {
                    continue;
                }

                var isFlag = string.IsNullOrEmpty(valueHint) && IsBooleanOption(longForm, description);
                var csharpType = isFlag ? "bool?" : "string?";

                options.Add(new CliOptionDefinition
                {
                    SwitchName = longForm,
                    ShortForm = string.IsNullOrEmpty(shortForm) ? null : shortForm,
                    PropertyName = propertyName,
                    CSharpType = csharpType,
                    Description = description,
                    IsFlag = isFlag,
                    IsRequired = false,
                    AcceptsMultipleValues = false,
                    IsKeyValue = false,
                    IsNumeric = false,
                    ValueSeparator = isFlag ? " " : " ",
                    EnumDefinition = null
                });
            }
        }

        return options;
    }

    /// <summary>
    /// Determines if an option is a boolean flag.
    /// </summary>
    private static bool IsBooleanOption(string optionName, string description)
    {
        var cleanName = optionName.TrimStart('-').ToLowerInvariant();

        // Options that typically take values
        var valueOptions = new[] { "requirement", "constraint", "index-url", "extra-index-url", "target", "src", "root", "prefix" };
        if (valueOptions.Any(v => cleanName.Contains(v)))
        {
            return false;
        }

        // Common boolean option patterns
        if (cleanName.StartsWith("no-") ||
            cleanName == "quiet" ||
            cleanName == "verbose" ||
            cleanName == "upgrade" ||
            cleanName == "force-reinstall" ||
            cleanName == "ignore-installed" ||
            cleanName == "pre" ||
            cleanName == "user" ||
            cleanName == "editable")
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Accumulates multi-line descriptions.
    /// </summary>
    private static int AccumulateMultiLineDescription(string[] lines, int currentIndex, ref string description)
    {
        var descriptionParts = new List<string>();
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

            if (trimmedNext.StartsWith('-'))
            {
                break;
            }

            if (trimmedNext.EndsWith(':') && trimmedNext.Length < 30)
            {
                break;
            }

            var leadingSpaces = nextLine.Length - nextLine.TrimStart().Length;
            if (leadingSpaces < 20)
            {
                break;
            }

            descriptionParts.Add(trimmedNext);
            nextIndex++;
        }

        description = string.Join(" ", descriptionParts);
        return nextIndex - 1;
    }

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
    [GeneratedRegex(@"(?:\w+\s+)?Options:\s*\n", RegexOptions.IgnoreCase)]
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
    [GeneratedRegex(@"^\s*(?:(?<short>-\w),\s*)?(?<long>--[\w-]+)(?:\s+(?<value><[^>]+>|\[[^\]]+\]))?\s{2,}(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex PipOptionPattern();

    #endregion
}
