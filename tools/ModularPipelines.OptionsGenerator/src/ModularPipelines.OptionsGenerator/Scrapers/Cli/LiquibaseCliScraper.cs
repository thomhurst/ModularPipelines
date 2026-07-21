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
    private static readonly HashSet<string> NumericOptions = new(StringComparer.OrdinalIgnoreCase)
    {
        "--changelog-lock-poll-rate",
        "--changelog-lock-wait-time-in-minutes",
        "--count",
        "--db-port",
        "--ddl-lock-timeout",
        "--max-rows",
        "--mssql-bytes-per-char",
        "--port",
        "--web-port"
    };

    private static readonly IReadOnlyDictionary<string, string[]> EnumValues =
        new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["--changelog-parse-mode"] = ["strict", "lax"],
            ["--duplicate-file-mode"] = ["silent", "debug", "info", "warn", "error"],
            ["--log-format"] = ["text", "json", "json_pretty"],
            ["--log-level"] = ["off", "severe", "warning", "info", "fine"],
            ["--missing-property-mode"] = ["preserve", "empty", "error"],
            ["--on-missing-include-changelog"] = ["warn", "fail"],
            ["--show-summary"] = ["off", "summary", "verbose"],
            ["--show-summary-output"] = ["log", "console", "all"],
            ["--supports-method-validation-level"] = ["off", "warn", "fail"],
            ["--tag-version"] = ["oldest", "newest"],
            ["--ui-service"] = ["console", "logger"]
        };

    public LiquibaseCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<LiquibaseCliScraper> logger)
        : base(executor, helpCache, logger)
    {
        ExecutablePath = ResolveExecutablePath();
    }

    public override string ToolName => "liquibase";

    public override string NamespacePrefix => "Liquibase";

    public override string TargetNamespace => "ModularPipelines.Liquibase";

    public override string OutputDirectory => "src/ModularPipelines.Liquibase";

    protected override string ExecutablePath { get; }

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "version", "lpm"
    };

    /// <summary>
    /// Extracts subcommand names from liquibase help text.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        var seenCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find "Commands" section
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
        var enums = options
            .Where(x => x.EnumDefinition is not null)
            .Select(x => x.EnumDefinition!)
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
            DocumentationUrl = "https://docs.liquibase.com/commands/home.html",
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
        var usageIndex = helpText.IndexOf("Usage:", StringComparison.OrdinalIgnoreCase);
        var preamble = usageIndex < 0 ? helpText : helpText[..usageIndex];
        var description = string.Join(
            " ",
            preamble.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));

        return string.IsNullOrWhiteSpace(description) ? null : description;
    }

    /// <summary>
    /// Parses options from liquibase help text.
    /// Format: --changeLogFile=PARAM          The root changelog file
    ///         --url=PARAM                    The database URL
    /// </summary>
    protected List<CliOptionDefinition> ParseOptions(string helpText)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var lines = helpText.Split('\n');

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var defineMatch = LiquibaseDefinePattern().Match(line);
            if (defineMatch.Success)
            {
                if (seenOptions.Add("-D"))
                {
                    var defineDescription = defineMatch.Groups["desc"].Value.Trim();
                    i = AccumulateMultiLineDescription(lines, i, ref defineDescription);
                    options.Add(CreateDefineOption(CleanDescription(defineDescription)));
                }

                continue;
            }

            var match = LiquibaseOptionPattern().Match(line);
            if (!match.Success)
            {
                continue;
            }

            var shortForm = match.Groups["short"].Value.Trim();
            var longForm = match.Groups["long"].Value.Trim();
            var valueHint = match.Groups["value"].Value.Trim();
            var description = match.Groups["desc"].Value.Trim();
            i = AccumulateMultiLineDescription(lines, i, ref description);

            if (!seenOptions.Add(longForm))
            {
                continue;
            }

            var propertyName = NormalizePropertyName(longForm);
            if (propertyName is null)
            {
                continue;
            }

            var isFlag = string.IsNullOrEmpty(valueHint);
            var isBoolean = !isFlag && BooleanDefaultPattern().IsMatch(description);
            var isNumeric = !isFlag && NumericOptions.Contains(longForm);
            var enumDefinition = TryCreateEnumDefinition(longForm, propertyName);
            var csharpType = DetermineCSharpType(isFlag, isBoolean, isNumeric, enumDefinition);
            var isRequired = description.Contains("[REQUIRED]", StringComparison.OrdinalIgnoreCase);

            options.Add(new CliOptionDefinition
            {
                SwitchName = longForm,
                ShortForm = string.IsNullOrEmpty(shortForm) ? null : shortForm,
                PropertyName = propertyName,
                CSharpType = csharpType,
                Description = CleanDescription(description),
                IsFlag = isFlag,
                IsRequired = isRequired,
                AcceptsMultipleValues = false,
                IsKeyValue = false,
                IsNumeric = isNumeric,
                ValueSeparator = "=",
                EnumDefinition = enumDefinition,
                IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag)
            });
        }

        return options;
    }

    private static CliOptionDefinition CreateDefineOption(string? description)
    {
        const string propertyName = "ChangelogProperty";
        return new CliOptionDefinition
        {
            SwitchName = "-D",
            PropertyName = propertyName,
            CSharpType = "IEnumerable<KeyValue>?",
            Description = description,
            IsFlag = false,
            IsRequired = false,
            AcceptsMultipleValues = true,
            IsKeyValue = true,
            IsNumeric = false,
            ValueSeparator = string.Empty,
            EnumDefinition = null,
            IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag: false)
        };
    }

    private static string DetermineCSharpType(
        bool isFlag,
        bool isBoolean,
        bool isNumeric,
        CliEnumDefinition? enumDefinition)
    {
        if (isFlag || isBoolean)
        {
            return "bool?";
        }

        if (enumDefinition is not null)
        {
            return $"{enumDefinition.EnumName}?";
        }

        return isNumeric ? "int?" : "string?";
    }

    private static CliEnumDefinition? TryCreateEnumDefinition(string longForm, string propertyName)
    {
        if (!EnumValues.TryGetValue(longForm, out var values))
        {
            return null;
        }

        return new CliEnumDefinition
        {
            EnumName = $"Liquibase{propertyName}",
            Description = $"Allowed values for {longForm}.",
            Values = values.Select(value => new CliEnumValue
            {
                MemberName = ToPascalCase(value),
                CliValue = value
            }).ToList()
        };
    }

    private static int AccumulateMultiLineDescription(string[] lines, int currentIndex, ref string description)
    {
        var descriptionParts = new List<string> { description };
        var nextIndex = currentIndex + 1;

        while (nextIndex < lines.Length)
        {
            var nextLine = lines[nextIndex];
            if (string.IsNullOrWhiteSpace(nextLine) ||
                LiquibaseOptionPattern().IsMatch(nextLine) ||
                LiquibaseDefinePattern().IsMatch(nextLine))
            {
                break;
            }

            var leadingSpaces = nextLine.Length - nextLine.TrimStart().Length;
            if (leadingSpaces < 20)
            {
                break;
            }

            descriptionParts.Add(nextLine.Trim());
            nextIndex++;
        }

        description = string.Join(" ", descriptionParts.Where(x => !string.IsNullOrWhiteSpace(x)));
        return nextIndex - 1;
    }

    private static string? CleanDescription(string description)
    {
        var cleaned = DefaultsMetadataPattern().Replace(description, string.Empty)
            .Replace("[REQUIRED]", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Trim();
        return string.IsNullOrEmpty(cleaned) ? null : cleaned;
    }

    private static string ResolveExecutablePath()
    {
        if (!OperatingSystem.IsWindows())
        {
            return "liquibase";
        }

        var pathDirectories = Environment.GetEnvironmentVariable("PATH")?
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            ?? [];

        foreach (var pathDirectory in pathDirectories)
        {
            var candidate = Path.Combine(pathDirectory.Trim('"'), "liquibase.bat");
            if (File.Exists(candidate))
            {
                return Path.GetFullPath(candidate);
            }
        }

        return "liquibase.bat";
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
    [GeneratedRegex(@"^Commands:?\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex CommandsSectionPattern();

    /// <summary>
    /// Matches command lines: "  update                         Deploy any changes"
    /// </summary>
    [GeneratedRegex(@"^\s{2,}(?<name>[\w-]+)(?:,\s*[\w-]+)?\s{2,}", RegexOptions.Multiline)]
    private static partial Regex CommandLinePattern();

    /// <summary>
    /// Matches next section header.
    /// </summary>
    [GeneratedRegex(@"^(?:Each argument|Available configuration|Full documentation)", RegexOptions.Multiline)]
    private static partial Regex NextSectionPattern();

    /// <summary>
    /// Matches liquibase-style option lines:
    ///   --changeLogFile=PARAM          The root changelog file
    ///   --verbose                      Enable verbose output
    /// </summary>
    [GeneratedRegex(@"^\s+(?:(?<short>-[A-Za-z]),\s*)?(?<long>--[\w-]+)(?:\[?=(?<value>PARAM)\]?)?\s+(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex LiquibaseOptionPattern();

    [GeneratedRegex(@"^\s+-D=PARAM\s+(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex LiquibaseDefinePattern();

    [GeneratedRegex(@"DEFAULT:\s*(?:true|false)\b", RegexOptions.IgnoreCase)]
    private static partial Regex BooleanDefaultPattern();

    [GeneratedRegex(@"\s*\(defaults file:.*$", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private static partial Regex DefaultsMetadataPattern();

    #endregion
}
