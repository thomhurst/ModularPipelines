using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Newman - Postman collection runner.
/// Newman uses commander.js-style help format.
///
/// newman help format (newman --help):
/// Usage: newman [options] [command]
///
/// Newman is a command-line collection runner for Postman.
///
/// Options:
///   -v, --version              output the version number
///   -h, --help                 display help for command
///
/// Commands:
///   run <collection> [options]  Run a collection
///   ...
/// </summary>
public partial class NewmanCliScraper : CliScraperBase
{
    public NewmanCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<NewmanCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "newman";

    public override string NamespacePrefix => "Newman";

    public override string TargetNamespace => "ModularPipelines.Newman";

    public override string OutputDirectory => "src/ModularPipelines.Newman";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "version"
    };

    /// <summary>
    /// Extracts subcommand names from newman help text.
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
            var section = helpText.Substring(sectionStart);
            var lines = section.Split('\n');

            foreach (var line in lines)
            {
                var match = CommandLinePattern().Match(line);
                if (match.Success)
                {
                    var commandName = match.Groups["name"].Value.Trim();
                    if (!string.IsNullOrEmpty(commandName) &&
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
    /// Parses a newman command from its help text.
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
        var options = ParseOptions(helpText);

        var className = GenerateClassName(commandPath);

        var command = new CliCommandDefinition
        {
            FullCommand = string.Join(" ", commandPath),
            CommandParts = commandParts,
            ClassName = className,
            ParentClassName = BaseOptionsClassName,
            ToolNamespacePrefix = NamespacePrefix,
            Description = description,
            DocumentationUrl = "https://www.npmjs.com/package/newman",
            Options = options,
            PositionalArguments = commandParts.Contains("run")
                ?
                [
                    new CliPositionalArgument
                    {
                        PropertyName = "Collection",
                        PlaceholderName = "collection",
                        CSharpType = "string?",
                        IsRequired = true,
                        PositionIndex = 0,
                        Description = "Postman collection file or URL"
                    }
                ]
                : [],
            SubDomainGroup = null,
            Enums = []
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    /// <summary>
    /// Extracts description from help text.
    /// </summary>
    private static string? ExtractDescription(string helpText)
    {
        var lines = helpText.Split('\n');

        foreach (var line in lines)
        {
            var trimmed = line.Trim();

            if (string.IsNullOrWhiteSpace(trimmed))
            {
                continue;
            }

            if (trimmed.StartsWith("Usage:", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (trimmed.EndsWith(':'))
            {
                continue;
            }

            if (trimmed.Length > 10 && !trimmed.Contains("--") && !trimmed.StartsWith('-'))
            {
                return trimmed;
            }
        }

        return null;
    }

    /// <summary>
    /// Parses options from newman help text.
    /// Format: -e, --environment <path>     Specify a URL or path to a Postman Environment
    ///         -n, --iteration-count <n>    Define the number of iterations
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(string helpText)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find "Options:" section
        var optionsSectionMatch = OptionsSectionPattern().Match(helpText);
        string section;
        if (optionsSectionMatch.Success)
        {
            var sectionStart = optionsSectionMatch.Index + optionsSectionMatch.Length;
            section = helpText.Substring(sectionStart);
        }
        else
        {
            section = helpText;
        }

        var lines = section.Split('\n');

        foreach (var line in lines)
        {
            var match = NewmanOptionPattern().Match(line);
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
                continue;
            }

            if (seenOptions.Contains(longForm))
            {
                continue;
            }

            seenOptions.Add(longForm);

            var propertyName = NormalizePropertyName(longForm);
            if (propertyName is null)
            {
                continue;
            }

            var isFlag = string.IsNullOrEmpty(valueHint);
            var csharpType = isFlag ? "bool?" : "string?";

            // Handle common array-type options
            if (longForm is "--global-var" or "--env-var" or "--folder" or "--reporter")
            {
                csharpType = "IEnumerable<string>?";
            }

            options.Add(new CliOptionDefinition
            {
                SwitchName = longForm,
                ShortForm = string.IsNullOrEmpty(shortForm) ? null : shortForm,
                PropertyName = propertyName,
                CSharpType = csharpType,
                Description = description,
                IsFlag = isFlag,
                IsRequired = false,
                AcceptsMultipleValues = csharpType.Contains("IEnumerable"),
                IsKeyValue = false,
                IsNumeric = valueHint == "n" || valueHint == "ms",
                ValueSeparator = " ",
                EnumDefinition = null
            });
        }

        return options;
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
    /// Matches "Options:" section.
    /// </summary>
    [GeneratedRegex(@"Options:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex OptionsSectionPattern();

    /// <summary>
    /// Matches command lines: "  run <collection> [options]  Run a collection"
    /// </summary>
    [GeneratedRegex(@"^\s{2,}(?<name>[\w-]+)\s", RegexOptions.Multiline)]
    private static partial Regex CommandLinePattern();

    /// <summary>
    /// Matches newman-style option lines:
    ///   -e, --environment <path>     Specify a URL or path
    ///   -n, --iteration-count <n>    Define the number
    ///   --bail                       Stop on first failure
    /// </summary>
    [GeneratedRegex(@"^\s+(?:(?<short>-\w),\s+)?(?<long>--[\w-]+)(?:\s+<(?<value>[^>]+)>)?\s+(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex NewmanOptionPattern();

    #endregion
}
