using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Generators;
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
    private static readonly HashSet<string> RepeatableOptions = new(StringComparer.Ordinal)
    {
        "--enable",
        "--exclude",
        "--include",
        "--source-path",
    };

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
        var enums = new Dictionary<string, CliEnumDefinition>(StringComparer.OrdinalIgnoreCase)
        {
            ["--color"] = CreateEnum("ShellcheckColor", "Color output mode", "auto", "always", "never"),
            ["--format"] = CreateEnum("ShellcheckFormat", "Output format for ShellCheck results", "checkstyle", "diff", "gcc", "json", "json1", "quiet", "tty"),
            ["--shell"] = CreateEnum("ShellcheckShell", "Shell dialect to check against", "sh", "bash", "dash", "ksh", "busybox"),
            ["--severity"] = CreateEnum("ShellcheckSeverity", "Minimum severity level to report", "error", "warning", "info", "style")
        };

        var options = ParseOptions(helpText, enums);

        var command = new CliCommandDefinition
        {
            FullCommand = "shellcheck",
            CommandParts = [],
            ClassName = "ShellcheckOptions",
            ParentClassName = BaseOptionsClassName,
            ToolNamespacePrefix = NamespacePrefix,
            Description = "ShellCheck - Shell script static analysis tool",
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
                    Description = "Shell script files to check",
                    Placement = PositionalArgumentPosition.AfterOptions
                }
            ],
            SubDomainGroup = null,
            Enums = enums.Values.ToList()
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    /// <summary>
    /// Parses options from ShellCheck help text.
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(
        string helpText,
        IReadOnlyDictionary<string, CliEnumDefinition> enums)
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
            var valueHint = match.Groups["longValue"].Value.Trim();
            if (string.IsNullOrEmpty(valueHint))
            {
                valueHint = match.Groups["longOptionalValue"].Value.Trim();
            }

            var description = match.Groups["desc"].Value.Trim();

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
            enums.TryGetValue(longForm, out var enumDefinition);
            var isBoolean = !isFlag && valueHint.Equals("bool", StringComparison.OrdinalIgnoreCase);
            var isNumeric = !isFlag && valueHint.Equals("NUM", StringComparison.OrdinalIgnoreCase);
            var acceptsMultipleValues = RepeatableOptions.Contains(longForm);
            var csharpType = DetermineCSharpType(
                isFlag,
                isBoolean,
                isNumeric,
                acceptsMultipleValues,
                enumDefinition);

            options.Add(new CliOptionDefinition
            {
                SwitchName = longForm,
                ShortForm = string.IsNullOrEmpty(shortForm) ? null : shortForm,
                PropertyName = propertyName,
                CSharpType = csharpType,
                Description = description,
                IsFlag = isFlag,
                IsRequired = false,
                AcceptsMultipleValues = acceptsMultipleValues,
                IsKeyValue = false,
                IsNumeric = isNumeric,
                ValueSeparator = "=",
                EnumDefinition = enumDefinition,
                IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag)
            });
        }

        return options;
    }

    private static string DetermineCSharpType(
        bool isFlag,
        bool isBoolean,
        bool isNumeric,
        bool acceptsMultipleValues,
        CliEnumDefinition? enumDefinition)
    {
        if (acceptsMultipleValues)
        {
            return "IEnumerable<string>?";
        }

        if (enumDefinition is not null)
        {
            return $"{enumDefinition.EnumName}?";
        }

        if (isFlag || isBoolean)
        {
            return "bool?";
        }

        return isNumeric ? "int?" : "string?";
    }

    private static CliEnumDefinition CreateEnum(string name, string description, params string[] values)
    {
        return new CliEnumDefinition
        {
            EnumName = name,
            Description = description,
            Values = values.Select(value => new CliEnumValue
            {
                MemberName = ToPascalCase(value),
                CliValue = value
            }).ToList()
        };
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
    /// Matches ShellCheck-style option lines with optional short form:
    ///   -a                   --check-sourced      Check sourced files
    ///   -C[WHEN]             --color[=WHEN]       Enable color output
    ///                        --wiki-link-count=NUM  Count of wiki links (no short form)
    /// </summary>
    [GeneratedRegex(@"^\s*(?:(?<short>-[A-Za-z])(?:(?:\[[^\]]+\])|(?:\s+(?!--)\S+))?\s+)?(?<long>--[\w-]+)(?:(?:\[=(?<longOptionalValue>[^\]]+)\])|(?:=(?<longValue>\S+)))?\s{2,}(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex ShellcheckOptionPattern();

    #endregion
}
