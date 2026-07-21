using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for jq - lightweight JSON processor.
/// jq uses a simple help format.
///
/// jq help format (jq --help):
/// jq - commandline JSON processor [version 1.8.2]
///
/// Usage: jq [options...] &lt;jq filter&gt; [file...]
///        jq [options...] --from-file &lt;jq filter file&gt; [file...]
///
///   -n, --null-input          use `null` as the single input value;
///       --arg name value      set $name to the string value;
///     ...
/// </summary>
public partial class JqCliScraper : CliScraperBase
{
    private static readonly IReadOnlyDictionary<string, int> OptionOperandCounts =
        new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            ["--arg"] = 2,
            ["--argjson"] = 2,
            ["--from-file"] = 1,
            ["--indent"] = 1,
            ["--library-path"] = 1,
            ["--rawfile"] = 2,
            ["--run-tests"] = 1,
            ["--slurpfile"] = 2
        };

    private static readonly IReadOnlyDictionary<string, string> DisplayedOperands =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["--arg"] = "name value",
            ["--argjson"] = "name value",
            ["--indent"] = "n",
            ["--library-path"] = "dir",
            ["--rawfile"] = "name file",
            ["--run-tests"] = "[filename]",
            ["--slurpfile"] = "name file"
        };

    public JqCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<JqCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "jq";

    public override string NamespacePrefix => "Jq";

    public override string TargetNamespace => "ModularPipelines.Jq";

    public override string OutputDirectory => "src/ModularPipelines.Jq";

    /// <summary>
    /// jq uses -h instead of --help for help text.
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

        // jq uses -h instead of --help
        var result = await Executor.ExecuteAsync(ToolName, "-h", cancellationToken);

        // jq outputs help to stderr
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
    /// jq doesn't have subcommands.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        return [];
    }

    /// <summary>
    /// Parses jq command options from help text.
    /// </summary>
    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken)
    {
        var description = "jq - lightweight and flexible command-line JSON processor";

        var options = ParseOptions(helpText);
        AddDocumentedOptions(options);

        var command = new CliCommandDefinition
        {
            FullCommand = "jq",
            CommandParts = [],
            ClassName = "JqOptions",
            ParentClassName = BaseOptionsClassName,
            ToolNamespacePrefix = NamespacePrefix,
            Description = description,
            DocumentationUrl = "https://jqlang.org/manual/",
            Options = options,
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Filter",
                    PlaceholderName = "jq filter",
                    CSharpType = "string?",
                    IsRequired = false,
                    PositionIndex = 0,
                    Description = "The jq filter expression to apply",
                    Placement = PositionalArgumentPosition.AfterOptions
                },
                new CliPositionalArgument
                {
                    PropertyName = "InputFiles",
                    PlaceholderName = "file...",
                    CSharpType = "IEnumerable<string>?",
                    IsRequired = false,
                    PositionIndex = 1,
                    Description = "Input JSON files or values (reads from stdin if not specified)",
                    Placement = PositionalArgumentPosition.AfterOptions
                }
            ],
            SubDomainGroup = null,
            Enums = []
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    /// <summary>
    /// Parses options from jq help text.
    /// Format:   -n, --null-input          use `null` as the single input value;
    ///               --arg name value      set $name to the string value;
    /// </summary>
    protected List<CliOptionDefinition> ParseOptions(string helpText)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var lines = helpText.Split('\n');

        for (var i = 0; i < lines.Length; i++)
        {
            var match = JqOptionPattern().Match(lines[i]);
            if (!match.Success)
            {
                continue;
            }

            var longForm = match.Groups["long"].Value.Trim();
            if (!seenOptions.Add(longForm))
            {
                continue;
            }

            var shortForm = match.Groups["short"].Value.Trim();
            var operandCount = OptionOperandCounts.GetValueOrDefault(longForm);
            var description = RemoveDisplayedOperands(longForm, match.Groups["tail"].Value);
            i = AccumulateMultiLineDescription(lines, i, ref description);

            var propertyName = NormalizeJqPropertyName(longForm);
            if (propertyName is null)
            {
                continue;
            }

            var isFlag = operandCount == 0;
            var isPair = operandCount == 2;
            var isNumeric = longForm == "--indent";
            var acceptsMultipleValues = isPair || longForm == "--library-path";

            options.Add(new CliOptionDefinition
            {
                SwitchName = longForm,
                ShortForm = string.IsNullOrEmpty(shortForm) ? null : shortForm,
                PreferShortForm = longForm == "--library-path",
                PropertyName = propertyName,
                CSharpType = DetermineCSharpType(isFlag, isPair, isNumeric, acceptsMultipleValues),
                Description = description.TrimEnd(';'),
                IsFlag = isFlag,
                IsRequired = false,
                AcceptsMultipleValues = acceptsMultipleValues,
                IsKeyValue = false,
                IsNumeric = isNumeric,
                ValueSeparator = " ",
                EnumDefinition = null,
                IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag)
            });
        }

        return options;
    }

    private static void AddDocumentedOptions(List<CliOptionDefinition> options)
    {
        if (options.All(x => x.SwitchName != "--run-tests"))
        {
            options.Add(new CliOptionDefinition
            {
                SwitchName = "--run-tests",
                PropertyName = "RunTests",
                CSharpType = "string?",
                Description = "Run jq tests from the specified file",
                IsFlag = false
            });
        }

        // jq -h omits the POSIX end-of-options marker documented by its manual.
        options.Add(new CliOptionDefinition
        {
            SwitchName = "--",
            PropertyName = "EndOfOptions",
            CSharpType = "bool?",
            Description = "Stop processing options so filters beginning with a dash are treated as positional arguments",
            IsFlag = true
        });
    }

    private static string RemoveDisplayedOperands(string longForm, string optionTail)
    {
        var trimmedTail = optionTail.TrimStart();
        if (!DisplayedOperands.TryGetValue(longForm, out var displayedOperands) ||
            !trimmedTail.StartsWith(displayedOperands, StringComparison.OrdinalIgnoreCase))
        {
            return trimmedTail.Trim();
        }

        return trimmedTail[displayedOperands.Length..].Trim();
    }

    private static string? NormalizeJqPropertyName(string longForm)
    {
        return longForm switch
        {
            "--argjson" => "ArgJson",
            "--jsonargs" => "JsonArgs",
            "--rawfile" => "RawFile",
            "--slurpfile" => "SlurpFile",
            _ => NormalizePropertyName(longForm)
        };
    }

    private static string DetermineCSharpType(
        bool isFlag,
        bool isPair,
        bool isNumeric,
        bool acceptsMultipleValues)
    {
        if (isFlag)
        {
            return "bool?";
        }

        if (isPair)
        {
            return "IEnumerable<CliOptionValuePair>?";
        }

        if (isNumeric)
        {
            return "int?";
        }

        return acceptsMultipleValues ? "IEnumerable<string>?" : "string?";
    }

    private static int AccumulateMultiLineDescription(string[] lines, int currentIndex, ref string description)
    {
        var descriptionParts = new List<string> { description };
        var nextIndex = currentIndex + 1;

        while (nextIndex < lines.Length)
        {
            var nextLine = lines[nextIndex];
            if (string.IsNullOrWhiteSpace(nextLine) || JqOptionPattern().IsMatch(nextLine))
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

    /// <summary>
    /// Checks if help text indicates the command has options.
    /// </summary>
    protected override bool HasOptions(string helpText)
    {
        return helpText.Contains("--") || helpText.Contains("Usage:");
    }

    #region Regex Patterns

    /// <summary>
    /// Matches jq 1.8 option lines with optional short aliases and operands.
    /// </summary>
    [GeneratedRegex(@"^\s+(?:(?<short>-[A-Za-z]),\s*)?(?<long>--[\w-]+)(?<tail>.*)$", RegexOptions.Multiline)]
    private static partial Regex JqOptionPattern();

    #endregion
}
