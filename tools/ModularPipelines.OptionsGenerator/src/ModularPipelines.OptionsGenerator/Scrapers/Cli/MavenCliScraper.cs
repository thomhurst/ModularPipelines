using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Apache Maven build tool.
/// Maven uses a traditional options format.
///
/// mvn help format (mvn -h):
/// usage: mvn [options] [&lt;goal(s)&gt;] [&lt;phase(s)&gt;]
///
/// Options:
///  -am,--also-make                        If project list is specified, also
///                                         build projects required by the list
///  -amd,--also-make-dependents            If project list is specified, also
///                                         build projects that depend on projects
///                                         on the list
///  -B,--batch-mode                        Run in non-interactive (batch) mode
///  ...
/// </summary>
public partial class MavenCliScraper : CliScraperBase
{
    public MavenCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<MavenCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "mvn";

    public override string NamespacePrefix => "Maven";

    public override string TargetNamespace => "ModularPipelines.Java";

    public override string OutputDirectory => "src/ModularPipelines.Java";

    /// <summary>
    /// Maven doesn't have subcommands in the traditional sense - it uses goals and phases.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        // Maven uses goals (e.g., clean, compile, test, package, install, deploy)
        // These are not really subcommands but lifecycle phases
        // We return an empty list since Maven is a single-command tool with many options
        return [];
    }

    /// <summary>
    /// Gets help text using mvn -h (not --help).
    /// </summary>
    protected override async Task<string?> GetHelpTextAsync(
        string[] commandPath,
        CancellationToken cancellationToken)
    {
        var cacheKey = string.Join(" ", commandPath);

        if (HelpCache.TryGet(cacheKey, out var cached))
        {
            return cached;
        }

        // Maven uses -h for help
        var result = await Executor.ExecuteAsync(ExecutablePath, "-h", cancellationToken);

        var helpText = !string.IsNullOrEmpty(result.StandardOutput)
            ? result.StandardOutput
            : result.StandardError;

        if (!string.IsNullOrWhiteSpace(helpText))
        {
            HelpCache.Set(cacheKey, helpText);
            return helpText;
        }

        Logger.LogWarning("No help text for command: {Command}", cacheKey);
        return null;
    }

    /// <summary>
    /// Parses Maven command options from help text.
    /// </summary>
    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken)
    {
        // Parse description
        var description = "Apache Maven - Project Management and Comprehension Tool";

        // Parse options from the help text
        var options = ParseOptions(helpText);

        var command = new CliCommandDefinition
        {
            FullCommand = "mvn",
            CommandParts = [],
            ClassName = "MavenOptions",
            ParentClassName = BaseOptionsClassName,
            ToolNamespacePrefix = NamespacePrefix,
            Description = description,
            DocumentationUrl = "https://maven.apache.org/ref/current/maven-embedder/cli.html",
            Options = options,
            PositionalArguments = [],
            SubDomainGroup = null,
            Enums = []
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    /// <summary>
    /// Parses options from Maven help text.
    /// Format: -am,--also-make                        Description
    ///         -B,--batch-mode                        Description
    ///         -D,--define &lt;arg&gt;                     Description
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(string helpText)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find "Options:" section
        var optionsMatch = OptionsSectionPattern().Match(helpText);
        if (!optionsMatch.Success)
        {
            return options;
        }

        var sectionStart = optionsMatch.Index + optionsMatch.Length;
        var section = helpText.Substring(sectionStart);
        var lines = section.Split('\n');

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];

            var match = MavenOptionPattern().Match(line);
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
                    // Some Maven options only have short form, skip for now
                    continue;
                }
                continue;
            }

            // Skip duplicates
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

            var isFlag = string.IsNullOrEmpty(valueHint);
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
                IsKeyValue = longForm == "--define",
                IsNumeric = false,
                ValueSeparator = " ",
                EnumDefinition = null,
                IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag)
            });
        }

        return options;
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

            // If line starts with -, it's a new option
            if (trimmedNext.StartsWith('-'))
            {
                break;
            }

            // Check indentation - continuation lines are heavily indented
            var leadingSpaces = nextLine.Length - nextLine.TrimStart().Length;
            if (leadingSpaces < 30)
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
        return helpText.Contains("Options:") || helpText.Contains("--");
    }

    #region Regex Patterns

    /// <summary>
    /// Matches "Options:" section header.
    /// </summary>
    [GeneratedRegex(@"Options:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex OptionsSectionPattern();

    /// <summary>
    /// Matches Maven-style option lines:
    ///  -am,--also-make                        Description
    ///  -D,--define &lt;arg&gt;                     Description
    /// </summary>
    [GeneratedRegex(@"^\s*(?:(?<short>-\w+),)?(?<long>--[\w-]+)(?:\s+(?<value><[^>]+>))?\s{2,}(?<desc>.*)?$", RegexOptions.Multiline)]
    private static partial Regex MavenOptionPattern();

    #endregion
}
