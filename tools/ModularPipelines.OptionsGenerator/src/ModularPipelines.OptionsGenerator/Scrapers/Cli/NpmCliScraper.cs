using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for the npm package manager.
/// </summary>
public partial class NpmCliScraper(
    ICliCommandExecutor executor,
    IHelpTextCache helpCache,
    ILogger<NpmCliScraper> logger)
    : CliScraperBase(executor, helpCache, logger)
{
    public override string ToolName => "npm";

    public override string NamespacePrefix => "Npm";

    /// <inheritdoc />
    public override bool IncludeInGenerationMatrix => false;

    public override string TargetNamespace => "ModularPipelines.Node";

    public override string OutputDirectory => "src/ModularPipelines.Node";

    protected override IReadOnlySet<string> AdditionalSkipSubcommands =>
        new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "help-search",
            "ll",
            "root",
        };

    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var match = AllCommandsSectionPattern().Match(helpText);
        if (!match.Success)
        {
            return [];
        }

        return CommandNamePattern()
            .Matches(match.Groups["commands"].Value)
            .Select(command => command.Value)
            .Distinct(StringComparer.OrdinalIgnoreCase);
    }

    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken) =>
        throw new InvalidOperationException("Shared traversal must pass its parsed synopsis.");

    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        UsageSynopsisParseResult usage,
        CancellationToken cancellationToken)
    {
        var commandParts = commandPath.Skip(1).ToArray();
        if (commandParts.Length == 0)
        {
            return Task.FromResult<CliCommandDefinition?>(null);
        }

        var options = ParseOptions(helpText);
        return Task.FromResult<CliCommandDefinition?>(new CliCommandDefinition
        {
            FullCommand = string.Join(' ', commandPath),
            CommandParts = commandParts,
            ClassName = GenerateClassName(commandPath),
            ParentClassName = BaseOptionsClassName,
            ToolNamespacePrefix = NamespacePrefix,
            Description = ExtractDescription(helpText),
            DocumentationUrl = $"https://docs.npmjs.com/cli/commands/npm-{commandParts[0]}",
            Options = options,
            PositionalArguments = NormalizePositionalArguments(
                commandParts,
                usage.PositionalArguments),
            SubDomainGroup = null,
            Enums = [],
        });
    }

    private static IReadOnlyList<CliPositionalArgument> NormalizePositionalArguments(
        string[] commandParts,
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        if (commandParts is not ["init"])
        {
            return positionalArguments;
        }

        return
        [
            new CliPositionalArgument
            {
                PropertyName = "Value",
                CSharpType = "string?",
                Placement = PositionalArgumentPosition.BeforeOptions,
                PositionIndex = 0,
                IsRequired = false,
            },
        ];
    }

    private static string? ExtractDescription(string helpText) =>
        helpText
            .ReplaceLineEndings("\n")
            .Split('\n')
            .Select(line => line.Trim())
            .FirstOrDefault(line =>
                !string.IsNullOrWhiteSpace(line)
                && !line.StartsWith("Usage:", StringComparison.OrdinalIgnoreCase)
                && !line.StartsWith("npm ", StringComparison.OrdinalIgnoreCase));

    private static List<CliOptionDefinition> ParseOptions(string helpText)
    {
        var normalizedHelp = helpText.ReplaceLineEndings("\n");
        var lines = normalizedHelp.Split('\n');
        var options = new List<CliOptionDefinition>();
        var seenSwitches = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        for (var index = 0; index < lines.Length; index++)
        {
            var declaration = DetailedOptionPattern().Match(lines[index]);
            if (!declaration.Success)
            {
                continue;
            }

            var switchName = declaration.Groups["long"].Value;
            if (!seenSwitches.Add(switchName))
            {
                continue;
            }

            var propertyName = NormalizePropertyName(switchName);
            if (propertyName is null)
            {
                continue;
            }

            var description = ReadDescription(lines, index + 1);
            var takesValue = Regex.IsMatch(
                normalizedHelp,
                $@"{Regex.Escape(switchName)}\s+<[^>]+>",
                RegexOptions.IgnoreCase);
            var acceptsMultipleValues = takesValue
                && HelpDeclaresRepeatableOption(normalizedHelp, switchName, description);
            var isFlag = !takesValue;

            options.Add(new CliOptionDefinition
            {
                SwitchName = switchName,
                ShortForm = declaration.Groups["short"].Success
                    ? declaration.Groups["short"].Value
                    : null,
                PropertyName = propertyName,
                CSharpType = acceptsMultipleValues ? "IEnumerable<string>?" : isFlag ? "bool?" : "string?",
                Description = description,
                IsFlag = isFlag,
                IsRequired = false,
                AcceptsMultipleValues = acceptsMultipleValues,
                IsKeyValue = false,
                IsNumeric = false,
                ValueSeparator = " ",
                EnumDefinition = null,
                IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag),
            });
        }

        return options;
    }

    private static string ReadDescription(string[] lines, int startIndex)
    {
        var description = new List<string>();

        for (var index = startIndex; index < lines.Length; index++)
        {
            var line = lines[index];
            if (DetailedOptionPattern().IsMatch(line))
            {
                break;
            }

            var trimmed = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
            {
                if (description.Count > 0)
                {
                    break;
                }

                continue;
            }

            description.Add(trimmed);
        }

        return string.Join(' ', description);
    }

    [GeneratedRegex(
        @"All commands:\s*(?<commands>.*?)(?:\r?\n\s*\r?\n|\z)",
        RegexOptions.IgnoreCase | RegexOptions.Singleline)]
    private static partial Regex AllCommandsSectionPattern();

    [GeneratedRegex(@"[a-z][a-z0-9-]*", RegexOptions.IgnoreCase)]
    private static partial Regex CommandNamePattern();

    [GeneratedRegex(
        @"^\s{2}(?:(?<short>-[A-Za-z0-9])\|)?(?<long>--[A-Za-z0-9][A-Za-z0-9-]*)\s*$")]
    private static partial Regex DetailedOptionPattern();
}
