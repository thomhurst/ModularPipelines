using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Google Cloud CLI (gcloud).
/// gcloud uses a Python argparse-based format with GROUPS/COMMANDS/FLAGS sections.
/// </summary>
public partial class GcloudCliScraper : CliScraperBase
{
    #region Required Abstract Properties

    public override string ToolName => "gcloud";
    public override string NamespacePrefix => "Gcloud";
    public override string TargetNamespace => "ModularPipelines.Google";
    public override string OutputDirectory => "src/ModularPipelines.Google";

    #endregion

    #region Gcloud-Specific Configuration

    /// <summary>
    /// On Windows, gcloud is installed as gcloud.cmd in the SDK directory.
    /// </summary>
    protected override string ExecutablePath { get; }

    #endregion

    public GcloudCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<GcloudCliScraper> logger)
        : base(executor, helpCache, logger)
    {
        ExecutablePath = ResolveGcloudPath();
        Logger.LogInformation("Resolved gcloud path: {Path}", ExecutablePath);
    }

    #region Path Resolution

    private static string ResolveGcloudPath()
    {
        // Check common installation paths on Windows
        var commonPaths = new[]
        {
            @"C:\Program Files (x86)\Google\Cloud SDK\google-cloud-sdk\bin\gcloud.cmd",
            @"C:\Program Files\Google\Cloud SDK\google-cloud-sdk\bin\gcloud.cmd",
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Cloud SDK\google-cloud-sdk\bin\gcloud.cmd"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"AppData\Local\Google\Cloud SDK\google-cloud-sdk\bin\gcloud.cmd"),
        };

        foreach (var path in commonPaths)
        {
            if (File.Exists(path))
            {
                return path;
            }
        }

        // Fall back to hoping it's in PATH
        return OperatingSystem.IsWindows() ? "gcloud.cmd" : "gcloud";
    }

    #endregion

    #region Abstract Method Implementations

    /// <summary>
    /// Extracts subcommands from gcloud's GROUPS and COMMANDS sections.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();

        // Extract from GROUPS section
        subcommands.AddRange(ExtractFromSection(helpText, "GROUPS"));

        // Extract from COMMANDS section
        subcommands.AddRange(ExtractFromSection(helpText, "COMMANDS"));

        return subcommands.Distinct();
    }

    /// <summary>
    /// Parses a gcloud command's help text into a CliCommandDefinition.
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

        var subDomain = commandParts.Length > 1 ? ToPascalCase(commandParts[0]) : null;
        var description = ExtractDescription(helpText);
        var parsedOptions = ParseOptions(helpText, commandParts);
        var options = parsedOptions.Options;
        var positionalArgs = ParsePositionalArguments(helpText);

        var enums = options
            .Where(o => o.EnumDefinition is not null)
            .Select(o => o.EnumDefinition!)
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
            DocumentationUrl = $"https://cloud.google.com/sdk/gcloud/reference/{string.Join("/", commandParts)}",
            Options = options,
            ArgumentGroups = parsedOptions.ArgumentGroups,
            PositionalArguments = positionalArgs,
            SubDomainGroup = subDomain,
            Enums = enums
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    #endregion

    #region Virtual Method Overrides

    /// <summary>
    /// gcloud uses "FLAGS" section instead of "Flags:" or "Options:".
    /// </summary>
    protected override bool HasOptions(string helpText)
    {
        return helpText.Contains("\nFLAGS\n") ||
               helpText.Contains("\nFLAGS\r\n") ||
               base.HasOptions(helpText);
    }

    /// <summary>
    /// Skip alpha/beta (unstable), help, topic (documentation only).
    /// </summary>
    protected override bool IsSkippableSubcommand(string subcommand)
    {
        var lower = subcommand.ToLowerInvariant();
        return lower is "help" or "topic" or "alpha" or "beta" ||
               base.IsSkippableSubcommand(subcommand);
    }

    protected override bool ShouldTreatOptionAsScalar(
        IReadOnlyList<string> commandParts,
        string switchName) =>
        switchName.Equals("--external-ipv6-prefix-length", StringComparison.OrdinalIgnoreCase);

    #endregion

    #region Gcloud-Specific Parsing Helpers

    private static List<string> ExtractFromSection(string helpText, string sectionName)
    {
        var subcommands = new List<string>();

        // Find the section
        var sectionMatch = Regex.Match(helpText, $@"^{sectionName}\s*$", RegexOptions.Multiline);
        if (!sectionMatch.Success)
        {
            return subcommands;
        }

        var sectionStart = sectionMatch.Index + sectionMatch.Length;

        // Find where section ends (next uppercase section header)
        var nextMatch = Regex.Match(helpText[sectionStart..], @"^[A-Z][A-Z_\s]+$", RegexOptions.Multiline);
        var sectionEnd = nextMatch.Success ? sectionStart + nextMatch.Index : helpText.Length;

        var section = helpText[sectionStart..sectionEnd];

        // gcloud format: command names are indented with 5+ spaces at line start
        // Example: "     compute"
        var matches = Regex.Matches(section, @"^\s{5}(\w[\w-]*)\s*$", RegexOptions.Multiline);
        foreach (Match match in matches)
        {
            var name = match.Groups[1].Value.Trim();
            if (!string.IsNullOrEmpty(name))
            {
                subcommands.Add(name);
            }
        }

        return subcommands;
    }

    private static string? ExtractDescription(string helpText)
    {
        // NAME section: "gcloud command - description"
        var match = Regex.Match(helpText, @"^NAME\s*\n\s+gcloud[^\n]+-\s*(.+?)(?=\n\n|\nSYNOPSIS)", RegexOptions.Singleline);
        if (match.Success)
        {
            return match.Groups[1].Value.Trim().Replace("\n", " ").Replace("  ", " ");
        }
        return null;
    }

    private (List<CliOptionDefinition> Options, IReadOnlyList<CliArgumentGroup> ArgumentGroups) ParseOptions(
        string helpText,
        IReadOnlyList<string> commandParts)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find FLAGS section
        var flagsMatch = Regex.Match(helpText, @"^FLAGS\s*$", RegexOptions.Multiline);
        if (!flagsMatch.Success)
        {
            return (options, []);
        }

        var sectionStart = flagsMatch.Index + flagsMatch.Length;

        // Find end of FLAGS section
        var nextSectionMatch = Regex.Match(helpText[sectionStart..], @"^[A-Z][A-Z_\s]+$", RegexOptions.Multiline);
        var sectionEnd = nextSectionMatch.Success ? sectionStart + nextSectionMatch.Index : helpText.Length;

        var flagsSection = helpText[sectionStart..sectionEnd];
        var argumentGroup = ParseArgumentGroups(flagsSection, ParseGcloudArgument);

        foreach (var argument in argumentGroup.FlattenArguments())
        {
            var option = CreateOption(argument);
            if (option is null || !seenOptions.Add(option.SwitchName))
            {
                continue;
            }

            if (!option.IsFlag
                && option.CSharpType is not ("bool" or "bool?")
                && !option.AcceptsMultipleValues
                && !ShouldTreatOptionAsScalar(commandParts, option.SwitchName)
                && HelpDeclaresRepeatableOption(
                    helpText,
                    option.SwitchName,
                    option.Description ?? string.Empty))
            {
                option = option with
                {
                    AcceptsMultipleValues = true,
                    CSharpType = AsCSharpType(option.CSharpType, acceptsMultipleValues: true),
                };
            }

            options.Add(option);
        }

        return (options, [argumentGroup]);
    }

    private static CliOptionDefinition? CreateOption(CliArgumentDefinition argument)
    {
        var longForm = argument.SwitchName;
        if (string.IsNullOrEmpty(longForm))
        {
            return null;
        }

        var propertyName = NormalizePropertyName(longForm);
        if (propertyName is null)
        {
            return null;
        }

        var valueHint = argument.ValueHint ?? string.Empty;
        var description = argument.Documentation;
        var isFlag = string.IsNullOrEmpty(valueHint) || argument.IsNegatable;
        var acceptsMultipleValues = !isFlag
            && (valueHint.Contains("...")
                || DescriptionDeclaresRepeatableOption(description ?? string.Empty));
        var isNumeric = IsNumericHint(valueHint);
        var isKeyValue = valueHint.Contains("KEY=VALUE") || valueHint.Contains("=VALUE,");
        var enumDefinition = TryDetectEnum(propertyName, description);

        return new CliOptionDefinition
        {
            SwitchName = longForm,
            PropertyName = propertyName,
            CSharpType = DetermineCSharpType(
                isFlag,
                acceptsMultipleValues,
                isKeyValue,
                isNumeric,
                enumDefinition),
            Description = description,
            IsFlag = isFlag,
            IsRequired = false,
            AcceptsMultipleValues = acceptsMultipleValues,
            IsKeyValue = isKeyValue,
            IsNumeric = isNumeric,
            ValueSeparator = isFlag ? " " : "=",
            EnumDefinition = enumDefinition,
            IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag, description)
        };
    }

    private static CliArgumentDefinition? ParseGcloudArgument(string line)
    {
        var match = GcloudFlagPattern().Match(line);
        if (!match.Success)
        {
            return null;
        }

        var negatable = match.Groups["negatable"].Success;
        var name = negatable
            ? $"--{match.Groups["negatableName"].Value}"
            : match.Groups["long"].Value;

        return new CliArgumentDefinition
        {
            SwitchName = name,
            ValueHint = match.Groups["value"].Value,
            IsNegatable = negatable,
            Indentation = match.Groups["indent"].Value.Length,
        };
    }

    private static List<CliPositionalArgument> ParsePositionalArguments(string helpText)
    {
        var args = new List<CliPositionalArgument>();

        var sectionMatch = Regex.Match(helpText, @"^POSITIONAL ARGUMENTS\s*$", RegexOptions.Multiline);
        if (!sectionMatch.Success)
        {
            return args;
        }

        var sectionStart = sectionMatch.Index + sectionMatch.Length;
        var nextMatch = Regex.Match(helpText[sectionStart..], @"^[A-Z][A-Z_\s]+$", RegexOptions.Multiline);
        var sectionEnd = nextMatch.Success ? sectionStart + nextMatch.Index : helpText.Length;

        var section = helpText[sectionStart..sectionEnd];

        // Match: "     ARG_NAME [ARG_NAME ...]"
        var argMatch = Regex.Match(section, @"^\s{5}([A-Z][A-Z_]+)(?:\s+\[[A-Z][A-Z_]+\s*\.\.\.\])?", RegexOptions.Multiline);
        if (argMatch.Success)
        {
            var argName = argMatch.Groups[1].Value;
            var isMultiple = argMatch.Value.Contains("...");

            var propertyName = NormalizePropertyName(argName);
            if (propertyName is not null)
            {
                args.Add(new CliPositionalArgument
                {
                    PropertyName = propertyName,
                    CSharpType = isMultiple ? "IEnumerable<string>" : "string",
                    IsRequired = true,
                    PositionIndex = 0,
                    Description = null
                });
            }
        }

        return args;
    }

    private static bool IsNumericHint(string hint)
    {
        var lower = hint.ToLowerInvariant();
        return lower.Contains("count") || lower.Contains("number") || lower.Contains("size") ||
               lower.Contains("timeout") || lower.Contains("seconds") || lower.Contains("iops");
    }

    private static CliEnumDefinition? TryDetectEnum(string propertyName, string? description)
    {
        if (string.IsNullOrEmpty(description))
        {
            return null;
        }

        // Pattern 1: "OPTION must be one of: value1, value2, value3" (comma-separated list)
        var match = Regex.Match(description, @"must be (?:one of:?\s*)([a-zA-Z][a-zA-Z0-9_-]*(?:,\s*[a-zA-Z][a-zA-Z0-9_-]*)+)", RegexOptions.IgnoreCase);
        if (match.Success)
        {
            var values = match.Groups[1].Value
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v.Trim().TrimEnd('.'))
                .Where(v => v.Length > 0 && v.Length < 25 && v.All(c => char.IsLetterOrDigit(c) || c == '-' || c == '_'))
                .Distinct()
                .ToArray();

            if (values.Length >= 2 && values.Length <= 12)
            {
                return CreateEnumDefinition(propertyName, values);
            }
        }

        // Pattern 2: "; one of value1, value2" at end of description
        match = Regex.Match(description, @";\s*one of\s+([a-zA-Z][a-zA-Z0-9_-]*(?:,\s*[a-zA-Z][a-zA-Z0-9_-]*)+)", RegexOptions.IgnoreCase);
        if (match.Success)
        {
            var values = match.Groups[1].Value
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v.Trim().TrimEnd('.'))
                .Where(v => v.Length > 0 && v.Length < 25)
                .Distinct()
                .ToArray();

            if (values.Length >= 2 && values.Length <= 12)
            {
                return CreateEnumDefinition(propertyName, values);
            }
        }

        return null;
    }

    private static CliEnumDefinition CreateEnumDefinition(string propertyName, string[] values)
    {
        // Use just the namespace prefix + property name for shorter enum names
        var enumName = $"Gcloud{propertyName}";

        return new CliEnumDefinition
        {
            EnumName = enumName,
            Values = values.Select(v => new CliEnumValue
            {
                MemberName = string.Join("", v.Split(['-', '_'], StringSplitOptions.RemoveEmptyEntries).Select(ToPascalCase)),
                CliValue = v
            }).ToList(),
            Description = $"Allowed values for --{propertyName.ToLowerInvariant()}."
        };
    }

    private static string DetermineCSharpType(
        bool isFlag,
        bool acceptsMultipleValues,
        bool isKeyValue,
        bool isNumeric,
        CliEnumDefinition? enumDef)
    {
        if (isFlag)
        {
            return "bool?";
        }

        if (enumDef is not null)
        {
            return $"{enumDef.EnumName}?";
        }

        if (isKeyValue)
        {
            return "IReadOnlyList<KeyValue>?";
        }

        var scalarType = isNumeric ? "int" : "string";
        return acceptsMultipleValues ? $"IEnumerable<{scalarType}>?" : $"{scalarType}?";
    }

    #endregion

    #region Regex Patterns

    /// <summary>
    /// Matches gcloud flag patterns:
    /// --flag
    /// --[no-]flag
    /// --option=VALUE
    /// </summary>
    [GeneratedRegex(
        @"^(?<indent>[ \t]+)(?:(?<negatable>--\[no-\])(?<negatableName>[\w-]+)|(?<long>--[\w-]+))(?:=(?<value>[^\s]+))?(?:,\s*-[\w-]+(?:[ =]\S+)?)?$")]
    private static partial Regex GcloudFlagPattern();

    #endregion
}
