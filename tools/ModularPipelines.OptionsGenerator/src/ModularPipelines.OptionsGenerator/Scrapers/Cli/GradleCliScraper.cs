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
            PositionalArguments = [],
            SubDomainGroup = null,
            Enums = []
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    /// <summary>
    /// Parses options from Gradle help text.
    /// Format: -a, --no-rebuild                   Do not rebuild project dependencies.
    ///         --build-cache                      Enables the Gradle build cache.
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(string helpText)
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

            var shortForms = match.Groups["short"].Value.Trim();
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

            // Determine if this is a flag or takes a value
            var isFlag = !description.Contains("Specifies") &&
                         !description.Contains("path") &&
                         !description.Contains("file") &&
                         !description.Contains("directory");
            var csharpType = isFlag ? "bool?" : "string?";

            options.Add(new CliOptionDefinition
            {
                SwitchName = longForm,
                ShortForm = shortForm,
                PropertyName = propertyName,
                CSharpType = csharpType,
                Description = description.Replace("[deprecated]", "").Trim(),
                IsFlag = isFlag,
                IsRequired = false,
                AcceptsMultipleValues = false,
                IsKeyValue = longForm == "--project-prop" || longForm == "--system-prop",
                IsNumeric = false,
                ValueSeparator = isFlag ? " " : "=",
                EnumDefinition = null,
                IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag)
            });
        }

        return options;
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
    /// -a, --no-rebuild                   Do not rebuild project dependencies.
    /// -?, -h, --help                     Shows this help message.
    ///     --build-cache                  Enables the Gradle build cache.
    /// </summary>
    [GeneratedRegex(@"^\s*(?:(?<short>(?:-\w,?\s*)+),\s*)?(?<long>--[\w-]+)\s{2,}(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex GradleOptionPattern();

    #endregion
}
