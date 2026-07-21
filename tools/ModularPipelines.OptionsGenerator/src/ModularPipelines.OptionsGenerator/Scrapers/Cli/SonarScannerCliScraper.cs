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
        ExecutablePath = ResolveExecutablePath();
    }

    public override string ToolName => "sonar-scanner";

    public override string NamespacePrefix => "SonarScanner";

    public override string TargetNamespace => "ModularPipelines.SonarScanner";

    public override string OutputDirectory => "src/ModularPipelines.SonarScanner";

    protected override string ExecutablePath { get; }

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
            DocumentationUrl = "https://docs.sonarsource.com/sonarqube-server/analyzing-source-code/scanners/sonarscanner/",
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
            var isKeyValue = longForm == "--define";
            var csharpType = isFlag ? "bool?" : isKeyValue ? "IEnumerable<KeyValue>?" : "string?";

            options.Add(new CliOptionDefinition
            {
                SwitchName = longForm,
                ShortForm = string.IsNullOrEmpty(shortForm) ? null : shortForm,
                PropertyName = propertyName,
                CSharpType = csharpType,
                Description = description,
                IsFlag = isFlag,
                IsRequired = false,
                AcceptsMultipleValues = isKeyValue,
                IsKeyValue = isKeyValue,
                IsNumeric = false,
                ValueSeparator = " ",
                EnumDefinition = null,
                IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag)
            });
        }

        return options;
    }

    private static string ResolveExecutablePath()
    {
        if (!OperatingSystem.IsWindows())
        {
            return "sonar-scanner";
        }

        var pathDirectories = Environment.GetEnvironmentVariable("PATH")?
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            ?? [];

        foreach (var pathDirectory in pathDirectories)
        {
            var candidate = Path.Combine(pathDirectory.Trim('"'), "sonar-scanner.bat");
            if (File.Exists(candidate))
            {
                return Path.GetFullPath(candidate);
            }
        }

        return "sonar-scanner.bat";
    }

    /// <summary>
    /// Gets common SonarQube properties that are passed via -D.
    /// </summary>
    private static List<CliOptionDefinition> GetCommonSonarProperties()
    {
        return
        [
            CreateProperty("-Dsonar.projectKey", "ProjectKey", "The project's unique key. Allowed characters are: letters, numbers, -, _, . and :"),
            CreateProperty("-Dsonar.projectName", "ProjectName", "Name of the project that will be displayed on the web interface"),
            CreateProperty("-Dsonar.projectVersion", "ProjectVersion", "The project version"),
            CreateProperty("-Dsonar.sources", "Sources", "Comma-separated paths to directories containing main source files"),
            CreateProperty("-Dsonar.tests", "Tests", "Comma-separated paths to directories containing test source files"),
            CreateProperty("-Dsonar.projectBaseDir", "ProjectBaseDir", "Root directory of the project to analyze"),
            CreateProperty("-Dproject.settings", "ProjectSettings", "Path to the project settings file"),
            CreateProperty("-Dsonar.sourceEncoding", "SourceEncoding", "Encoding of the source files"),
            CreateProperty("-Dsonar.host.url", "HostUrl", "The server URL"),
            CreateProperty("-Dsonar.token", "Token", "Authentication token for the SonarQube server"),
            CreateProperty("-Dsonar.organization", "Organization", "Organization key (required for SonarCloud)"),
            CreateProperty("-Dsonar.region", "Region", "SonarQube Cloud region"),
            CreateProperty("-Dsonar.exclusions", "Exclusions", "Comma-separated file path patterns to exclude from analysis"),
            CreateProperty("-Dsonar.coverage.exclusions", "CoverageExclusions", "Comma-separated file path patterns to exclude from coverage analysis"),
        ];
    }

    private static CliOptionDefinition CreateProperty(string switchName, string propertyName, string description)
    {
        return new CliOptionDefinition
        {
            SwitchName = switchName,
            PropertyName = propertyName,
            CSharpType = "string?",
            Description = description,
            ValueSeparator = "=",
            IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag: false),
        };
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
