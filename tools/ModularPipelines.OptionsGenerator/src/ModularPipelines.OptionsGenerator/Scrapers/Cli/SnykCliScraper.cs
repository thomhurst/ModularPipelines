using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Snyk security CLI.
/// Snyk uses a custom help format with subcommands.
///
/// snyk help format (snyk --help):
/// CLI commands help
///   Snyk CLI scans and monitors your projects for security vulnerabilities.
///
/// Usage: snyk [options] [command] [args]
///
/// Commands:
///   auth [API_TOKEN]       Authenticate Snyk CLI with a Snyk account
///   test                   Test a project for vulnerabilities
///   monitor                Monitor a project for new vulnerabilities
///   ...
/// </summary>
public partial class SnykCliScraper : CliScraperBase
{
    private static readonly HashSet<string> NumericOptions = new(StringComparer.OrdinalIgnoreCase)
    {
        "--detection-depth",
        "--max-depth",
        "--nested-jars-depth",
    };

    private static readonly HashSet<string> ValueOptionsWithoutHelpPlaceholders = new(StringComparer.OrdinalIgnoreCase)
    {
        "--config-dir",
        "--dotnet-target-framework",
        "--fetch-tfstate-headers",
        "--filter",
        "--json-file-output",
        "--packages-folder",
        "--repo",
        "--tf-lockfile",
        "--tf-provider-version",
        "--tfc-endpoint",
        "--tfc-token",
    };

    public SnykCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<SnykCliScraper> logger)
        : base(executor, helpCache, logger)
    {
        ExecutablePath = ResolveExecutablePath();
    }

    public override string ToolName => "snyk";

    public override string NamespacePrefix => "Snyk";

    public override string TargetNamespace => "ModularPipelines.Snyk";

    public override string OutputDirectory => "src/ModularPipelines.Snyk";

    protected override string ExecutablePath { get; }

    protected override int MaxParallelism => 4;

    public override async IAsyncEnumerable<CliCommandDefinition> ScrapeAsync(
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in base.ScrapeAsync(cancellationToken))
        {
            commands.Add(command);
        }

        foreach (var command in DisambiguateEnumNames(commands))
        {
            yield return command;
        }
    }

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "about", "config"
    };

    /// <summary>
    /// Extracts subcommand names from Snyk help text.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        var seenCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var normalizedText = helpText.Replace("\r\n", "\n").Replace("\r", "\n");

        if (AvailableCommandsPattern().IsMatch(normalizedText))
        {
            AddMatches(RootCommandPattern().Matches(normalizedText));
        }
        else
        {
            var groupMatch = GroupOverviewPattern().Match(normalizedText);
            if (groupMatch.Success)
            {
                var groupName = groupMatch.Groups["group"].Value;
                AddMatches(GroupCommandPattern().Matches(normalizedText)
                    .Cast<Match>()
                    .Where(x => x.Groups["group"].Value.Equals(groupName, StringComparison.OrdinalIgnoreCase)));
            }
            else if (AibomOverviewPattern().IsMatch(normalizedText))
            {
                AddMatches(AibomCommandPattern().Matches(normalizedText).Cast<Match>());
            }
            else if (SbomOverviewPattern().IsMatch(normalizedText))
            {
                subcommands.Add("test");
            }
        }

        Logger.LogInformation("[snyk] Extracted {Count} subcommands", subcommands.Count);
        return subcommands;

        void AddMatches(IEnumerable<Match> matches)
        {
            foreach (var match in matches)
            {
                var commandName = match.Groups["command"].Value.Trim();
                if (!string.IsNullOrEmpty(commandName) &&
                    seenCommands.Add(commandName))
                {
                    subcommands.Add(commandName);
                }
            }
        }
    }

    /// <summary>
    /// Parses a Snyk command from its help text.
    /// </summary>
    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken)
    {
        var commandParts = commandPath.Skip(1).ToArray();

        if (commandParts.Length == 0)
        {
            return Task.FromResult<CliCommandDefinition?>(null);
        }

        var description = ExtractDescription(helpText);
        var options = ParseOptions(helpText);
        AddDocumentedOptions(commandParts, options);
        var positionalArguments = GetPositionalArguments(commandParts);
        var enums = options
            .Where(x => x.EnumDefinition is not null)
            .Select(x => x.EnumDefinition!)
            .GroupBy(x => x.EnumName, StringComparer.OrdinalIgnoreCase)
            .Select(x => x.First())
            .ToList();

        var className = GenerateClassName(commandPath);

        var command = new CliCommandDefinition
        {
            FullCommand = string.Join(" ", commandPath),
            CommandParts = commandParts,
            ClassName = className,
            ParentClassName = BaseOptionsClassName,
            ToolNamespacePrefix = NamespacePrefix,
            Description = description,
            DocumentationUrl = "https://docs.snyk.io/snyk-cli/commands",
            Options = options,
            PositionalArguments = positionalArguments,
            SubDomainGroup = null,
            Enums = enums
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    /// <summary>
    /// Extracts description from help text.
    /// </summary>
    private static string? ExtractDescription(string helpText)
    {
        var lines = helpText.Split('\n');

        foreach (var line in lines)
        {
            var trimmed = line.Trim();

            if (string.IsNullOrWhiteSpace(trimmed))
            {
                continue;
            }

            // Skip section headers
            if (trimmed.EndsWith(':') || trimmed.StartsWith("Usage:"))
            {
                continue;
            }

            // Skip command lines
            if (trimmed.StartsWith("snyk"))
            {
                continue;
            }

            if (trimmed.Length > 20)
            {
                return trimmed;
            }
        }

        return null;
    }

    /// <summary>
    /// Parses options from Snyk help text.
    /// Format: --severity-threshold=&lt;low|medium|high|critical&gt;
    ///         --json
    ///         --sarif
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(string helpText)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var optionsMatch = OptionsSectionPattern().Match(helpText);
        if (!optionsMatch.Success)
        {
            ParseOptionLines(helpText.Split('\n'), options, seenOptions);
        }
        else
        {
            var sectionStart = optionsMatch.Index + optionsMatch.Length;
            var nextSection = NextSectionPattern().Match(helpText, sectionStart);
            var sectionEnd = nextSection.Success ? nextSection.Index : helpText.Length;
            var section = helpText.Substring(sectionStart, sectionEnd - sectionStart);
            ParseOptionLines(section.Split('\n'), options, seenOptions);
        }

        if (DebugOptionPattern().IsMatch(helpText) && seenOptions.Add("-d"))
        {
            options.Add(new CliOptionDefinition
            {
                SwitchName = "-d",
                PropertyName = "Debug",
                CSharpType = "bool?",
                Description = "Output debug logs.",
                IsFlag = true,
                ValueSeparator = " ",
            });
        }

        return options;
    }

    private void ParseOptionLines(
        string[] lines,
        List<CliOptionDefinition> options,
        HashSet<string> seenOptions)
    {
        var optionIndentation = lines
            .Where(IsOptionHeading)
            .Select(GetIndentation)
            .DefaultIfEmpty(-1)
            .Min();

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];

            if (!IsOptionHeading(line) || !HasOptionHeadingIndentation(lines, i, optionIndentation))
            {
                continue;
            }

            var description = ExtractOptionDescription(lines, i);
            foreach (Match match in SnykOptionPattern().Matches(line))
            {
                var longForm = match.Groups["long"].Value.Trim();
                var valueHint = match.Groups["value"].Value.Trim().Trim('<', '>', '[', ']');

                if (!seenOptions.Add(longForm))
                {
                    continue;
                }

                var propertyName = NormalizePropertyName(longForm);
                if (propertyName is null)
                {
                    continue;
                }

                var isNumeric = NumericOptions.Contains(longForm);
                var isFlag = string.IsNullOrEmpty(valueHint)
                    && !isNumeric
                    && !ValueOptionsWithoutHelpPlaceholders.Contains(longForm);
                var isBoolean = IsBooleanValueHint(valueHint);
                var csharpType = isFlag || isBoolean ? "bool?" : isNumeric ? "int?" : "string?";

                CliEnumDefinition? enumDef = null;
                if (!isBoolean && !string.IsNullOrEmpty(valueHint) && valueHint.Contains('|'))
                {
                    var values = valueHint.Split('|', StringSplitOptions.RemoveEmptyEntries)
                        .Select(v => v.Trim())
                        .ToArray();
                    if (values.Length >= 2)
                    {
                        enumDef = new CliEnumDefinition
                        {
                            EnumName = $"Snyk{propertyName}",
                            Values = values.Select(v => new CliEnumValue
                            {
                                MemberName = GeneratorUtils.ToEnumMemberName(v),
                                CliValue = v
                            }).ToList(),
                            Description = $"Allowed values for --{longForm.TrimStart('-')}"
                        };
                        csharpType = $"{enumDef.EnumName}?";
                    }
                }

                options.Add(new CliOptionDefinition
                {
                    SwitchName = longForm,
                    ShortForm = null,
                    PropertyName = propertyName,
                    CSharpType = csharpType,
                    Description = description,
                    IsFlag = isFlag,
                    IsRequired = description?.Contains("Required.", StringComparison.OrdinalIgnoreCase) == true,
                    AcceptsMultipleValues = false,
                    IsKeyValue = false,
                    IsNumeric = isNumeric,
                    ValueSeparator = isFlag ? " " : "=",
                    EnumDefinition = enumDef,
                    IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag)
                        || longForm.Equals("--fetch-tfstate-headers", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
    }

    private static void AddDocumentedOptions(
        string[] commandParts,
        List<CliOptionDefinition> options)
    {
        switch (string.Join(' ', commandParts))
        {
            case "sbom":
                AddOptionIfMissing(
                    options,
                    "--allow-incomplete-sbom",
                    "AllowIncompleteSbom",
                    "bool?",
                    "Continue generating an SBOM when some projects fail to resolve",
                    isFlag: true);
                break;
            case "container sbom":
                AddOptionIfMissing(options, "--org", "Org", "string?", "Snyk Organization ID");
                AddOptionIfMissing(
                    options,
                    "--exclude-app-vulns",
                    "ExcludeAppVulns",
                    "bool?",
                    "Exclude application vulnerabilities",
                    isFlag: true);
                AddOptionIfMissing(
                    options,
                    "--exclude-node-modules",
                    "ExcludeNodeModules",
                    "bool?",
                    "Exclude node_modules scanning",
                    isFlag: true);
                AddOptionIfMissing(
                    options,
                    "--nested-jars-depth",
                    "NestedJarsDepth",
                    "int?",
                    "Maximum nested JAR extraction depth",
                    isNumeric: true);
                AddOptionIfMissing(options, "--username", "Username", "string?", "Container registry username");
                AddOptionIfMissing(options, "--password", "Password", "string?", "Container registry password", isSecret: true);
                break;
        }
    }

    private static void AddOptionIfMissing(
        List<CliOptionDefinition> options,
        string switchName,
        string propertyName,
        string csharpType,
        string description,
        bool isFlag = false,
        bool isNumeric = false,
        bool isSecret = false)
    {
        if (options.Any(x => x.SwitchName.Equals(switchName, StringComparison.OrdinalIgnoreCase)))
        {
            return;
        }

        options.Add(new CliOptionDefinition
        {
            SwitchName = switchName,
            PropertyName = propertyName,
            CSharpType = csharpType,
            Description = description,
            IsFlag = isFlag,
            IsNumeric = isNumeric,
            ValueSeparator = isFlag ? " " : "=",
            IsSecret = isSecret,
        });
    }

    private static bool IsBooleanValueHint(string valueHint)
    {
        var values = valueHint.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return values.Length == 2 &&
               values.Contains("true", StringComparer.OrdinalIgnoreCase) &&
               values.Contains("false", StringComparer.OrdinalIgnoreCase);
    }

    private static List<CliPositionalArgument> GetPositionalArguments(string[] commandParts)
    {
        return string.Join(' ', commandParts) switch
        {
            "auth" =>
            [
                CreatePositional("ApiToken", "API_TOKEN", "Snyk API token", isRequired: false, isSecret: true)
            ],
            "test" or "monitor" =>
            [
                CreatePositional("Target", "TARGET", "Package, version, or repository target to scan", isRequired: false)
            ],
            "container test" or "container monitor" or "container sbom" =>
            [
                CreatePositional("Image", "IMAGE", "Container image to scan", isRequired: true)
            ],
            "iac test" =>
            [
                CreatePositional("Path", "PATH", "Infrastructure as Code path to scan", isRequired: false)
            ],
            "code test" =>
            [
                CreatePositional("Path", "PATH", "Source code path to scan", isRequired: false)
            ],
            "sbom" =>
            [
                CreatePositional("TargetDirectory", "TARGET_DIRECTORY", "Project directory to scan", isRequired: false)
            ],
            "policy" =>
            [
                CreatePositional("PathToPolicyFile", "PATH_TO_POLICY_FILE", "Path to the Snyk policy file", isRequired: false)
            ],
            _ => []
        };
    }

    private static CliPositionalArgument CreatePositional(
        string propertyName,
        string placeholderName,
        string description,
        bool isRequired,
        bool isSecret = false)
    {
        return new CliPositionalArgument
        {
            PropertyName = propertyName,
            PlaceholderName = placeholderName,
            CSharpType = "string?",
            Description = description,
            IsRequired = isRequired,
            IsSecret = isSecret,
            PositionIndex = 0,
            Placement = PositionalArgumentPosition.BeforeOptions,
        };
    }

    internal static string ResolveExecutablePath(string? searchPath = null, bool? isWindows = null)
    {
        if (!(isWindows ?? OperatingSystem.IsWindows()))
        {
            return "snyk";
        }

        var pathDirectories = (searchPath ?? Environment.GetEnvironmentVariable("PATH"))?
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            ?? [];

        foreach (var pathDirectory in pathDirectories)
        {
            foreach (var executableName in new[] { "snyk.exe", "snyk.cmd" })
            {
                var candidate = Path.Combine(pathDirectory.Trim('"'), executableName);
                if (File.Exists(candidate))
                {
                    return Path.GetFullPath(candidate);
                }
            }
        }

        return "snyk";
    }

    private static string? ExtractOptionDescription(string[] lines, int currentIndex)
    {
        // Look for description on same line or next lines
        var nextIndex = currentIndex + 1;
        var descParts = new List<string>();

        while (nextIndex < lines.Length)
        {
            var line = lines[nextIndex];
            if (string.IsNullOrWhiteSpace(line) || IsOptionHeading(line))
            {
                break;
            }

            descParts.Add(line.Trim());
            nextIndex++;
        }

        return descParts.Count > 0 ? string.Join(" ", descParts) : null;
    }

    private static bool IsOptionHeading(string line)
    {
        return line.TrimStart().TrimStart('[').StartsWith("--", StringComparison.Ordinal);
    }

    private static int GetIndentation(string line)
    {
        return line.Length - line.TrimStart().Length;
    }

    private static bool HasOptionHeadingIndentation(string[] lines, int index, int standardIndentation)
    {
        var indentation = GetIndentation(lines[index]);
        if (indentation == standardIndentation)
        {
            return true;
        }

        if (indentation < standardIndentation || index == 0 || !string.IsNullOrWhiteSpace(lines[index - 1]))
        {
            return false;
        }

        return lines
            .Skip(index + 1)
            .FirstOrDefault(line => !string.IsNullOrWhiteSpace(line)) is { } nextLine
            && GetIndentation(nextLine) > indentation;
    }

    internal static IReadOnlyList<CliCommandDefinition> DisambiguateEnumNames(
        IReadOnlyList<CliCommandDefinition> commands)
    {
        var canonicalValueSets = commands
            .SelectMany(command => command.Options)
            .Where(option => option.EnumDefinition is not null)
            .GroupBy(option => option.EnumDefinition!.EnumName, StringComparer.OrdinalIgnoreCase)
            .Select(group => new
            {
                group.Key,
                ValueSets = group
                    .GroupBy(option => GetEnumValueSet(option.EnumDefinition!), StringComparer.OrdinalIgnoreCase)
                    .ToList(),
            })
            .Where(group => group.ValueSets.Count > 1)
            .ToDictionary(
                group => group.Key,
                group => group.ValueSets
                    .OrderByDescending(valueSet => valueSet.Count())
                    .ThenByDescending(valueSet => valueSet.First().EnumDefinition!.Values.Count)
                    .ThenBy(valueSet => valueSet.Key, StringComparer.OrdinalIgnoreCase)
                    .First()
                    .Key,
                StringComparer.OrdinalIgnoreCase);

        return commands.Select(command =>
        {
            var enumPrefix = command.ClassName.EndsWith("Options", StringComparison.Ordinal)
                ? command.ClassName[..^"Options".Length]
                : command.ClassName;
            var options = command.Options.Select(option =>
            {
                var enumDefinition = option.EnumDefinition;
                if (enumDefinition is null ||
                    !canonicalValueSets.TryGetValue(enumDefinition.EnumName, out var canonicalValueSet) ||
                    string.Equals(GetEnumValueSet(enumDefinition), canonicalValueSet, StringComparison.OrdinalIgnoreCase))
                {
                    return option;
                }

                var enumName = $"{enumPrefix}{option.PropertyName}";
                return option with
                {
                    CSharpType = option.CSharpType.Replace(enumDefinition.EnumName, enumName, StringComparison.Ordinal),
                    EnumDefinition = enumDefinition with { EnumName = enumName },
                };
            }).ToList();

            return command with
            {
                Options = options,
                Enums = options
                    .Where(option => option.EnumDefinition is not null)
                    .Select(option => option.EnumDefinition!)
                    .DistinctBy(enumDefinition => enumDefinition.EnumName, StringComparer.OrdinalIgnoreCase)
                    .ToList(),
            };
        }).ToList();
    }

    private static string GetEnumValueSet(CliEnumDefinition enumDefinition)
    {
        return string.Join(
            '\u001F',
            enumDefinition.Values
                .Select(value => value.CliValue)
                .Order(StringComparer.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Checks if help text indicates the command has options.
    /// </summary>
    protected override bool HasOptions(string helpText)
    {
        return helpText.Contains("Usage", StringComparison.OrdinalIgnoreCase);
    }

    #region Regex Patterns

    /// <summary>
    /// Matches the current root command section.
    /// </summary>
    [GeneratedRegex(@"^Available commands\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex AvailableCommandsPattern();

    [GeneratedRegex(@"^\s+snyk\s+(?<command>[\w-]+)\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex RootCommandPattern();

    [GeneratedRegex(@"^snyk\s+(?<group>container|iac|code|sbom)\s+commands?\b", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex GroupOverviewPattern();

    [GeneratedRegex(@"^\s*(?:-\s+)?(?<group>container|iac|code|sbom)\s+(?<command>[\w-]+)\s*[,;]", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex GroupCommandPattern();

    [GeneratedRegex(@"^SBOM\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex SbomOverviewPattern();

    [GeneratedRegex(@"^AI-BOM\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex AibomOverviewPattern();

    [GeneratedRegex(@"^\s*See also:\s+snyk\s+aibom\s+(?<command>[\w-]+)\b", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex AibomCommandPattern();

    /// <summary>
    /// Matches "Options:" section header.
    /// </summary>
    [GeneratedRegex(@"^Options?:?\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex OptionsSectionPattern();

    /// <summary>
    /// Matches next section headers.
    /// </summary>
    [GeneratedRegex(@"\n[A-Z][\w\s]*:\s*\n")]
    private static partial Regex NextSectionPattern();

    /// <summary>
    /// Matches Snyk-style option lines:
    /// --severity-threshold=&lt;low|medium|high|critical&gt;
    /// --json
    /// </summary>
    [GeneratedRegex(@"(?<long>--[\w-]+)(?:=(?:<(?<value>[^>\s]+)>?|(?<value>[^\s,]+)))?")]
    private static partial Regex SnykOptionPattern();

    [GeneratedRegex(@"\bUse (?:the )?-d(?: option)?\b", RegexOptions.IgnoreCase)]
    private static partial Regex DebugOptionPattern();

    #endregion
}
