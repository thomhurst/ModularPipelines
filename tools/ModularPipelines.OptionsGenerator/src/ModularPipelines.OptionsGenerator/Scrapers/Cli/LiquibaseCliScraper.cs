using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Liquibase - database change management.
/// Liquibase uses a custom help format.
///
/// liquibase help format (liquibase --help):
/// Liquibase 'community' version by Liquibase
///
/// Usage: liquibase [OPTIONS] [COMMAND]
///
/// Commands:
///   update                         Deploy any changes in the changelog file
///   rollback                       Rollback changes
///   status                         Show undeployed changes
///   ...
///
/// Global Options:
///   --changeLogFile=PARAM          The root changelog file
///   --url=PARAM                    The database URL
///   ...
/// </summary>
public partial class LiquibaseCliScraper : CliScraperBase
{
    public LiquibaseCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<LiquibaseCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "liquibase";

    public override string NamespacePrefix => "Liquibase";

    public override string TargetNamespace => "ModularPipelines.Liquibase";

    public override string OutputDirectory => "src/ModularPipelines.Liquibase";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "version"
    };

    /// <summary>
    /// Extracts subcommand names from liquibase help text.
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
    /// Parses a liquibase command from its help text.
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
            DocumentationUrl = "https://docs.liquibase.com/commands/home.html",
            Options = options,
            PositionalArguments = [],
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
    /// Parses options from liquibase help text.
    /// Format: --changeLogFile=PARAM          The root changelog file
    ///         --url=PARAM                    The database URL
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(string helpText)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var lines = helpText.Split('\n');

        foreach (var line in lines)
        {
            var match = LiquibaseOptionPattern().Match(line);
            if (!match.Success)
            {
                continue;
            }

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

            options.Add(new CliOptionDefinition
            {
                SwitchName = longForm,
                ShortForm = null,
                PropertyName = propertyName,
                CSharpType = csharpType,
                Description = description,
                IsFlag = isFlag,
                IsRequired = false,
                AcceptsMultipleValues = false,
                IsKeyValue = false,
                IsNumeric = false,
                ValueSeparator = "=",
                EnumDefinition = null,
                IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag)
            });
        }

        return options;
    }

    /// <summary>
    /// Checks if help text indicates the command has options.
    /// </summary>
    protected override bool HasOptions(string helpText)
    {
        return helpText.Contains("--") || helpText.Contains("Options:");
    }

    #region Regex Patterns

    /// <summary>
    /// Matches "Commands:" section.
    /// </summary>
    [GeneratedRegex(@"Commands:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex CommandsSectionPattern();

    /// <summary>
    /// Matches command lines: "  update                         Deploy any changes"
    /// </summary>
    [GeneratedRegex(@"^\s{2,}(?<name>[\w-]+)\s{2,}", RegexOptions.Multiline)]
    private static partial Regex CommandLinePattern();

    /// <summary>
    /// Matches next section header.
    /// </summary>
    [GeneratedRegex(@"\n[A-Z][\w\s]+:\s*\n")]
    private static partial Regex NextSectionPattern();

    /// <summary>
    /// Matches liquibase-style option lines:
    ///   --changeLogFile=PARAM          The root changelog file
    ///   --verbose                      Enable verbose output
    /// </summary>
    [GeneratedRegex(@"^\s+(?<long>--[\w-]+)(?:=(?<value>\w+))?\s{2,}(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex LiquibaseOptionPattern();

    #endregion
}
