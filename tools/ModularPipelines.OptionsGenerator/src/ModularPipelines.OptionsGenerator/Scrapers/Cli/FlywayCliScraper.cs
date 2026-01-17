using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Flyway database migration tool.
/// Flyway uses a custom help format.
///
/// flyway help format (flyway --help):
/// Usage
/// =====
/// flyway [options] command
///
/// Commands
/// --------
/// migrate  : Migrates the database
/// clean    : Drops all objects in the configured schemas
/// info     : Prints the information about applied, current and pending migrations
/// validate : Validates the applied migrations against the ones on the classpath
/// ...
///
/// Configuration
/// -------------
/// -url=                         : Jdbc url to use to connect to the database
/// -user=                        : User to use to connect to the database
/// ...
/// </summary>
public partial class FlywayCliScraper : CliScraperBase
{
    public FlywayCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<FlywayCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "flyway";

    public override string NamespacePrefix => "Flyway";

    public override string TargetNamespace => "ModularPipelines.Flyway";

    public override string OutputDirectory => "src/ModularPipelines.Flyway";

    /// <summary>
    /// Flyway is a single-level CLI (flyway [options] command), not multi-level.
    /// Limit depth to prevent the scraper from treating repeated help output as nested subcommands.
    /// </summary>
    protected override int MaxCommandDepth => 2; // flyway + command = 2

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "version"
    };

    /// <summary>
    /// Extracts subcommand names from Flyway help text.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        var seenCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find "Commands" section
        var commandsSectionMatch = FlywayCommandsSectionPattern().Match(helpText);
        if (!commandsSectionMatch.Success)
        {
            return subcommands;
        }

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
            var match = FlywayCommandLinePattern().Match(line);
            if (match.Success)
            {
                var commandName = match.Groups["command"].Value.Trim();
                if (!string.IsNullOrEmpty(commandName) &&
                    !commandName.Contains(' ') &&
                    seenCommands.Add(commandName))
                {
                    subcommands.Add(commandName);
                }
            }
        }

        return subcommands;
    }

    /// <summary>
    /// Parses a Flyway command from its help text.
    /// </summary>
    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken)
    {
        var commandParts = commandPath.Skip(1).ToArray();

        // Parse description
        var description = commandParts.Length == 0
            ? "Flyway - Database migration tool"
            : ExtractDescription(helpText, commandParts);

        // Parse options from the help text
        var options = ParseOptions(helpText);

        var className = commandParts.Length == 0
            ? "FlywayOptions"
            : GenerateClassName(commandPath);

        var command = new CliCommandDefinition
        {
            FullCommand = string.Join(" ", commandPath),
            CommandParts = commandParts,
            ClassName = className,
            ParentClassName = BaseOptionsClassName,
            ToolNamespacePrefix = NamespacePrefix,
            Description = description,
            DocumentationUrl = "https://documentation.red-gate.com/fd/command-line-184127404.html",
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
    private static string? ExtractDescription(string helpText, string[] commandParts)
    {
        if (commandParts.Length == 0)
        {
            return "Flyway - Database migration tool";
        }

        // Try to find description for this command in the Commands section
        var commandName = commandParts[0];
        var pattern = $@"{commandName}\s*:\s*(?<desc>[^\n]+)";
        var match = Regex.Match(helpText, pattern, RegexOptions.IgnoreCase);
        if (match.Success)
        {
            return match.Groups["desc"].Value.Trim();
        }

        return null;
    }

    /// <summary>
    /// Parses options from Flyway help text.
    /// Format: -url=                         : Jdbc url to use to connect to the database
    ///         -user=                        : User to use to connect to the database
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(string helpText)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find "Configuration" section
        var configSectionMatch = ConfigurationSectionPattern().Match(helpText);
        if (!configSectionMatch.Success)
        {
            return options;
        }

        var sectionStart = configSectionMatch.Index + configSectionMatch.Length;
        var section = helpText.Substring(sectionStart);
        var lines = section.Split('\n');

        foreach (var line in lines)
        {
            var match = FlywayOptionPattern().Match(line);
            if (!match.Success)
            {
                continue;
            }

            var optionName = match.Groups["option"].Value.Trim();
            var description = match.Groups["desc"].Value.Trim();

            if (string.IsNullOrEmpty(optionName) || seenOptions.Contains(optionName))
            {
                continue;
            }

            seenOptions.Add(optionName);

            var propertyName = NormalizePropertyName(optionName);
            if (propertyName is null)
            {
                continue;
            }

            // Flyway options are key=value style, not flags
            var csharpType = "string?";

            options.Add(new CliOptionDefinition
            {
                SwitchName = $"-{optionName}",
                ShortForm = null,
                PropertyName = propertyName,
                CSharpType = csharpType,
                Description = description,
                IsFlag = false,
                IsRequired = false,
                AcceptsMultipleValues = optionName.Contains("locations"),
                IsKeyValue = false,
                IsNumeric = optionName.Contains("batch") || optionName.Contains("timeout"),
                ValueSeparator = "=",
                EnumDefinition = null,
                IsSecret = GeneratorUtils.IsSecretOption(propertyName, false)
            });
        }

        return options;
    }

    /// <summary>
    /// Checks if help text indicates the command has options.
    /// </summary>
    protected override bool HasOptions(string helpText)
    {
        return helpText.Contains("Configuration") || helpText.Contains("-url=");
    }

    #region Regex Patterns

    /// <summary>
    /// Matches "Commands" section header.
    /// Also matches "Available Commands:" format used in newer flyway versions.
    /// </summary>
    [GeneratedRegex(@"(?:Available\s+)?Commands?\s*[-=:]*\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex FlywayCommandsSectionPattern();

    /// <summary>
    /// Matches "Configuration" section header.
    /// </summary>
    [GeneratedRegex(@"Configuration\s*[-=]*\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex ConfigurationSectionPattern();

    /// <summary>
    /// Matches next section headers.
    /// </summary>
    [GeneratedRegex(@"\n\w+\s*[-=]+\s*\n")]
    private static partial Regex NextSectionPattern();

    /// <summary>
    /// Matches Flyway command lines: "migrate  : Migrates the database"
    /// Also matches "  migrate         Migrates the database" format (no colon).
    /// </summary>
    [GeneratedRegex(@"^(?:\s*)(?<command>[\w-]+)(?:\s+:\s+|\s{2,})", RegexOptions.Multiline)]
    private static partial Regex FlywayCommandLinePattern();

    /// <summary>
    /// Matches Flyway-style option lines:
    /// -url=                         : Jdbc url to use to connect to the database
    /// </summary>
    [GeneratedRegex(@"^\s*-(?<option>[\w.]+)=?\s*:\s*(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex FlywayOptionPattern();

    #endregion
}
