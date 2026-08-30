using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Azure CLI (az).
/// Azure CLI uses Python's argparse with standard --flag and --option VALUE patterns.
///
/// Azure CLI help format:
/// az --help
/// Group
///     az : ...
///
/// Subgroups:
///     account       : Manage Azure subscription information.
///     acr           : Manage Azure Container Registries.
///     ...
///
/// Commands:
///     configure     : Configure Azure CLI settings.
///     feedback      : Send feedback to the Azure CLI Team.
///     ...
///
/// az account --help
/// Group
///     az account : Manage Azure subscription information.
///
/// Subgroups:
///     lock          : Manage Azure subscription level locks.
///
/// Commands:
///     clear         : Clear all subscriptions from the CLI's local cache.
///     list          : Get a list of subscriptions for the logged in account.
///     ...
///
/// az account show --help
/// Command
///     az account show : Get the details of a subscription.
///
/// Arguments
///     --subscription -s     : Name or ID of subscription.
///
/// Global Arguments
///     --debug               : Increase logging verbosity to show all debug logs.
///     --help -h             : Show this help message and exit.
///     ...
/// </summary>
public partial class AzCliScraper : CliScraperBase
{
    public AzCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<AzCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "az";

    public override string NamespacePrefix => "Az";

    public override string TargetNamespace => "ModularPipelines.Azure";

    public override string OutputDirectory => "src/ModularPipelines.Azure";


    /// <summary>
    /// Azure CLI is slow, limit parallelism to avoid overwhelming the CLI.
    /// </summary>
    protected override int MaxParallelism => 10;

    /// <summary>
    /// Azure policy arguments are described as global but are registered on the selected
    /// command parser, so they must follow the subcommand path.
    /// </summary>
    protected override bool GlobalOptionsBeforeSubcommands => false;

    protected override IReadOnlyList<CliOptionDefinition> SupplementalGlobalOptions =>
    [
        new()
        {
            SwitchName = "--acquire-policy-token",
            PropertyName = "AcquirePolicyToken",
            CSharpType = "bool?",
            Description = "Acquire an Azure Policy token automatically for this resource operation.",
            IsFlag = true,
            ValueSeparator = " ",
            IsSecret = false,
        },
        new()
        {
            SwitchName = "--change-reference",
            PropertyName = "ChangeReference",
            CSharpType = "string?",
            Description = "The related change reference ID for this resource operation.",
            IsFlag = false,
            ValueSeparator = " ",
            IsSecret = false,
        },
    ];

    /// <summary>
    /// Skip common utility subcommands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "help", "version", "upgrade", "feedback", "find", "interactive", "rest", "configure"
    };

    /// <summary>
    /// Extracts subcommand names from Azure CLI help text.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        var seenCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find Subgroups: and Commands: sections
        var sections = new[] { "Subgroups:", "Commands:" };

        foreach (var sectionHeader in sections)
        {
            var sectionIndex = helpText.IndexOf(sectionHeader, StringComparison.OrdinalIgnoreCase);
            if (sectionIndex < 0)
            {
                continue;
            }

            var sectionStart = sectionIndex + sectionHeader.Length;

            // Find where this section ends (next section header or end of text)
            var sectionEnd = helpText.Length;
            var nextSectionMatch = SectionHeaderPattern().Match(helpText, sectionStart);
            if (nextSectionMatch.Success)
            {
                sectionEnd = nextSectionMatch.Index;
            }

            var section = helpText.Substring(sectionStart, sectionEnd - sectionStart);

            // Parse command lines: "    command-name    : description"
            var lines = section.Split('\n');
            foreach (var line in lines)
            {
                var match = SubcommandLinePattern().Match(line);
                if (match.Success)
                {
                    var commandName = match.Groups["name"].Value.Trim();
                    if (!string.IsNullOrEmpty(commandName) && !commandName.Contains(' ') &&
                        seenCommands.Add(commandName))
                    {
                        subcommands.Add(commandName);
                    }
                }
            }
        }

        return subcommands;
    }

    /// <summary>
    /// Parses an Azure CLI command from its help text.
    /// </summary>
    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken)
    {
        var commandParts = commandPath.Skip(1).ToArray(); // Skip "az"

        if (commandParts.Length == 0)
        {
            // This is just the root command, skip it
            return Task.FromResult<CliCommandDefinition?>(null);
        }

        // Check if this is a group (has Subgroups: or Commands: sections) rather than a leaf command
        // Only generate options for leaf commands
        if (helpText.Contains("Subgroups:") || helpText.Contains("Commands:"))
        {
            // This is a group, not a leaf command - still explore subcommands but don't generate options class
            if (!helpText.Contains("Arguments"))
            {
                return Task.FromResult<CliCommandDefinition?>(null);
            }
        }

        // Get the first subcommand for grouping (e.g., "account" for "az account show")
        var subDomain = commandParts.Length > 1 ? ToPascalCase(commandParts[0]) : null;

        // Parse description from help text
        var description = ExtractDescription(helpText);

        // Parse options from the help text
        var options = ParseOptions(helpText);

        // If no options, skip generating this command
        if (options.Count == 0)
        {
            return Task.FromResult<CliCommandDefinition?>(null);
        }

        // Extract enums from options
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
            DocumentationUrl = null,
            Options = options,
            PositionalArguments = [],
            SubDomainGroup = subDomain,
            Enums = enums,
            CompatibilityMethods = AzCliCompatibility.GetMethods(commandParts),
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    /// <summary>
    /// Extracts description from help text.
    /// </summary>
    private static string? ExtractDescription(string helpText)
    {
        // Look for "Command" or "Group" section description
        var match = DescriptionPattern().Match(helpText);
        if (match.Success)
        {
            return match.Groups["desc"].Value.Trim();
        }

        return null;
    }

    /// <summary>
    /// Parses options from Azure CLI help text.
    /// Azure CLI uses: --option VALUE, --flag, -s (short forms)
    /// </summary>
    private static List<CliOptionDefinition> ParseOptions(string helpText)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Parse every command-specific Arguments section. Azure CLI uses both standard
        // names and command-specific names such as "Network Arguments".
        foreach (Match sectionMatch in SectionHeaderPattern().Matches(helpText))
        {
            var sectionName = sectionMatch.Groups["name"].Value.TrimEnd(':').Trim();
            if (!IsCommandArgumentsSection(sectionName))
            {
                continue;
            }

            var lines = GetSectionLines(helpText, sectionMatch);
            for (var i = 0; i < lines.Length; i++)
            {
                var option = ParseOption(lines, ref i, sectionName, seenOptions);
                if (option is not null)
                {
                    options.Add(option);
                }
            }
        }

        return options;
    }

    private static bool IsCommandArgumentsSection(string sectionName) =>
        sectionName.EndsWith("Arguments", StringComparison.OrdinalIgnoreCase)
        && !sectionName.StartsWith("Global ", StringComparison.OrdinalIgnoreCase);

    private static string[] GetSectionLines(string helpText, Match sectionMatch)
    {
        var sectionStart = sectionMatch.Index + sectionMatch.Length;
        var nextSectionMatch = SectionHeaderPattern().Match(helpText, sectionStart);
        var sectionEnd = nextSectionMatch.Success ? nextSectionMatch.Index : helpText.Length;
        return helpText[sectionStart..sectionEnd].Split('\n');
    }

    private static CliOptionDefinition? ParseOption(
        string[] lines,
        ref int lineIndex,
        string sectionName,
        ISet<string> seenOptions)
    {
        var match = AzOptionPattern().Match(lines[lineIndex]);
        if (!match.Success)
        {
            return null;
        }

        var longFlag = match.Groups["long"].Value.Trim();
        if (string.IsNullOrEmpty(longFlag)
            || !seenOptions.Add(longFlag)
            || IsGlobalOption(longFlag))
        {
            return null;
        }

        var alias = match.Groups["alias"].Value.Trim();
        var valueHint = match.Groups["value"].Value.Trim();
        var description = match.Groups["desc"].Value.Trim();
        lineIndex = AccumulateMultiLineDescription(lines, lineIndex, ref description);

        var propertyName = NormalizePropertyName(longFlag);
        if (propertyName is null)
        {
            return null;
        }

        var isRequired = match.Groups["required"].Success
                         || sectionName.Equals("Required Arguments", StringComparison.OrdinalIgnoreCase);
        var explicitBooleanValue = HelpDeclaresExplicitBooleanValue(description);
        var isFlag = !isRequired && IsPresenceOnlyFlag(
            longFlag,
            valueHint,
            description,
            explicitBooleanValue);
        var csharpType = DetermineType(
            longFlag,
            valueHint,
            description,
            isFlag,
            explicitBooleanValue);

        return new CliOptionDefinition
        {
            SwitchName = $"--{longFlag}",
            ShortForm = string.IsNullOrEmpty(alias) ? null : alias,
            PropertyName = propertyName,
            CSharpType = csharpType,
            Description = description,
            IsFlag = isFlag,
            IsRequired = isRequired,
            AcceptsMultipleValues = csharpType.StartsWith("IEnumerable"),
            GroupValues = HelpDeclaresSpaceSeparatedList(description)
                          || IsGroupedAzureGenericUpdateOption(longFlag, description),
            IsKeyValue = false,
            IsNumeric = csharpType == "int?",
            ValueSeparator = " ",
            ValueArity = GetValueArity(isFlag, description),
            EnumDefinition = null,
            IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag),
        };
    }

    /// <summary>
    /// Determines whether an option is rendered without a value.
    /// </summary>
    private static bool IsPresenceOnlyFlag(
        string switchName,
        string valueHint,
        string description,
        bool explicitBooleanValue)
    {
        if (switchName.Equals("accept-term", StringComparison.OrdinalIgnoreCase)
            && string.IsNullOrEmpty(valueHint))
        {
            return true;
        }

        if (explicitBooleanValue || HelpDeclaresOptionValue(switchName, description))
        {
            return false;
        }

        return string.IsNullOrEmpty(valueHint) ||
               valueHint.Equals("true", StringComparison.OrdinalIgnoreCase) ||
               valueHint.Equals("false", StringComparison.OrdinalIgnoreCase);
    }

    private static bool HelpDeclaresOptionValue(string switchName, string description) =>
        AzValueDescriptionPattern().IsMatch(description)
        || AzEmbeddedValueDescriptionPattern().IsMatch(description)
        || description.Contains("may be supplied", StringComparison.OrdinalIgnoreCase)
        || HelpDeclaresSpaceSeparatedList(description)
        || description.Contains("key=value", StringComparison.OrdinalIgnoreCase)
        || IsAzureGenericUpdateOption(switchName);

    private static bool HelpDeclaresOptionalValue(string description) =>
        OptionalValueDescriptionPattern().IsMatch(description);

    private static bool IsAzureGenericUpdateOption(string switchName) =>
        switchName.Equals("add", StringComparison.OrdinalIgnoreCase)
        || switchName.Equals("remove", StringComparison.OrdinalIgnoreCase)
        || switchName.Equals("set", StringComparison.OrdinalIgnoreCase);

    private static bool IsGroupedAzureGenericUpdateOption(string switchName, string description) =>
        IsAzureGenericUpdateOption(switchName)
        && !DescriptionDeclaresRepeatableOption(description);

    private static CliOptionValueArity GetValueArity(bool isFlag, string description) =>
        !isFlag && HelpDeclaresOptionalValue(description)
            ? CliOptionValueArity.Optional
            : CliOptionValueArity.Required;

    /// <summary>
    /// Determines the C# type based on value hint and description.
    /// </summary>
    private static string DetermineType(
        string switchName,
        string valueHint,
        string description,
        bool isFlag,
        bool explicitBooleanValue)
    {
        if (isFlag)
        {
            return "bool?";
        }

        var lowerHint = valueHint.ToLowerInvariant();
        var lowerDesc = description.ToLowerInvariant();

        if (IsAzureGenericUpdateOption(switchName))
        {
            return "IEnumerable<string>?";
        }

        // Check for numeric types
        if (lowerHint.Contains("number") || lowerHint.Contains("count") ||
            lowerHint.Contains("port") || lowerHint.Contains("size") ||
            lowerHint.Contains("timeout") || int.TryParse(valueHint, out _))
        {
            return "int?";
        }

        // Explicit Boolean values stay scalar unless the help specifically describes
        // the Boolean values themselves as a collection. Words such as "multiple"
        // may instead describe the resources affected by one Boolean value.
        if (explicitBooleanValue)
        {
            return HelpDeclaresBooleanList(lowerDesc)
                ? "IEnumerable<string>?"
                : "bool?";
        }

        // Check for list types (space-separated or multiple values)
        if (HelpDeclaresSpaceSeparatedList(lowerDesc)
            || DescriptionDeclaresRepeatableOption(lowerDesc)
            || lowerDesc.Contains("list of"))
        {
            return "IEnumerable<string>?";
        }

        return "string?";
    }

    private static bool HelpDeclaresBooleanList(string description) =>
        HelpDeclaresSpaceSeparatedList(description) ||
        DescriptionDeclaresRepeatableOption(description) ||
        description.Contains("list of true") ||
        description.Contains("list of false");

    private static bool HelpDeclaresSpaceSeparatedList(string description) =>
        description.Contains("space-separated", StringComparison.OrdinalIgnoreCase)
        || description.Contains("space-delimited", StringComparison.OrdinalIgnoreCase)
        || description.Contains("separated by spaces", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Checks if an option is a global option that should be on the base class.
    /// </summary>
    private static bool IsGlobalOption(string optionName)
    {
        var globalOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "debug", "help", "only-show-errors", "output", "query", "subscription", "verbose"
        };

        return globalOptions.Contains(optionName);
    }

    /// <summary>
    /// Accumulates multi-line descriptions.
    /// </summary>
    private static int AccumulateMultiLineDescription(string[] lines, int currentIndex, ref string description)
    {
        var descriptionParts = new List<string>();
        if (!string.IsNullOrEmpty(description))
        {
            descriptionParts.Add(description);
        }

        var nextIndex = currentIndex + 1;
        while (nextIndex < lines.Length)
        {
            var nextLine = lines[nextIndex];
            var trimmedNext = nextLine.Trim();

            // Stop conditions
            if (string.IsNullOrWhiteSpace(trimmedNext))
            {
                break;
            }

            // New option line (starts with dash)
            if (trimmedNext.StartsWith('-'))
            {
                break;
            }

            // Section header (ends with colon and is short)
            if (trimmedNext.Length < 50 && (trimmedNext.EndsWith(':') || char.IsUpper(trimmedNext[0])))
            {
                var words = trimmedNext.Split(' ');
                if (words.Length <= 3)
                {
                    break;
                }
            }

            // Line must be indented to be a continuation
            var leadingSpaces = nextLine.Length - nextLine.TrimStart().Length;
            if (leadingSpaces < 20) // Azure CLI uses heavy indentation for descriptions
            {
                break;
            }

            descriptionParts.Add(trimmedNext);
            nextIndex++;
        }

        description = string.Join(" ", descriptionParts);
        return nextIndex - 1;
    }

    /// <summary>
    /// Checks if help text indicates the command has options.
    /// </summary>
    protected override bool HasOptions(string helpText)
    {
        return helpText.Contains("Arguments") ||
               helpText.Contains("--") ||
               AzOptionPattern().IsMatch(helpText);
    }

    #region Regex Patterns

    /// <summary>
    /// Matches section headers like "Arguments", "Global Arguments", "Subgroups:", etc.
    /// </summary>
    [GeneratedRegex(@"^(?<name>[A-Z][\w \t]*:?)[ \t]*\r?$", RegexOptions.Multiline)]
    private static partial Regex SectionHeaderPattern();

    /// <summary>
    /// Matches subcommand lines: "    command-name    : description"
    /// </summary>
    [GeneratedRegex(@"^\s{2,}(?<name>[\w-]+)\s+:", RegexOptions.Multiline)]
    private static partial Regex SubcommandLinePattern();

    /// <summary>
    /// Matches command/group description:
    /// "Command" or "Group" followed by description on same or next line.
    /// </summary>
    [GeneratedRegex(@"(?:Command|Group)\s*\n\s*az\s+[\w\s-]+:\s*(?<desc>.+)$", RegexOptions.Multiline)]
    private static partial Regex DescriptionPattern();

    /// <summary>
    /// Matches Azure CLI-style option lines:
    /// --option -o VALUE    : Description
    /// --option --alias     : Description
    /// --flag               : Description
    /// </summary>
    [GeneratedRegex(@"^\s+--(?<long>[\w-]+)(?:\s+(?<alias>-{1,2}[\w-]+))*(?:\s+(?<value>[A-Z_]+))?(?:\s+\[(?<required>Required)\])?\s*:\s*(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex AzOptionPattern();

    [GeneratedRegex(@"^(?:(?:a|an|the)\s+)?(?:path|uri|url|name|id|identifier|description|query|string|value|marketplace version|template|resource|parameters?|managed identity|subnet|virtual network|default identity|install script|registry adapter|storage mount|key vault|source|related resource|related change|batch|issue|scope|list\s+of|defines?|validation level|denysettings|accepts?)\b", RegexOptions.IgnoreCase)]
    private static partial Regex AzValueDescriptionPattern();

    [GeneratedRegex(@"\b(?:resource ID|secret URI|URI or path|key-value pairs|accepted values?|allowed values?)\b", RegexOptions.IgnoreCase)]
    private static partial Regex AzEmbeddedValueDescriptionPattern();

    [GeneratedRegex(@"\b(?:provided|specified|supplied)\s+without\s+(?:any\s+)?(?:values?|resource IDs?)\b", RegexOptions.IgnoreCase)]
    private static partial Regex OptionalValueDescriptionPattern();

    #endregion
}
