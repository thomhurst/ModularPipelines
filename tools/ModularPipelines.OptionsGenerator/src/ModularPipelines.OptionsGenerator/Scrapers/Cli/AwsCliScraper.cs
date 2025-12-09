using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for AWS CLI.
/// AWS CLI uses Python argparse with a format similar to gcloud.
///
/// AWS CLI help format (aws help):
/// NAME
///        aws -
///
/// DESCRIPTION
///        The  AWS  Command  Line Interface is a unified tool to manage...
///
/// SYNOPSIS
///        aws [options] &lt;command&gt; &lt;subcommand&gt; [parameters]
///
/// OPTIONS
///        --debug (boolean)
///        Turn on debug logging...
///
///        --endpoint-url (string)
///        Override command's default URL...
///
/// AVAILABLE SERVICES
///        o accessanalyzer
///        o account
///        o acm
///        ...
///
/// Service help (aws ec2 help):
/// NAME
///        ec2 -
///
/// DESCRIPTION
///        Amazon Elastic Compute Cloud (Amazon EC2) provides...
///
/// AVAILABLE COMMANDS
///        o allocate-address
///        o describe-instances
///        ...
///
/// Command help (aws ec2 describe-instances help):
/// NAME
///        describe-instances -
///
/// DESCRIPTION
///        Describes the specified instances...
///
/// OPTIONS
///        --instance-ids (list)
///        The instance IDs...
/// </summary>
public partial class AwsCliScraper : CliScraperBase
{
    public AwsCliScraper(ICliCommandExecutor executor, ILogger<AwsCliScraper> logger)
        : base(executor, logger)
    {
    }

    public override string ToolName => "aws";

    public override string NamespacePrefix => "Aws";

    public override string TargetNamespace => "ModularPipelines.AmazonWebServices";

    public override string OutputDirectory => "src/ModularPipelines.AmazonWebServices";

    /// <summary>
    /// AWS CLI has 3-level nesting: aws -> service -> command.
    /// </summary>
    protected override int MaxDiscoveryDepth => 3;

    /// <summary>
    /// AWS CLI has 350+ services - use higher parallelism for faster discovery.
    /// </summary>
    protected override int MaxParallelism => Math.Max(Environment.ProcessorCount * 2, 16);

    /// <summary>
    /// Skip utility commands and commands that don't have traditional options.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "help", "configure", "history", "cli-dev", "alias"
    };

    /// <summary>
    /// AWS CLI uses "help" suffix instead of "--help".
    /// </summary>
    protected override async Task<string?> GetHelpTextAsync(string[] commandPath, CancellationToken cancellationToken)
    {
        var cacheKey = string.Join(" ", commandPath);

        // AWS CLI format: aws [service] [command] help
        var args = commandPath.Length > 1
            ? string.Join(" ", commandPath.Skip(1)) + " help"
            : "help";

        var result = await Executor.ExecuteAsync(ExecutablePath, args, cancellationToken);

        // AWS CLI outputs help to stdout
        var helpText = !string.IsNullOrEmpty(result.StandardOutput)
            ? result.StandardOutput
            : result.StandardError;

        if (!string.IsNullOrWhiteSpace(helpText))
        {
            return helpText;
        }

        Logger.LogWarning("No help text for command: {Command}", cacheKey);
        return null;
    }

    /// <summary>
    /// Extracts subcommands from AWS CLI's "AVAILABLE SERVICES" or "AVAILABLE COMMANDS" sections.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        var seenCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Extract from AVAILABLE SERVICES section (root level)
        subcommands.AddRange(ExtractFromSection(helpText, "AVAILABLE SERVICES", seenCommands));

        // Extract from AVAILABLE COMMANDS section (service level)
        subcommands.AddRange(ExtractFromSection(helpText, "AVAILABLE COMMANDS", seenCommands));

        return subcommands;
    }

    /// <summary>
    /// Parses an AWS CLI command from its help text.
    /// </summary>
    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken)
    {
        var commandParts = commandPath.Skip(1).ToArray(); // Skip tool name

        if (commandParts.Length == 0)
        {
            // Root command, skip
            return Task.FromResult<CliCommandDefinition?>(null);
        }

        // Service-level commands (aws ec2) - these are groups, not leaf commands
        // Only parse leaf commands (aws ec2 describe-instances)
        if (commandParts.Length == 1 && !HasLeafOptions(helpText))
        {
            return Task.FromResult<CliCommandDefinition?>(null);
        }

        // Get subdomain for grouping (service name like "ec2", "s3", "lambda")
        var subDomain = commandParts.Length > 0 ? ToPascalCase(commandParts[0]) : null;

        var description = ExtractDescription(helpText);
        var options = ParseOptions(helpText, commandParts);

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
            DocumentationUrl = $"https://awscli.amazonaws.com/v2/documentation/api/latest/reference/{string.Join("/", commandParts)}/index.html",
            Options = options,
            PositionalArguments = [],
            SubDomainGroup = subDomain,
            Enums = enums
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    /// <summary>
    /// Checks if help text has leaf command options (not just subcommands).
    /// </summary>
    private static bool HasLeafOptions(string helpText)
    {
        // Leaf commands have OPTIONS section with actual options, not just global options
        // Service-level commands have AVAILABLE COMMANDS but minimal OPTIONS
        return Regex.IsMatch(helpText, @"^OPTIONS\s*$", RegexOptions.Multiline) &&
               !Regex.IsMatch(helpText, @"^AVAILABLE COMMANDS\s*$", RegexOptions.Multiline);
    }

    /// <summary>
    /// AWS uses "FLAGS" or "OPTIONS" sections.
    /// </summary>
    protected override bool HasOptions(string helpText)
    {
        return Regex.IsMatch(helpText, @"^OPTIONS\s*$", RegexOptions.Multiline) ||
               helpText.Contains("--");
    }

    #region AWS-Specific Parsing Helpers

    private static List<string> ExtractFromSection(string helpText, string sectionName, HashSet<string> seenCommands)
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
        var nextMatch = Regex.Match(helpText[sectionStart..], @"^[A-Z][A-Z\s]+$", RegexOptions.Multiline);
        var sectionEnd = nextMatch.Success ? sectionStart + nextMatch.Index : helpText.Length;

        var section = helpText[sectionStart..sectionEnd];

        // AWS format: "       o command-name" or just indented "command-name"
        var matches = AwsCommandPattern().Matches(section);
        foreach (Match match in matches)
        {
            var name = match.Groups["name"].Value.Trim();
            if (!string.IsNullOrEmpty(name) && seenCommands.Add(name))
            {
                subcommands.Add(name);
            }
        }

        return subcommands;
    }

    private static string? ExtractDescription(string helpText)
    {
        // NAME section: "command-name -"
        // DESCRIPTION section follows
        var descMatch = Regex.Match(helpText, @"^DESCRIPTION\s*\n\s+(.+?)(?=\n\n[A-Z]|\nSYNOPSIS|\nOPTIONS|\nAVAILABLE)", RegexOptions.Singleline | RegexOptions.Multiline);
        if (descMatch.Success)
        {
            var desc = descMatch.Groups[1].Value.Trim();
            // Clean up whitespace
            desc = Regex.Replace(desc, @"\s+", " ");
            // Truncate if too long
            if (desc.Length > 500)
            {
                desc = desc[..500] + "...";
            }
            return desc;
        }
        return null;
    }

    private List<CliOptionDefinition> ParseOptions(string helpText, string[] commandParts)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var className = GenerateClassName([ToolName, .. commandParts]);

        // Find OPTIONS section
        var optionsMatch = Regex.Match(helpText, @"^OPTIONS\s*$", RegexOptions.Multiline);
        if (!optionsMatch.Success)
        {
            return options;
        }

        var sectionStart = optionsMatch.Index + optionsMatch.Length;

        // Find end of OPTIONS section
        var nextSectionMatch = Regex.Match(helpText[sectionStart..], @"^[A-Z][A-Z\s]+$", RegexOptions.Multiline);
        var sectionEnd = nextSectionMatch.Success ? sectionStart + nextSectionMatch.Index : helpText.Length;

        var optionsSection = helpText[sectionStart..sectionEnd];

        // Parse each option: "--option (type)" or "--option"
        var optionMatches = AwsOptionPattern().Matches(optionsSection);

        foreach (Match match in optionMatches)
        {
            var longForm = match.Groups["long"].Value;
            var typeHint = match.Groups["type"].Value.Trim().Trim('(', ')');

            if (string.IsNullOrEmpty(longForm) || seenOptions.Contains(longForm))
            {
                continue;
            }

            // Skip global options that are duplicated in every command
            if (IsGlobalOption(longForm) && commandParts.Length > 1)
            {
                continue;
            }

            seenOptions.Add(longForm);

            var propertyName = NormalizePropertyName(longForm);
            if (propertyName is null)
            {
                continue;
            }

            // Get description (lines following the option with more indentation)
            var optionEnd = match.Index + match.Length;
            var descMatch = Regex.Match(optionsSection[optionEnd..], @"^\s{8,}(.+?)(?=\n\s{7}--|\n\n\s{7}--|\z)", RegexOptions.Singleline);
            var description = descMatch.Success
                ? Regex.Replace(descMatch.Groups[1].Value.Trim(), @"\s+", " ")
                : null;

            var isFlag = IsAwsBooleanType(typeHint);
            var isArray = typeHint.Contains("list") || typeHint.Contains("...") || (description?.Contains("multiple values") ?? false);
            var isNumeric = IsNumericType(typeHint);
            var isKeyValue = typeHint.Contains("map") || typeHint.Contains("structure") || (description?.Contains("key=value") ?? false);

            var enumDef = TryDetectEnum(propertyName, className, description);
            var csharpType = DetermineCSharpType(isFlag, isArray, isKeyValue, isNumeric, enumDef);

            options.Add(new CliOptionDefinition
            {
                SwitchName = longForm,
                PropertyName = propertyName,
                CSharpType = csharpType,
                Description = description,
                IsFlag = isFlag,
                IsRequired = false,
                AcceptsMultipleValues = isArray,
                IsKeyValue = isKeyValue,
                IsNumeric = isNumeric,
                ValueSeparator = isFlag ? " " : " ",
                EnumDefinition = enumDef
            });
        }

        return options;
    }

    private static bool IsGlobalOption(string optionName)
    {
        var globalOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "--debug", "--endpoint-url", "--no-verify-ssl", "--no-paginate",
            "--output", "--query", "--profile", "--region", "--version",
            "--color", "--no-sign-request", "--ca-bundle", "--cli-read-timeout",
            "--cli-connect-timeout", "--cli-binary-format", "--no-cli-pager",
            "--cli-auto-prompt", "--no-cli-auto-prompt"
        };
        return globalOptions.Contains(optionName);
    }

    private static bool IsAwsBooleanType(string typeHint)
    {
        var lower = typeHint.ToLowerInvariant();
        return string.IsNullOrEmpty(lower) || lower == "boolean" || lower == "bool";
    }

    private static bool IsNumericType(string typeHint)
    {
        var lower = typeHint.ToLowerInvariant();
        return lower.Contains("integer") || lower.Contains("long") || lower.Contains("float") || lower.Contains("double");
    }

    private static CliEnumDefinition? TryDetectEnum(string propertyName, string className, string? description)
    {
        if (string.IsNullOrEmpty(description))
        {
            return null;
        }

        // Pattern: "Possible values: value1, value2, value3" or "Valid values: ..."
        var match = Regex.Match(description, @"(?:Possible|Valid|Allowed)\s+values?:\s*([a-zA-Z][a-zA-Z0-9_-]*(?:,?\s*[a-zA-Z][a-zA-Z0-9_-]*)+)", RegexOptions.IgnoreCase);
        if (match.Success)
        {
            var values = match.Groups[1].Value
                .Split([',', ' '], StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v.Trim().TrimEnd('.'))
                .Where(v => v.Length > 0 && v.Length < 30 && IsValidEnumValue(v))
                .Distinct()
                .ToArray();

            if (values.Length >= 2 && values.Length <= 15)
            {
                return CreateEnumDefinition(propertyName, className, values);
            }
        }

        return null;
    }

    private static bool IsValidEnumValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        // Must start with letter
        if (!char.IsLetter(value[0]))
        {
            return false;
        }

        return value.All(c => char.IsLetterOrDigit(c) || c == '-' || c == '_');
    }

    private static CliEnumDefinition CreateEnumDefinition(string propertyName, string className, string[] values)
    {
        var enumName = $"{className.Replace("Options", "")}{propertyName}";

        return new CliEnumDefinition
        {
            EnumName = enumName,
            Values = values.Select(v => new CliEnumValue
            {
                MemberName = NormalizeEnumMemberName(v),
                CliValue = v
            }).ToList(),
            Description = $"Allowed values for --{propertyName.ToLowerInvariant()}."
        };
    }

    private static string NormalizeEnumMemberName(string value)
    {
        var cleaned = value.Replace("-", "_").Replace(".", "_");
        var parts = cleaned.Split('_', StringSplitOptions.RemoveEmptyEntries);
        return string.Join("", parts.Select(ToPascalCase));
    }

    private static string DetermineCSharpType(bool isFlag, bool isArray, bool isKeyValue, bool isNumeric, CliEnumDefinition? enumDef)
    {
        if (isFlag) return "bool?";
        if (enumDef is not null) return $"{enumDef.EnumName}?";
        if (isKeyValue) return "KeyValue[]?";
        if (isArray) return "IEnumerable<string>?";
        if (isNumeric) return "int?";
        return "string?";
    }

    #endregion

    #region Regex Patterns

    /// <summary>
    /// Matches AWS command lines in AVAILABLE SERVICES/COMMANDS sections.
    /// Format: "       o command-name" or indented "command-name"
    /// </summary>
    [GeneratedRegex(@"^\s+o\s+(?<name>[\w-]+)", RegexOptions.Multiline)]
    private static partial Regex AwsCommandPattern();

    /// <summary>
    /// Matches AWS CLI option patterns:
    /// --option (type)
    /// --flag
    /// </summary>
    [GeneratedRegex(@"^\s{7}(?<long>--[\w-]+)(?:\s+\((?<type>[^)]+)\))?", RegexOptions.Multiline)]
    private static partial Regex AwsOptionPattern();

    #endregion
}
