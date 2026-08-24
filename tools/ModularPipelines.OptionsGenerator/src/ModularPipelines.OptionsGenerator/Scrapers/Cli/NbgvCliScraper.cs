using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for the Nerdbank.GitVersioning <c>nbgv</c> .NET tool.
/// </summary>
public partial class NbgvCliScraper(
    ICliCommandExecutor executor,
    IHelpTextCache helpCache,
    ILogger<NbgvCliScraper> logger)
    : CliScraperBase(executor, helpCache, logger)
{
    private const string DocumentationUrl =
        "https://dotnet.github.io/Nerdbank.GitVersioning/docs/nbgv-cli.html";

    public override string ToolName => "nbgv";

    public override string NamespacePrefix => "Nbgv";

    public override string TargetNamespace => "ModularPipelines.NerdbankGitVersioning";

    public override string OutputDirectory => "src/ModularPipelines.NerdbankGitVersioning";

    protected override int MaxCommandDepth => 2;

    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var commandsSection = CommandsSectionPattern().Match(helpText);
        if (!commandsSection.Success)
        {
            return [];
        }

        var commands = helpText[(commandsSection.Index + commandsSection.Length)..];
        return
        [
            .. CommandLinePattern()
                .Matches(commands)
                .Select(match => match.Groups["name"].Value)
                .Distinct(StringComparer.OrdinalIgnoreCase),
        ];
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

        var command = new CliCommandDefinition
        {
            FullCommand = string.Join(' ', commandPath),
            CommandParts = commandParts,
            ClassName = GenerateClassName(commandPath),
            ParentClassName = BaseOptionsClassName,
            ToolNamespacePrefix = NamespacePrefix,
            Description = ExtractDescription(helpText),
            DocumentationUrl = DocumentationUrl,
            Options = ParseOptions(helpText),
            PositionalArguments = usage.PositionalArguments,
            UsageSynopsis = usage.Synopsis,
            HasOperandTakingUsage = usage.HasOperandTokens,
            Enums = [],
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    private static string? ExtractDescription(string helpText) =>
        DescriptionPattern().Match(helpText) is { Success: true } match
            ? match.Groups["description"].Value.Trim()
            : null;

    private static List<CliOptionDefinition> ParseOptions(string helpText)
    {
        var options = new List<CliOptionDefinition>();
        var seenSwitches = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var lines = helpText.ReplaceLineEndings("\n").Split('\n');

        for (var index = 0; index < lines.Length; index++)
        {
            var match = NbgvOptionLinePattern().Match(lines[index]);
            if (!match.Success)
            {
                continue;
            }

            var switchName = match.Groups["long"].Value;
            if (switchName == "--help" || !seenSwitches.Add(switchName))
            {
                continue;
            }

            var description = match.Groups["description"].Value.Trim();
            index = AccumulateDescription(lines, index, ref description);

            var valueHint = match.Groups["value"].Value;
            var explicitBoolean =
                switchName == "--public-release"
                || HelpDeclaresExplicitBooleanValue(description);
            var isFlag = string.IsNullOrEmpty(valueHint) && !explicitBoolean;
            var acceptsMultipleValues =
                switchName is "--define" or "--source"
                || HelpDeclaresRepeatableOption(helpText, switchName, description);
            var propertyName = NormalizeNbgvPropertyName(switchName);
            if (propertyName is null)
            {
                continue;
            }

            options.Add(new CliOptionDefinition
            {
                SwitchName = switchName,
                ShortForm = GetShortForm(match.Groups["aliases"].Value),
                PropertyName = propertyName,
                CSharpType = GetCSharpType(isFlag, explicitBoolean, acceptsMultipleValues),
                Description = description,
                IsFlag = isFlag,
                AcceptsMultipleValues = acceptsMultipleValues,
                ValueSeparator = explicitBoolean ? "=" : " ",
                IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag),
            });
        }

        return options;
    }

    private static int AccumulateDescription(
        string[] lines,
        int currentIndex,
        ref string description)
    {
        var descriptionParts = new List<string> { description };
        var nextIndex = currentIndex + 1;

        while (nextIndex < lines.Length
               && !string.IsNullOrWhiteSpace(lines[nextIndex])
               && !NbgvOptionLinePattern().IsMatch(lines[nextIndex]))
        {
            descriptionParts.Add(lines[nextIndex].Trim());
            nextIndex++;
        }

        description = string.Join(' ', descriptionParts.Where(part => part.Length > 0));
        return nextIndex - 1;
    }

    private static string? GetShortForm(string aliases) =>
        ShortAliasPattern().Matches(aliases)
            .Select(match => match.Value)
            .FirstOrDefault(alias => alias != "-?");

    private static string? NormalizeNbgvPropertyName(string switchName) =>
        switchName switch
        {
            "--nextVersion" => "NextVersion",
            "--versionIncrement" => "VersionIncrement",
            _ => NormalizePropertyName(switchName),
        };

    private static string GetCSharpType(
        bool isFlag,
        bool explicitBoolean,
        bool acceptsMultipleValues)
    {
        if (isFlag || explicitBoolean)
        {
            return "bool?";
        }

        return acceptsMultipleValues ? "IEnumerable<string>?" : "string?";
    }

    [GeneratedRegex(@"(?im)^\s*Commands:\s*$")]
    private static partial Regex CommandsSectionPattern();

    [GeneratedRegex(@"(?m)^\s{2,}(?<name>[a-z][\w-]*)(?:\s|$)")]
    private static partial Regex CommandLinePattern();

    [GeneratedRegex(@"(?im)^\s*Description:\s*\n\s*(?<description>[^\r\n]+)")]
    private static partial Regex DescriptionPattern();

    [GeneratedRegex(
        @"^\s*(?<aliases>(?:-[^,\s]+,\s*)*)(?<long>--[\w?-]+)(?:\s+<(?<value>[^>]+)>)?\s{2,}(?<description>.*)$")]
    private static partial Regex NbgvOptionLinePattern();

    [GeneratedRegex(@"-[A-Za-z0-9?](?=,|$)")]
    private static partial Regex ShortAliasPattern();
}
