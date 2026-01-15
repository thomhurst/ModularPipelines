using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for ShellCheck - shell script static analysis tool.
/// ShellCheck uses a standard help format.
///
/// shellcheck help format (shellcheck --help):
/// Usage: shellcheck [OPTIONS...] FILES...
///   -a                   --check-sourced      Check sourced files
///   -C[WHEN]             --color[=WHEN]       Enable color output
///   -i CODE1,CODE2..     --include=CODE1,CODE2..  Include only specified warnings
///   ...
/// </summary>
public partial class ShellcheckCliScraper : CliScraperBase
{
    public ShellcheckCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<ShellcheckCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "shellcheck";

    public override string NamespacePrefix => "Shellcheck";

    public override string TargetNamespace => "ModularPipelines.Shellcheck";

    public override string OutputDirectory => "src/ModularPipelines.Shellcheck";

    /// <summary>
    /// ShellCheck doesn't have subcommands.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        return [];
    }

    /// <summary>
    /// Parses ShellCheck command options from help text.
    /// </summary>
    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken)
    {
        var description = "ShellCheck - Shell script static analysis tool";

        // Add enum for format option
        var formatEnum = new CliEnumDefinition
        {
            EnumName = "ShellcheckFormat",
            Values =
            [
                new CliEnumValue { MemberName = "Tty", CliValue = "tty" },
                new CliEnumValue { MemberName = "Gcc", CliValue = "gcc" },
                new CliEnumValue { MemberName = "Checkstyle", CliValue = "checkstyle" },
                new CliEnumValue { MemberName = "Diff", CliValue = "diff" },
                new CliEnumValue { MemberName = "Json", CliValue = "json" },
                new CliEnumValue { MemberName = "Json1", CliValue = "json1" },
                new CliEnumValue { MemberName = "Quiet", CliValue = "quiet" }
            ],
            Description = "Output format for ShellCheck results"
        };

        // Add enum for shell option
        var shellEnum = new CliEnumDefinition
        {
            EnumName = "ShellcheckShell",
            Values =
            [
                new CliEnumValue { MemberName = "Sh", CliValue = "sh" },
                new CliEnumValue { MemberName = "Bash", CliValue = "bash" },
                new CliEnumValue { MemberName = "Dash", CliValue = "dash" },
                new CliEnumValue { MemberName = "Ksh", CliValue = "ksh" }
            ],
            Description = "Shell dialect to check against"
        };

        // Add enum for severity option
        var severityEnum = new CliEnumDefinition
        {
            EnumName = "ShellcheckSeverity",
            Values =
            [
                new CliEnumValue { MemberName = "Error", CliValue = "error" },
                new CliEnumValue { MemberName = "Warning", CliValue = "warning" },
                new CliEnumValue { MemberName = "Info", CliValue = "info" },
                new CliEnumValue { MemberName = "Style", CliValue = "style" }
            ],
            Description = "Minimum severity level to report"
        };

        var options = ParseOptions(helpText, formatEnum, shellEnum, severityEnum);

        var command = new CliCommandDefinition
        {
            FullCommand = "shellcheck",
            CommandParts = [],
            ClassName = "ShellcheckOptions",
            ParentClassName = BaseOptionsClassName,
            ToolNamespacePrefix = NamespacePrefix,
            Description = description,
            DocumentationUrl = "https://github.com/koalaman/shellcheck",
            Options = options,
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Files",
                    PlaceholderName = "FILES...",
                    CSharpType = "IEnumerable<string>?",
                    IsRequired = false,
                    PositionIndex = 0,
                    Description = "Shell script files to check"
                }
            ],
            SubDomainGroup = null,
            Enums = [formatEnum, shellEnum, severityEnum]
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    /// <summary>
    /// Parses options from ShellCheck help text.
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(
        string helpText,
        CliEnumDefinition formatEnum,
        CliEnumDefinition shellEnum,
        CliEnumDefinition severityEnum)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var lines = helpText.Split('\n');

        foreach (var line in lines)
        {
            var match = ShellcheckOptionPattern().Match(line);
            if (!match.Success)
            {
                continue;
            }

            var shortForm = match.Groups["short"].Value.Trim();
            var longForm = match.Groups["long"].Value.Trim();
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

            var isFlag = !longForm.Contains('=') && !shortForm.Contains('[');
            var csharpType = isFlag ? "bool?" : "string?";
            CliEnumDefinition? enumDef = null;

            // Check for enum-type options
            if (propertyName == "Format")
            {
                csharpType = "ShellcheckFormat?";
                enumDef = formatEnum;
            }
            else if (propertyName == "Shell")
            {
                csharpType = "ShellcheckShell?";
                enumDef = shellEnum;
            }
            else if (propertyName == "Severity")
            {
                csharpType = "ShellcheckSeverity?";
                enumDef = severityEnum;
            }

            // Handle array-type options
            if (longForm.Contains("--include") || longForm.Contains("--exclude"))
            {
                csharpType = "IEnumerable<string>?";
            }

            options.Add(new CliOptionDefinition
            {
                SwitchName = longForm.Split('=')[0],
                ShortForm = string.IsNullOrEmpty(shortForm) ? null : shortForm.Split('[')[0],
                PropertyName = propertyName,
                CSharpType = csharpType,
                Description = description,
                IsFlag = isFlag,
                IsRequired = false,
                AcceptsMultipleValues = csharpType.Contains("IEnumerable"),
                IsKeyValue = false,
                IsNumeric = false,
                ValueSeparator = "=",
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
        return helpText.Contains("--");
    }

    #region Regex Patterns

    /// <summary>
    /// Matches ShellCheck-style option lines:
    ///   -a                   --check-sourced      Check sourced files
    ///   -C[WHEN]             --color[=WHEN]       Enable color output
    /// </summary>
    [GeneratedRegex(@"^\s+(?<short>-\w(?:\[[^\]]+\])?)\s+(?<long>--[\w-]+(?:=\w+|\[=\w+\])?)\s+(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex ShellcheckOptionPattern();

    #endregion
}
