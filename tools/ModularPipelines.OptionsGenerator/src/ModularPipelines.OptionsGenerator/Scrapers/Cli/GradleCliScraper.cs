using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Gradle build tool.
/// Gradle uses a custom help format.
///
/// gradle help format (gradle --help):
/// USAGE: gradle [option...] [task...]
///
/// -?, -h, --help                     Shows this help message.
/// -a, --no-rebuild                   Do not rebuild project dependencies.
/// -b, --build-file                   Specify the build file. [deprecated]
/// ...
/// </summary>
public partial class GradleCliScraper : CliScraperBase
{
    private static readonly HashSet<string> ValueOptions = new(StringComparer.OrdinalIgnoreCase)
    {
        "--configuration-cache-problems",
        "--console",
        "--console-unicode",
        "--dependency-verification",
        "--develocity-plugin-version",
        "--develocity-url",
        "--exclude-task",
        "--gradle-user-home",
        "--include-build",
        "--init-script",
        "--max-workers",
        "--priority",
        "--project-cache-dir",
        "--project-dir",
        "--project-prop",
        "--system-prop",
        "--update-locks",
        "--warning-mode",
        "--write-verification-metadata"
    };

    private static readonly HashSet<string> RepeatableOptions = new(StringComparer.OrdinalIgnoreCase)
    {
        "--exclude-task",
        "--include-build",
        "--init-script",
        "--project-prop",
        "--system-prop"
    };

    private static readonly HashSet<string> KeyValueOptions = new(StringComparer.OrdinalIgnoreCase)
    {
        "--project-prop",
        "--system-prop"
    };

    public GradleCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<GradleCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "gradle";

    public override string NamespacePrefix => "Gradle";

    public override string TargetNamespace => "ModularPipelines.Java";

    public override string OutputDirectory => "src/ModularPipelines.Java";

    /// <summary>
    /// Gradle doesn't have subcommands - it uses tasks.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        // Gradle uses tasks, not subcommands
        return [];
    }

    /// <summary>
    /// Parses Gradle command options from help text.
    /// </summary>
    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken)
    {
        var description = "Gradle Build Tool - Automate building, testing, publishing, and deploying software";

        var options = ParseOptions(helpText);
        var enums = options
            .Where(x => x.EnumDefinition is not null)
            .Select(x => x.EnumDefinition!)
            .ToList();

        var command = new CliCommandDefinition
        {
            FullCommand = "gradle",
            CommandParts = [],
            ClassName = "GradleOptions",
            ParentClassName = BaseOptionsClassName,
            ToolNamespacePrefix = NamespacePrefix,
            Description = description,
            DocumentationUrl = "https://docs.gradle.org/current/userguide/command_line_interface.html",
            Options = options,
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Tasks",
                    PlaceholderName = "task(s)",
                    CSharpType = "IEnumerable<string>?",
                    Description = "Gradle tasks to execute.",
                    PositionIndex = 0,
                    IsRequired = false,
                    Placement = PositionalArgumentPosition.AfterOptions
                }
            ],
            SubDomainGroup = null,
            Enums = enums
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    /// <summary>
    /// Parses options from Gradle help text.
    /// Format: -a, --no-rebuild                   Do not rebuild project dependencies.
    ///         --build-cache                      Enables the Gradle build cache.
    /// </summary>
    protected List<CliOptionDefinition> ParseOptions(string helpText)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var lines = helpText.Split('\n');

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];

            var match = GradleOptionPattern().Match(line);
            if (!match.Success)
            {
                continue;
            }

            var shortForms = match.Groups["shorts"].Value.Trim();
            var longForm = match.Groups["long"].Value.Trim();
            var description = match.Groups["desc"].Value.Trim();

            if (string.IsNullOrEmpty(longForm))
            {
                continue;
            }

            // Skip duplicates
            if (seenOptions.Contains(longForm))
            {
                continue;
            }

            seenOptions.Add(longForm);

            i = AccumulateMultiLineDescription(lines, i, ref description);

            // Skip deprecated options
            if (description.Contains("[deprecated]", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            // Extract single short form (handle -?, -h case)
            var shortForm = ExtractShortForm(shortForms);

            var propertyName = NormalizePropertyName(longForm);
            if (propertyName is null)
            {
                continue;
            }

            var isFlag = !ValueOptions.Contains(longForm);
            var acceptsMultipleValues = RepeatableOptions.Contains(longForm);
            var isKeyValue = KeyValueOptions.Contains(longForm);
            var enumDefinition = TryCreateEnumDefinition(longForm, propertyName, description);
            var isNumeric = longForm == "--max-workers";
            var csharpType = DetermineCSharpType(
                isFlag,
                acceptsMultipleValues,
                isKeyValue,
                isNumeric,
                enumDefinition);

            options.Add(new CliOptionDefinition
            {
                SwitchName = longForm,
                ShortForm = shortForm,
                PropertyName = propertyName,
                CSharpType = csharpType,
                Description = description.Replace("[deprecated]", "").Trim(),
                IsFlag = isFlag,
                IsRequired = false,
                AcceptsMultipleValues = acceptsMultipleValues,
                IsKeyValue = isKeyValue,
                IsNumeric = isNumeric,
                ValueSeparator = " ",
                EnumDefinition = enumDefinition,
                IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag)
            });
        }

        return options;
    }

    private static string DetermineCSharpType(
        bool isFlag,
        bool acceptsMultipleValues,
        bool isKeyValue,
        bool isNumeric,
        CliEnumDefinition? enumDefinition)
    {
        if (isFlag)
        {
            return "bool?";
        }

        if (enumDefinition is not null)
        {
            return $"{enumDefinition.EnumName}?";
        }

        if (isKeyValue)
        {
            return "IEnumerable<KeyValue>?";
        }

        if (acceptsMultipleValues)
        {
            return "IEnumerable<string>?";
        }

        return isNumeric ? "int?" : "string?";
    }

    private static int AccumulateMultiLineDescription(string[] lines, int currentIndex, ref string description)
    {
        var descriptionParts = new List<string> { description };
        var nextIndex = currentIndex + 1;

        while (nextIndex < lines.Length)
        {
            var nextLine = lines[nextIndex];
            if (string.IsNullOrWhiteSpace(nextLine) || GradleOptionPattern().IsMatch(nextLine))
            {
                break;
            }

            var leadingSpaces = nextLine.Length - nextLine.TrimStart().Length;
            if (leadingSpaces < 4)
            {
                break;
            }

            descriptionParts.Add(nextLine.Trim());
            nextIndex++;
        }

        description = string.Join(" ", descriptionParts.Where(x => !string.IsNullOrWhiteSpace(x)));
        return nextIndex - 1;
    }

    private static CliEnumDefinition? TryCreateEnumDefinition(string longForm, string propertyName, string description)
    {
        if (!description.Contains("Supported values", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var values = QuotedValuePattern().Matches(description)
            .Select(match => match.Groups["value"].Value)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (values.Count < 2)
        {
            return null;
        }

        return new CliEnumDefinition
        {
            EnumName = $"Gradle{propertyName}",
            Description = $"Allowed values for {longForm}.",
            Values = values.Select(value => new CliEnumValue
            {
                MemberName = ToPascalCase(value),
                CliValue = value
            }).ToList()
        };
    }

    /// <summary>
    /// Extracts a usable short form from potentially multiple short forms (e.g., "-?, -h" -> "-h").
    /// </summary>
    private static string? ExtractShortForm(string shortForms)
    {
        if (string.IsNullOrWhiteSpace(shortForms))
        {
            return null;
        }

        // Split on comma and find a valid short form (not -?)
        var forms = shortForms.Split(',', StringSplitOptions.RemoveEmptyEntries);
        foreach (var form in forms)
        {
            var trimmed = form.Trim();
            if (trimmed.Length == 2 && trimmed.StartsWith('-') && char.IsLetter(trimmed[1]))
            {
                return trimmed;
            }
        }

        return null;
    }

    /// <summary>
    /// Checks if help text indicates the command has options.
    /// </summary>
    protected override bool HasOptions(string helpText)
    {
        return helpText.Contains("--") || helpText.Contains("USAGE:");
    }

    #region Regex Patterns

    /// <summary>
    /// Matches Gradle-style option lines:
    /// --no-rebuild, -a                   Do not rebuild project dependencies.
    /// --help, -?, -h                     Shows this help message.
    /// --build-cache                      Enables the Gradle build cache.
    /// </summary>
    [GeneratedRegex(@"^\s{2,}(?<long>--[\w-]+)(?<shorts>(?:,\s*-[^\s,]+)*)\s{2,}(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex GradleOptionPattern();

    [GeneratedRegex(@"'(?<value>[^']+)'")]
    private static partial Regex QuotedValuePattern();

    #endregion
}
