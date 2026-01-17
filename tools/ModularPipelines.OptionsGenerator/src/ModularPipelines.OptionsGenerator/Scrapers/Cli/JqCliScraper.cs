using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for jq - lightweight JSON processor.
/// jq uses a simple help format.
///
/// jq help format (jq --help):
/// jq - commandline JSON processor [version 1.6]
///
/// Usage: jq [options...] &lt;jq filter&gt; [file...]
///        jq [options...] --from-file &lt;jq filter file&gt; [file...]
///
///     --tab               use tabs for indentation;
///     --arg a v           set variable $a to value &lt;v&gt;;
///     ...
/// </summary>
public partial class JqCliScraper : CliScraperBase
{
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

        var command = new CliCommandDefinition
        {
            FullCommand = "jq",
            CommandParts = [],
            ClassName = "JqOptions",
            ParentClassName = BaseOptionsClassName,
            ToolNamespacePrefix = NamespacePrefix,
            Description = description,
            DocumentationUrl = "https://stedolan.github.io/jq/manual/",
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
                    Description = "The jq filter expression to apply"
                },
                new CliPositionalArgument
                {
                    PropertyName = "InputFiles",
                    PlaceholderName = "file...",
                    CSharpType = "IEnumerable<string>?",
                    IsRequired = false,
                    PositionIndex = 1,
                    Description = "Input JSON files (reads from stdin if not specified)"
                }
            ],
            SubDomainGroup = null,
            Enums = []
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    /// <summary>
    /// Parses options from jq help text.
    /// Format:     --tab               use tabs for indentation;
    ///             --arg a v           set variable $a to value &lt;v&gt;;
    ///             -r                  output raw strings, not JSON texts;
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(string helpText)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var lines = helpText.Split('\n');

        foreach (var line in lines)
        {
            // Try long form first
            var match = JqLongOptionPattern().Match(line);
            if (match.Success)
            {
                var longForm = match.Groups["long"].Value.Trim();
                var description = match.Groups["desc"].Value.Trim().TrimEnd(';');

                if (string.IsNullOrEmpty(longForm) || seenOptions.Contains(longForm))
                {
                    continue;
                }

                seenOptions.Add(longForm);

                var propertyName = NormalizePropertyName(longForm);
                if (propertyName is null)
                {
                    continue;
                }

                var isFlag = !line.Contains(" a ") && !line.Contains(" name ") && !line.Contains(" v ");
                var csharpType = isFlag ? "bool?" : "string?";

                options.Add(new CliOptionDefinition
                {
                    SwitchName = longForm,
                    ShortForm = null,
                    PropertyName = propertyName,
                    CSharpType = csharpType,
                    Description = description,
                    IsFlag = isFlag,
                    IsRequired = false,
                    AcceptsMultipleValues = false,
                    IsKeyValue = longForm == "--arg" || longForm == "--argjson",
                    IsNumeric = false,
                    ValueSeparator = " ",
                    EnumDefinition = null
                });
            }
            else
            {
                // Try short form
                match = JqShortOptionPattern().Match(line);
                if (match.Success)
                {
                    var shortForm = match.Groups["short"].Value.Trim();
                    var description = match.Groups["desc"].Value.Trim().TrimEnd(';');

                    if (string.IsNullOrEmpty(shortForm) || seenOptions.Contains(shortForm))
                    {
                        continue;
                    }

                    seenOptions.Add(shortForm);

                    // Map short forms to meaningful property names
                    var propertyName = MapShortFormToPropertyName(shortForm);
                    if (propertyName is null)
                    {
                        continue;
                    }

                    var isFlag = true;
                    var csharpType = "bool?";

                    options.Add(new CliOptionDefinition
                    {
                        SwitchName = shortForm,
                        ShortForm = shortForm,
                        PropertyName = propertyName,
                        CSharpType = csharpType,
                        Description = description,
                        IsFlag = isFlag,
                        IsRequired = false,
                        AcceptsMultipleValues = false,
                        IsKeyValue = false,
                        IsNumeric = false,
                        ValueSeparator = " ",
                        EnumDefinition = null
                    });
                }
            }
        }

        return options;
    }

    /// <summary>
    /// Maps jq short form options to property names.
    /// </summary>
    private static string? MapShortFormToPropertyName(string shortForm)
    {
        return shortForm switch
        {
            "-c" => "Compact",
            "-r" => "RawOutput",
            "-R" => "RawInput",
            "-s" => "Slurp",
            "-S" => "SortKeys",
            "-C" => "ColorOutput",
            "-M" => "MonochromeOutput",
            "-a" => "AsciiOutput",
            "-e" => "ExitStatus",
            "-n" => "NullInput",
            "-j" => "JoinOutput",
            _ => null
        };
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
    /// Matches jq-style long option lines:
    ///     --tab               use tabs for indentation;
    /// </summary>
    [GeneratedRegex(@"^\s+(?<long>--[\w-]+)(?:\s+\w+)*\s{2,}(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex JqLongOptionPattern();

    /// <summary>
    /// Matches jq-style short option lines:
    ///     -r                  output raw strings, not JSON texts;
    /// </summary>
    [GeneratedRegex(@"^\s+(?<short>-\w)\s{2,}(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex JqShortOptionPattern();

    #endregion
}
