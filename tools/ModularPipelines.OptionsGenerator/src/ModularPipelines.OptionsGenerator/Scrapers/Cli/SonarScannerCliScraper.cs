using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for SonarScanner CLI.
/// SonarScanner uses a simple -D property style for most configuration.
///
/// sonar-scanner help format (sonar-scanner -h):
/// usage: sonar-scanner [options]
///
///  -D,--define &lt;arg&gt;     Define property
///  -h,--help              Display help information
///  -v,--version           Display version information
///  -X,--debug             Produce execution debug output
/// </summary>
public partial class SonarScannerCliScraper : CliScraperBase
{
    public SonarScannerCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<SonarScannerCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "sonar-scanner";

    public override string NamespacePrefix => "SonarScanner";

    public override string TargetNamespace => "ModularPipelines.SonarScanner";

    public override string OutputDirectory => "src/ModularPipelines.SonarScanner";

    /// <summary>
    /// SonarScanner doesn't have subcommands.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        return [];
    }

    /// <summary>
    /// Gets help text using sonar-scanner -h.
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
    /// Parses SonarScanner command options from help text.
    /// </summary>
    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken)
    {
        var description = "SonarScanner - Scanner CLI for SonarQube and SonarCloud";

        var options = ParseOptions(helpText);

        // Add common SonarQube properties as options
        options.AddRange(GetCommonSonarProperties());

        var command = new CliCommandDefinition
        {
            FullCommand = "sonar-scanner",
            CommandParts = [],
            ClassName = "SonarScannerOptions",
            ParentClassName = BaseOptionsClassName,
            ToolNamespacePrefix = NamespacePrefix,
            Description = description,
            DocumentationUrl = "https://docs.sonarqube.org/latest/analyzing-source-code/scanners/sonarscanner/",
            Options = options,
            PositionalArguments = [],
            SubDomainGroup = null,
            Enums = []
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    /// <summary>
    /// Parses options from SonarScanner help text.
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(string helpText)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var lines = helpText.Split('\n');

        foreach (var line in lines)
        {
            var match = SonarOptionPattern().Match(line);
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
    /// Gets common SonarQube properties that are passed via -D.
    /// </summary>
    private static List<CliOptionDefinition> GetCommonSonarProperties()
    {
        return
        [
            new CliOptionDefinition
            {
                SwitchName = "-Dsonar.projectKey",
                PropertyName = "ProjectKey",
                CSharpType = "string?",
                Description = "The project's unique key. Allowed characters are: letters, numbers, -, _, . and :",
                IsFlag = false,
                IsRequired = false,
                ValueSeparator = "=",
                IsSecret = GeneratorUtils.IsSecretOption("ProjectKey", false)
            },
            new CliOptionDefinition
            {
                SwitchName = "-Dsonar.projectName",
                PropertyName = "ProjectName",
                CSharpType = "string?",
                Description = "Name of the project that will be displayed on the web interface",
                IsFlag = false,
                IsRequired = false,
                ValueSeparator = "=",
                IsSecret = GeneratorUtils.IsSecretOption("ProjectName", false)
            },
            new CliOptionDefinition
            {
                SwitchName = "-Dsonar.projectVersion",
                PropertyName = "ProjectVersion",
                CSharpType = "string?",
                Description = "The project version",
                IsFlag = false,
                IsRequired = false,
                ValueSeparator = "=",
                IsSecret = GeneratorUtils.IsSecretOption("ProjectVersion", false)
            },
            new CliOptionDefinition
            {
                SwitchName = "-Dsonar.sources",
                PropertyName = "Sources",
                CSharpType = "string?",
                Description = "Comma-separated paths to directories containing main source files",
                IsFlag = false,
                IsRequired = false,
                ValueSeparator = "=",
                IsSecret = GeneratorUtils.IsSecretOption("Sources", false)
            },
            new CliOptionDefinition
            {
                SwitchName = "-Dsonar.tests",
                PropertyName = "Tests",
                CSharpType = "string?",
                Description = "Comma-separated paths to directories containing test source files",
                IsFlag = false,
                IsRequired = false,
                ValueSeparator = "=",
                IsSecret = GeneratorUtils.IsSecretOption("Tests", false)
            },
            new CliOptionDefinition
            {
                SwitchName = "-Dsonar.host.url",
                PropertyName = "HostUrl",
                CSharpType = "string?",
                Description = "The server URL",
                IsFlag = false,
                IsRequired = false,
                ValueSeparator = "=",
                IsSecret = GeneratorUtils.IsSecretOption("HostUrl", false)
            },
            new CliOptionDefinition
            {
                SwitchName = "-Dsonar.token",
                PropertyName = "Token",
                CSharpType = "string?",
                Description = "Authentication token for the SonarQube server",
                IsFlag = false,
                IsRequired = false,
                ValueSeparator = "=",
                IsSecret = GeneratorUtils.IsSecretOption("Token", false)
            },
            new CliOptionDefinition
            {
                SwitchName = "-Dsonar.organization",
                PropertyName = "Organization",
                CSharpType = "string?",
                Description = "Organization key (required for SonarCloud)",
                IsFlag = false,
                IsRequired = false,
                ValueSeparator = "=",
                IsSecret = GeneratorUtils.IsSecretOption("Organization", false)
            },
            new CliOptionDefinition
            {
                SwitchName = "-Dsonar.exclusions",
                PropertyName = "Exclusions",
                CSharpType = "string?",
                Description = "Comma-separated file path patterns to exclude from analysis",
                IsFlag = false,
                IsRequired = false,
                ValueSeparator = "=",
                IsSecret = GeneratorUtils.IsSecretOption("Exclusions", false)
            },
            new CliOptionDefinition
            {
                SwitchName = "-Dsonar.coverage.exclusions",
                PropertyName = "CoverageExclusions",
                CSharpType = "string?",
                Description = "Comma-separated file path patterns to exclude from coverage analysis",
                IsFlag = false,
                IsRequired = false,
                ValueSeparator = "=",
                IsSecret = GeneratorUtils.IsSecretOption("CoverageExclusions", false)
            }
        ];
    }

    /// <summary>
    /// Checks if help text indicates the command has options.
    /// </summary>
    protected override bool HasOptions(string helpText)
    {
        return helpText.Contains("--") || helpText.Contains("-D");
    }

    #region Regex Patterns

    /// <summary>
    /// Matches SonarScanner-style option lines:
    ///  -D,--define &lt;arg&gt;     Define property
    ///  -h,--help              Display help information
    /// </summary>
    [GeneratedRegex(@"^\s*(?:(?<short>-\w),)?(?<long>--[\w-]+)(?:\s+(?<value><[^>]+>))?\s+(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex SonarOptionPattern();

    #endregion
}
