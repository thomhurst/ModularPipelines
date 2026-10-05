using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
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
    public override bool IncludeInGenerationMatrix => true;

    public override string TargetNamespace => "ModularPipelines.Node";

    public override string OutputDirectory => "src/ModularPipelines.Node";

    public override CliToolDefinition CreateToolDefinition() => base.CreateToolDefinition() with
    {
        CommandCoverage = new CliCommandCoveragePolicy
        {
            MinimumCommandCount = 50,
            SentinelCommands = ["npm install", "npm exec", "npm access list packages", "npm token revoke", "npm version", "npm root", "npm org set", "npm org rm", "npm org ls"],
        },
    };

    // Each npm help process starts Node.js; keep the traversal's memory bounded.
    protected override int MaxParallelism => 2;

    protected override IReadOnlySet<string> AdditionalSkipSubcommands =>
        new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ll",
        };

    /// <inheritdoc />
    protected override bool IsSkippableSubcommand(string subcommand) =>
        // npm version edits package versions; it is not merely a diagnostic command.
        !subcommand.Equals("version", StringComparison.OrdinalIgnoreCase) && base.IsSkippableSubcommand(subcommand);

    /// <inheritdoc />
    protected override IEnumerable<string> ExtractSubcommands(string[] commandPath, string helpText)
    {
        if (commandPath.Length == 1)
        {
            return ExtractSubcommands(helpText);
        }

        return GetUsageTails(commandPath, helpText)
            .Select(tail => ChildCommandPattern().Match(tail))
            .Where(match => match.Success)
            .Select(match => match.Groups["command"].Value)
            .Distinct(StringComparer.Ordinal);
    }

    /// <inheritdoc />
    protected override UsageSynopsisParseResult ParseUsageSynopsis(string[] commandPath, string helpText) =>
        base.ParseUsageSynopsis(commandPath, NormalizeUsageHelp(helpText));

    private static string NormalizeUsageHelp(string helpText)
    {
        var normalized = string.Join('\n', helpText.ReplaceLineEndings("\n").Split('\n').Select(line =>
        {
            var usage = line.Trim();
            if (!usage.StartsWith("npm ", StringComparison.Ordinal))
            {
                return line;
            }

            usage = SynopsisExplanationPattern().Replace(usage, string.Empty);
            // npm org's help omits angle brackets around these parameter names.
            return usage.StartsWith("npm org ", StringComparison.Ordinal)
                ? BareOrgOperandPattern().Replace(usage, "<${name}>")
                : usage;
        }));

        // Audit's choices name verbs; choices on commands such as profile enable-2fa name values.
        return AuditSynopsisPattern().Replace(normalized, match => match.Value + "\n"
            + string.Join('\n', match.Groups["commands"].Value.Split('|').Select(command => "npm audit " + command)));
    }

    private static IEnumerable<string> GetUsageTails(string[] commandPath, string helpText)
    {
        var prefix = string.Join(' ', commandPath);
        return NormalizeUsageHelp(helpText).ReplaceLineEndings("\n").Split('\n')
            .Select(line => line.Trim())
            .Where(line => line.Equals(prefix, StringComparison.Ordinal)
                || line.StartsWith(prefix + " ", StringComparison.Ordinal))
            .Select(line => line[prefix.Length..].TrimStart());
    }

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
        if (commandParts.Length == 0 || (ExtractSubcommands(commandPath, helpText).Any()
            && !GetUsageTails(commandPath, helpText).Any(tail => tail.Length == 0 || tail[0] is '<' or '[' or '-')))
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
            SubDomainGroup = commandParts.Length > 1 ? ToPascalCase(commandParts[0]) : null,
            Enums = [],
        });
    }

    private static IReadOnlyList<CliPositionalArgument> NormalizePositionalArguments(
        string[] commandParts,
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        if (commandParts is not ["init"])
        {
            return [.. positionalArguments.Select(argument => NormalizeOperand(commandParts, argument))];
        }

        return
        [
            new CliPositionalArgument
            {
                PropertyName = "Value",
                CSharpType = "string?",
                Phase = CommandLinePhase.EarlyOperand,
                PositionIndex = 0,
                IsRequired = false,
            },
        ];
    }

    private static CliPositionalArgument NormalizeOperand(string[] commandParts, CliPositionalArgument argument)
    {
        // npm 11's terse help omits defaults and repetition supported by its command implementations.
        if (commandParts is ["ls" or "pack" or "publish"] && argument.PropertyName == "PackageSpec")
        {
            var variadic = commandParts[0] != "publish";
            return argument with
            {
                IsRequired = false,
                IsVariadic = variadic,
                CSharpType = variadic ? "IEnumerable<string>?" : "string?",
            };
        }

        if (commandParts is ["run" or "start" or "stop" or "test" or "restart"]
            && argument.Phase == CommandLinePhase.Passthrough)
        {
            return argument with { IsVariadic = true, CSharpType = "IEnumerable<string>?", IsRequired = false };
        }

        // With no event, npm run lists the package's scripts.
        return commandParts is ["run"] && argument.PropertyName == "Command"
            ? argument with { IsRequired = false, CSharpType = "string?" }
            : argument;
    }

    internal static string? ExtractDescription(string helpText) =>
        helpText
            .ReplaceLineEndings("\n")
            .Split('\n')
            .Select(line => line.Trim())
            .FirstOrDefault(line =>
                !string.IsNullOrWhiteSpace(line)
                && !line.StartsWith("Usage:", StringComparison.OrdinalIgnoreCase)
                && !line.StartsWith("npm ", StringComparison.OrdinalIgnoreCase));

    internal static List<CliOptionDefinition> ParseOptions(string helpText)
    {
        var normalizedHelp = helpText.ReplaceLineEndings("\n");
        var lines = normalizedHelp.Split('\n');
        var options = new List<CliOptionDefinition>();
        var declarations = new Dictionary<string, (Match Match, string Description)>(StringComparer.Ordinal);
        for (var index = 0; index < lines.Length; index++)
        {
            var declaration = DetailedOptionPattern().Match(lines[index]);
            if (declaration.Success)
            {
                declarations.TryAdd(declaration.Groups["long"].Value, (declaration, ReadDescription(lines, index + 1)));
            }
        }

        var synopsis = string.Join(' ', lines.SkipWhile(line => line.Trim() != "Options:")
            .Skip(1).SkipWhile(string.IsNullOrWhiteSpace).TakeWhile(line => !string.IsNullOrWhiteSpace(line)));
        foreach (Match declaration in SynopsisOptionPattern().Matches(synopsis))
        {
            declarations.TryAdd(declaration.Groups["long"].Value, (declaration, string.Empty));
        }

        foreach (var (switchName, (declaration, description)) in declarations)
        {
            if (switchName.StartsWith("--no-", StringComparison.Ordinal)
                && declarations.ContainsKey("--" + switchName[5..]))
            {
                continue;
            }

            var propertyName = NormalizePropertyName(switchName);
            if (propertyName is null)
            {
                continue;
            }

            options.Add(CreateOption(switchName, propertyName, declaration, description, normalizedHelp, synopsis, declarations.Keys));
        }

        return options;
    }

    private static CliOptionDefinition CreateOption(
        string switchName,
        string propertyName,
        Match declaration,
        string description,
        string normalizedHelp,
        string synopsis,
        ICollection<string> switches)
    {
        var takesValue = Regex.IsMatch(
            normalizedHelp,
            $@"{Regex.Escape(switchName)}\s+<[^>]+>",
            RegexOptions.IgnoreCase)
            || Regex.IsMatch(synopsis, $@"{Regex.Escape(switchName)}\s+[a-zA-Z][\w-]*(?=[\s\]\|]|$)");
        var acceptsMultipleValues = takesValue
            && (HelpDeclaresRepeatableOption(normalizedHelp, switchName, description)
                || Regex.IsMatch(synopsis, $@"{Regex.Escape(switchName)}\s+<[^>]+>\s+\.{{3}}"));
        var isFlag = !takesValue;

        return new CliOptionDefinition
        {
            SwitchName = switchName,
            NegatedSwitchName = isFlag && switches.Contains("--no-" + switchName[2..])
                ? "--no-" + switchName[2..]
                : null,
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
        };
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

    [GeneratedRegex(@"\s+\((?:See|same as)\b.*\)$", RegexOptions.IgnoreCase)]
    private static partial Regex SynopsisExplanationPattern();

    [GeneratedRegex(@"(?<= )(?<name>orgname|username)(?=\s|$)")]
    private static partial Regex BareOrgOperandPattern();

    [GeneratedRegex(@"^npm audit \[(?<commands>[a-z][a-z0-9-]*(?:\|[a-z][a-z0-9-]*)+)\][ \t]*\r?$", RegexOptions.Multiline)]
    private static partial Regex AuditSynopsisPattern();

    [GeneratedRegex(@"^(?<command>[a-z][a-z0-9-]*)(?:\s|$)")]
    private static partial Regex ChildCommandPattern();

    [GeneratedRegex(@"[a-z][a-z0-9-]*", RegexOptions.IgnoreCase)]
    private static partial Regex CommandNamePattern();

    [GeneratedRegex(@"(?:(?<short>-[A-Za-z0-9])\|)?(?<long>--[A-Za-z0-9][A-Za-z0-9-]*)")]
    private static partial Regex SynopsisOptionPattern();

    [GeneratedRegex(
        @"^\s{2}(?:(?<short>-[A-Za-z0-9])\|)?(?<long>--[A-Za-z0-9][A-Za-z0-9-]*)\s*$")]
    private static partial Regex DetailedOptionPattern();
}
