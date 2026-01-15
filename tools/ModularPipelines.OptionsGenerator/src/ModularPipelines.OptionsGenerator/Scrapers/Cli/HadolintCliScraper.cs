using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Hadolint - Dockerfile linter.
/// Hadolint uses a standard help format.
///
/// hadolint help format (hadolint --help):
/// hadolint - Lint Dockerfile
///
/// Usage: hadolint [-v|--version] [-c|--config FILENAME] [-f|--format ARG]
///                 [DOCKERFILE...] [-V|--verbose] [--no-fail] [--no-color]
///                 [-t|--trusted-registry REGISTRY] [--ignore RULE]
///                 [--strict-labels] [--failure-threshold THRESHOLD]
///
/// Available options:
///   -h,--help                Show this help text
///   -v,--version             Show version
///   ...
/// </summary>
public partial class HadolintCliScraper : CliScraperBase
{
    public HadolintCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<HadolintCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "hadolint";

    public override string NamespacePrefix => "Hadolint";

    public override string TargetNamespace => "ModularPipelines.Hadolint";

    public override string OutputDirectory => "src/ModularPipelines.Hadolint";

    /// <summary>
    /// Hadolint doesn't have subcommands.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        return [];
    }

    /// <summary>
    /// Parses Hadolint command options from help text.
    /// </summary>
    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken)
    {
        var description = "Hadolint - Dockerfile linter that helps you build best practice Docker images";

        // Add enum for format option
        var formatEnum = new CliEnumDefinition
        {
            EnumName = "HadolintFormat",
            Values =
            [
                new CliEnumValue { MemberName = "Tty", CliValue = "tty" },
                new CliEnumValue { MemberName = "Json", CliValue = "json" },
                new CliEnumValue { MemberName = "Checkstyle", CliValue = "checkstyle" },
                new CliEnumValue { MemberName = "Codeclimate", CliValue = "codeclimate" },
                new CliEnumValue { MemberName = "Gitlab_codeclimate", CliValue = "gitlab_codeclimate" },
                new CliEnumValue { MemberName = "Gnu", CliValue = "gnu" },
                new CliEnumValue { MemberName = "Codacy", CliValue = "codacy" },
                new CliEnumValue { MemberName = "Sonarqube", CliValue = "sonarqube" },
                new CliEnumValue { MemberName = "Sarif", CliValue = "sarif" }
            ],
            Description = "Output format for Hadolint results"
        };

        // Add enum for failure threshold
        var thresholdEnum = new CliEnumDefinition
        {
            EnumName = "HadolintFailureThreshold",
            Values =
            [
                new CliEnumValue { MemberName = "Error", CliValue = "error" },
                new CliEnumValue { MemberName = "Warning", CliValue = "warning" },
                new CliEnumValue { MemberName = "Info", CliValue = "info" },
                new CliEnumValue { MemberName = "Style", CliValue = "style" },
                new CliEnumValue { MemberName = "Ignore", CliValue = "ignore" },
                new CliEnumValue { MemberName = "None", CliValue = "none" }
            ],
            Description = "Failure threshold for Hadolint exit code"
        };

        // Parse options with enum definitions passed in
        var options = ParseOptions(helpText, formatEnum, thresholdEnum);

        var command = new CliCommandDefinition
        {
            FullCommand = "hadolint",
            CommandParts = [],
            ClassName = "HadolintOptions",
            ParentClassName = BaseOptionsClassName,
            ToolNamespacePrefix = NamespacePrefix,
            Description = description,
            DocumentationUrl = "https://github.com/hadolint/hadolint",
            Options = options,
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Dockerfiles",
                    PlaceholderName = "DOCKERFILE...",
                    CSharpType = "IEnumerable<string>?",
                    IsRequired = false,
                    PositionIndex = 0,
                    Description = "Dockerfile(s) to lint (reads from stdin if not specified)"
                }
            ],
            SubDomainGroup = null,
            Enums = [formatEnum, thresholdEnum]
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    /// <summary>
    /// Parses options from Hadolint help text.
    /// Format: -h,--help                Show this help text
    ///         -v,--version             Show version
    ///         -c,--config FILENAME     Use FILENAME as configuration file
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(
        string helpText,
        CliEnumDefinition formatEnum,
        CliEnumDefinition thresholdEnum)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find "Available options:" section
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
            var match = HadolintOptionPattern().Match(line);
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
            CliEnumDefinition? enumDef = null;

            // Check for enum-type options
            if (propertyName == "Format")
            {
                csharpType = "HadolintFormat?";
                enumDef = formatEnum;
            }
            else if (propertyName == "FailureThreshold")
            {
                csharpType = "HadolintFailureThreshold?";
                enumDef = thresholdEnum;
            }

            // Check for array-type options
            var isArray = longForm == "--ignore" || longForm == "--trusted-registry";
            if (isArray)
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
                AcceptsMultipleValues = isArray,
                IsKeyValue = false,
                IsNumeric = false,
                ValueSeparator = " ",
                EnumDefinition = enumDef
            });
        }

        return options;
    }

    /// <summary>
    /// Checks if help text indicates the command has options.
    /// </summary>
    protected override bool HasOptions(string helpText)
    {
        return helpText.Contains("--") || helpText.Contains("Available options:");
    }

    #region Regex Patterns

    /// <summary>
    /// Matches "Available options:" section header.
    /// </summary>
    [GeneratedRegex(@"Available options:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex OptionsSectionPattern();

    /// <summary>
    /// Matches Hadolint-style option lines:
    ///   -h,--help                Show this help text
    ///   -c,--config FILENAME     Use FILENAME as configuration file
    ///   --no-fail                Don't exit with failure code
    /// </summary>
    [GeneratedRegex(@"^\s*(?:(?<short>-\w),)?(?<long>--[\w-]+)(?:\s+(?<value>[A-Z]+))?\s{2,}(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex HadolintOptionPattern();

    #endregion
}
