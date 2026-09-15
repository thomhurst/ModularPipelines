using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
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
    private static readonly string[] GcloudUsageSynopsisHeadings = ["SYNOPSIS"];

    private static readonly string[] StructuredExampleEndMarkers =
        ["JSON Example:", "YAML Example:", "File Example:"];

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

    protected override IReadOnlyList<string> UsageSynopsisHeadings => GcloudUsageSynopsisHeadings;

    #endregion

    public GcloudCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<GcloudCliScraper> logger)
        : base(executor, helpCache, logger)
    {
        ExecutablePath = ResolveGcloudPath();
        if (Logger.IsEnabled(LogLevel.Information))
        {
            Logger.LogInformation("Resolved gcloud path: {Path}", ExecutablePath);
        }
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
        CancellationToken cancellationToken) =>
        ParseCommandAsync(commandPath, helpText, ParseUsageSynopsis(commandPath, helpText), cancellationToken);

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

        var subDomain = commandParts.Length > 1 ? ToPascalCase(commandParts[0]) : null;
        var description = ExtractDescription(helpText);
        var parsedOptions = ParseArguments(helpText, commandParts, commandPath, usage);
        var options = parsedOptions.Options;
        var positionalArgs = parsedOptions.PositionalArguments;

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
            RequiredAlternativeGroups = parsedOptions.RequiredAlternativeGroups,
            PositionalArguments = positionalArgs,
            UsageSynopsis = usage.Synopsis,
            SubDomainGroup = subDomain,
            Enums = enums
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    #endregion

    #region Virtual Method Overrides

    protected override UsageSynopsisParseResult ParseUsageSynopsis(string[] commandPath, string helpText)
    {
        var arguments = ExtractSections(helpText, "FLAGS", "REQUIRED FLAGS", "OPTIONAL FLAGS", "POSITIONAL ARGUMENTS")
            .SelectMany(section => ParseArgumentGroups(section.Content, ParseGcloudArgument).FlattenArguments())
            .Where(argument => !string.IsNullOrEmpty(argument.ValueHint))
            .DistinctBy(argument => (argument.SwitchName, argument.ValueHint))
            .OrderByDescending(argument => argument.ValueHint!.Length)
            .ToArray();
        foreach (var (_, synopsis) in ExtractSections(helpText, "SYNOPSIS"))
        {
            var normalized = synopsis;
            foreach (var argument in arguments)
            {
                // The declared value grammar can wrap across lines. Its brackets and
                // spaces describe an option value, not additional positional operands.
                var valuePattern = string.Concat(argument.ValueHint!.Select(character => char.IsWhiteSpace(character)
                    ? @"\s+" : Regex.Escape(character.ToString()) + @"\s*"));
                normalized = Regex.Replace(normalized,
                    @"(?<![\w-])" + Regex.Escape(argument.SwitchName) + "=" + valuePattern + @"(?![\w])",
                    argument.SwitchName + "=VALUE ");
            }
            helpText = helpText.Replace(synopsis, normalized, StringComparison.Ordinal);
        }
        return base.ParseUsageSynopsis(commandPath, helpText);
    }

    protected override UsageSynopsisParseResult NormalizeUsageSynopsis(
        CliCommandDefinition command, UsageSynopsisParseResult usage)
    {
        // This synopsis placeholder denotes inherited flags, not a positional operand.
        var operands = usage.PositionalArguments
            .Where(argument => argument.PropertyName != "GcloudWideFlag")
            .ToList();
        return usage with
        {
            PositionalArguments = operands,
            HasOperandTokens = operands.Count > 0 || usage.UnparsedOperandTokens.Count > 0,
        };
    }

    /// <summary>
    /// gcloud can split flags into required and optional sections.
    /// </summary>
    protected override bool HasOptions(string helpText)
    {
        return ExtractSections(helpText, "FLAGS", "REQUIRED FLAGS", "OPTIONAL FLAGS", "POSITIONAL ARGUMENTS").Any()
               || base.HasOptions(helpText);
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
        switchName.Equals("--external-ipv6-prefix-length", StringComparison.OrdinalIgnoreCase)
        || switchName.Contains("service-account", StringComparison.OrdinalIgnoreCase);

    #endregion

    #region Gcloud-Specific Parsing Helpers

    private static List<string> ExtractFromSection(string helpText, string sectionName)
    {
        var subcommands = new List<string>();

        foreach (var (_, content) in ExtractSections(helpText, sectionName))
        {
            // gcloud command names are indented with five spaces at line start.
            var matches = SubcommandPattern().Matches(content);
            foreach (Match match in matches)
            {
                var name = match.Groups[1].Value.Trim();
                if (!string.IsNullOrEmpty(name))
                {
                    subcommands.Add(name);
                }
            }
        }

        return subcommands;
    }

    private static IEnumerable<(string Name, string Content)> ExtractSections(string helpText, params string[] sectionNames)
    {
        var headings = SectionHeadingPattern().Matches(helpText);
        for (var index = 0; index < headings.Count; index++)
        {
            var heading = headings[index];
            if (!sectionNames.Contains(heading.Value.Trim(), StringComparer.Ordinal))
            {
                continue;
            }

            var start = heading.Index + heading.Length;
            var end = index + 1 < headings.Count ? headings[index + 1].Index : helpText.Length;
            yield return (heading.Value.Trim(), helpText[start..end]);
        }
    }

    [GeneratedRegex(@"^[A-Z][A-Z_ ]*[ \t]*\r?$", RegexOptions.Multiline)]
    private static partial Regex SectionHeadingPattern();

    private static string? ExtractDescription(string helpText)
    {
        // NAME section: "gcloud command - description"
        var match = CommandDescriptionPattern().Match(helpText);
        if (match.Success)
        {
            return match.Groups[1].Value.Trim().Replace("\n", " ").Replace("  ", " ");
        }
        return null;
    }

    private (List<CliOptionDefinition> Options, IReadOnlyList<CliArgumentGroup> ArgumentGroups,
        IReadOnlyList<CliRequiredAlternativeGroup> RequiredAlternativeGroups,
        IReadOnlyList<CliPositionalArgument> PositionalArguments) ParseArguments(
        string helpText,
        IReadOnlyList<string> commandParts,
        string[] commandPath,
        UsageSynopsisParseResult usage)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var argumentGroups = new List<CliArgumentGroup>();
        var sections = new List<(string Name, CliArgumentGroup Group)>();
        var requiredAlternativeGroups = new List<CliRequiredAlternativeGroup>();
        foreach (var (name, content) in ExtractSections(helpText, "FLAGS", "REQUIRED FLAGS", "OPTIONAL FLAGS", "POSITIONAL ARGUMENTS"))
        {
            var argumentGroup = ParseArgumentGroups(content,
                name == "POSITIONAL ARGUMENTS" ? ParseGcloudResourceArgument : ParseGcloudArgument);
            argumentGroups.Add(argumentGroup);
            sections.Add((name, argumentGroup));
            foreach (var argument in argumentGroup.FlattenArguments().Where(argument => !argument.IsPositional))
            {
                foreach (var option in CreateOptions(argument, commandParts, helpText))
                {
                    if (!seenOptions.Add(option.SwitchName))
                    {
                        var existingIndex = options.FindIndex(existing => existing.SwitchName.Equals(option.SwitchName, StringComparison.OrdinalIgnoreCase));
                        if (options[existingIndex].IsGeneratedNegation && !option.IsGeneratedNegation)
                        {
                            options[existingIndex] = NormalizeRepeatability(option, helpText, commandParts, argument.Description);
                        }

                        continue;
                    }

                    options.Add(NormalizeRepeatability(option, helpText, commandParts, argument.Description));
                }
            }
        }

        var positionalArguments = ParsePositionalArguments(usage, commandPath, argumentGroups, options);
        foreach (var (name, argumentGroup) in sections)
        {
            ApplyRequiredGroups(argumentGroup, options, positionalArguments, requiredAlternativeGroups,
                name == "REQUIRED FLAGS", allowPresenceRequirements: name != "OPTIONAL FLAGS");
        }

        return (options, argumentGroups, requiredAlternativeGroups, positionalArguments);
    }

    private static void ApplyRequiredGroups(
        CliArgumentGroup group,
        List<CliOptionDefinition> options,
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        List<CliRequiredAlternativeGroup> requiredAlternativeGroups,
        bool required,
        bool allowPresenceRequirements)
    {
        // A group introduced by "Or" is conditional on selecting that alternative.
        if (group.Kind.HasFlag(CliArgumentGroupKind.Alternative)
            && group.Description?.TrimStart().StartsWith("Or ", StringComparison.OrdinalIgnoreCase) == true)
        {
            return;
        }

        if (group.Kind.HasFlag(CliArgumentGroupKind.AtLeastOne)
            || group.Kind.HasFlag(CliArgumentGroupKind.AtMostOne))
        {
            var constraint = CreateAlternativeConstraint(group, options, positionalArguments);
            requiredAlternativeGroups.Add(constraint with
            {
                IsRequired = allowPresenceRequirements && constraint.IsRequired,
            });
            return;
        }

        if (allowPresenceRequirements
            && (required || DescribesRequiredBundle(group)))
        {
            if (ApplyMandatoryGroup(group, options, positionalArguments, requiredAlternativeGroups, required))
            {
                return;
            }
        }
        else if (group.Arguments.Any(ArgumentIsConditionallyRequired))
        {
            // Optional bundles still require their mandatory members when any member is supplied.
            requiredAlternativeGroups.Add(CreateAlternativeConstraint(group, options, positionalArguments) with { IsRequired = false });
            return;
        }

        foreach (var nested in group.Groups)
        {
            var inheritsRequiredness = required && IsOrdinaryArgumentBundle(nested);
            ApplyRequiredGroups(nested, options, positionalArguments, requiredAlternativeGroups, inheritsRequiredness, allowPresenceRequirements);
        }
    }

    private static bool DescribesRequiredBundle(CliArgumentGroup group) =>
        CliArgumentGroupParser.DescribesRequiredBundle(group.Description);

    private static bool IsOrdinaryArgumentBundle(CliArgumentGroup group) =>
        group.Kind == CliArgumentGroupKind.None
        && !DescribesRequiredBundle(group)
        && !group.Arguments.Any(ArgumentIsConditionallyRequired);

    private static bool ApplyMandatoryGroup(
        CliArgumentGroup group,
        List<CliOptionDefinition> options,
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        List<CliRequiredAlternativeGroup> requiredAlternativeGroups,
        bool requireAllArguments)
    {
        if (!requireAllArguments && !group.Kind.HasFlag(CliArgumentGroupKind.Resource)
            && !group.Arguments.Any(ArgumentIsConditionallyRequired))
        {
            // A mandatory bundle still needs input when no individual member is mandatory.
            requiredAlternativeGroups.Add(CreateAlternativeConstraint(group, options, positionalArguments) with { IsRequired = true });
            return true;
        }

        ApplyRequiredArguments(group, options, positionalArguments, requiredAlternativeGroups, requireAllArguments);
        return false;
    }

    private static CliRequiredAlternativeGroup CreateAlternativeConstraint(
        CliArgumentGroup group,
        IReadOnlyList<CliOptionDefinition> options,
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        var isChoice = group.Kind.HasFlag(CliArgumentGroupKind.AtLeastOne)
                       || group.Kind.HasFlag(CliArgumentGroupKind.AtMostOne);
        var members = new List<CliRequiredAlternativeMember>();
        var nestedGroups = group.Groups.Select(nested => CreateAlternativeConstraint(nested, options, positionalArguments)).ToList();
        foreach (var argument in group.Arguments)
        {
            var argumentMembers = GetRequiredAlternativeMembers(argument, options, positionalArguments).ToArray();
            var required = ArgumentIsRequiredInGroup(group, argument);
            if (argumentMembers.Length > 1)
            {
                // Positive and negated forms are one exclusive branch in any containing group.
                nestedGroups.Add(new CliRequiredAlternativeGroup
                {
                    IsRequired = required,
                    IsMutuallyExclusive = true,
                    Members = argumentMembers,
                });
            }
            else
            {
                members.AddRange(argumentMembers.Select(member => member with { IsRequired = required }));
            }
        }

        return new CliRequiredAlternativeGroup
        {
            IsRequired = group.Kind.HasFlag(CliArgumentGroupKind.AtLeastOne)
                         || DescribesRequiredBundle(group),
            IsChoice = isChoice,
            IsMutuallyExclusive = group.Kind.HasFlag(CliArgumentGroupKind.AtMostOne),
            Members = members,
            Groups = nestedGroups,
        };
    }

    private static bool ArgumentIsRequiredInGroup(CliArgumentGroup group, CliArgumentDefinition argument) =>
        (group.Kind.HasFlag(CliArgumentGroupKind.Resource) && group.Arguments.Count == 1)
        || ArgumentIsConditionallyRequired(argument);

    private static bool ArgumentIsConditionallyRequired(CliArgumentDefinition argument) =>
        argument.Description is { } description && (description.Contains(
            "This flag argument must be specified if any of the other arguments in this group are specified.",
            StringComparison.OrdinalIgnoreCase)
            || description.Contains(
                "This positional argument must be specified if any of the other arguments in this group are specified.",
                StringComparison.OrdinalIgnoreCase));

    private static void ApplyRequiredArguments(
        CliArgumentGroup group,
        List<CliOptionDefinition> options,
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        List<CliRequiredAlternativeGroup> requiredAlternativeGroups,
        bool requireAllArguments)
    {
        var isResource = group.Kind.HasFlag(CliArgumentGroupKind.Resource);
        foreach (var argument in group.Arguments)
        {
            // Mandatory groups can contain optional settings. Only explicitly mandatory
            // members (or a resource's sole selector) inherit the group's requirement.
            if ((!requireAllArguments || isResource)
                && !ArgumentIsRequiredInGroup(group, argument))
            {
                continue;
            }

            var members = GetRequiredAlternativeMembers(argument, options, positionalArguments).ToArray();
            if (argument.IsPositional)
            {
                if (members.Any(member => positionalArguments.Any(positional =>
                        positional.PropertyName == member.PropertyName && !positional.IsRequired)))
                {
                    requiredAlternativeGroups.Add(new CliRequiredAlternativeGroup { Members = members });
                }

                continue;
            }

            var index = options.FindIndex(option => option.SwitchName == argument.SwitchName);
            if (index >= 0)
            {
                if (options[index].IsFlag || members.Length > 1)
                {
                    // Require an emitted flag or one of the documented positive/negated forms.
                    requiredAlternativeGroups.Add(new CliRequiredAlternativeGroup
                    {
                        IsMutuallyExclusive = members.Length > 1,
                        Members = members,
                    });
                }
                else
                {
                    options[index] = options[index] with { IsRequired = true };
                }
            }
        }
    }

    private static IEnumerable<CliRequiredAlternativeMember> GetRequiredAlternativeMembers(
        CliArgumentDefinition argument,
        IReadOnlyList<CliOptionDefinition> options,
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        if (argument.IsPositional)
        {
            var propertyName = NormalizePropertyName(argument.SwitchName);
            return positionalArguments.Where(positional => positional.PropertyName == propertyName)
                .Select(positional => new CliRequiredAlternativeMember
                {
                    PropertyName = positional.PropertyName,
                    PositionalArgumentPhase = positional.Phase,
                    PositionalArgumentPositionIndex = positional.PositionIndex,
                });
        }

        return options.Where(option => option.SwitchName == argument.SwitchName
                || (option.IsGeneratedNegation && option.IsFlag && option.SwitchName == $"--no-{argument.SwitchName[2..]}"))
            .Select(option => new CliRequiredAlternativeMember
            {
                OptionSwitch = option.SwitchName,
                PropertyName = option.PropertyName,
            });
    }

    private CliOptionDefinition NormalizeRepeatability(
        CliOptionDefinition option,
        string helpText,
        IReadOnlyList<string> commandParts,
        string? optionDescription)
    {
        if (option.IsFlag
            || option.CSharpType is "bool" or "bool?"
            || option.AcceptsMultipleValues
            || ShouldTreatOptionAsScalar(commandParts, option.SwitchName)
            || !HelpDeclaresRepeatableOption(
                helpText,
                option.SwitchName,
                optionDescription ?? string.Empty))
        {
            return option;
        }

        return option with
        {
            AcceptsMultipleValues = true,
            CSharpType = option.IsKeyValue
                ? option.CSharpType
                : AsCSharpType(option.CSharpType, acceptsMultipleValues: true),
        };
    }

    private IEnumerable<CliOptionDefinition> CreateOptions(
        CliArgumentDefinition argument,
        IReadOnlyList<string> commandParts,
        string helpText)
    {
        var longForm = argument.SwitchName;
        if (string.IsNullOrEmpty(longForm))
        {
            yield break;
        }

        var propertyName = NormalizePropertyName(longForm);
        if (propertyName is null)
        {
            yield break;
        }

        var option = CreateOptionDefinition(argument, longForm, propertyName, commandParts, helpText);
        yield return option;

        var negativeSwitch = $"--no-{longForm[2..]}";
        if (argument.IsNegatable
            || DescriptionMentionsSwitch(argument.Documentation, negativeSwitch))
        {
            yield return CreateNegatedOption(option, negativeSwitch, argument.Documentation);
        }
    }

    private CliOptionDefinition CreateOptionDefinition(
        CliArgumentDefinition argument,
        string longForm,
        string propertyName,
        IReadOnlyList<string> commandParts,
        string helpText)
    {
        var valueHint = argument.ValueHint ?? string.Empty;
        var description = argument.Documentation;
        var isFlag = string.IsNullOrEmpty(valueHint) || argument.IsNegatable;
        var hasCompositeSyntax = IsCompositeValueHint(valueHint);
        var isStructuredValue = hasCompositeSyntax
                                || DescriptionDeclaresStructuredValue(description);
        var isKeyValue = IsKeyValue(valueHint, isStructuredValue);
        var (acceptsMultipleValues, isDelimitedList) = GetCollectionBehavior(
            argument, commandParts, helpText, isFlag, hasCompositeSyntax, isStructuredValue, isKeyValue);
        var isNumeric = IsNumericValue(longForm, valueHint, description, isStructuredValue)
                        && !DurationDescriptionPattern().IsMatch(argument.Description ?? string.Empty);
        var enumDefinition = isStructuredValue ? null : TryDetectEnum(propertyName, description);

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
            Description = AddDelimitedListGuidance(description, isDelimitedList, isNumeric, enumDefinition),
            ValueShapeDescription = argument.Description ?? string.Empty,
            IsFlag = isFlag,
            IsRequired = false,
            AcceptsMultipleValues = acceptsMultipleValues,
            CollectionSeparator = isDelimitedList ? "," : null,
            IsKeyValue = isKeyValue,
            IsNumeric = isNumeric,
            ValueSeparator = isFlag ? " " : "=",
            EnumDefinition = enumDefinition,
            IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag, description)
        };
    }

    private (bool AcceptsMultipleValues, bool IsDelimitedList) GetCollectionBehavior(
        CliArgumentDefinition argument,
        IReadOnlyList<string> commandParts,
        string helpText,
        bool isFlag,
        bool hasCompositeSyntax,
        bool isStructuredValue,
        bool isKeyValue)
    {
        var valueHint = argument.ValueHint ?? string.Empty;
        var isKnownScalar = ShouldTreatOptionAsScalar(commandParts, argument.SwitchName);
        var repeatsSwitch = !isFlag && (DescriptionDeclaresRepeatedSwitch(argument.Description, argument.SwitchName)
            || GroupRepeatedSwitchDescriptionPattern().IsMatch(argument.GroupDescription ?? string.Empty)
            || HelpOptionBlockMatches(helpText, argument.SwitchName,
                block => DescriptionDeclaresRepeatedSwitch(block, argument.SwitchName)));
        var isDelimitedList = UsesCommaSeparatedList(
            valueHint, argument.Description, isFlag, isStructuredValue, isKeyValue, isKnownScalar, repeatsSwitch);
        var acceptsMultipleValues = isDelimitedList
                                    || (!isKnownScalar && repeatsSwitch)
                                    || ((!isKnownScalar || valueHint.Contains("...", StringComparison.Ordinal))
                                        && AcceptsMultipleValues(argument.SwitchName, valueHint, argument.Description, isFlag, hasCompositeSyntax));
        return (acceptsMultipleValues, isDelimitedList);
    }

    private static bool DescriptionDeclaresRepeatedSwitch(string? description, string switchName) =>
        RepeatedSwitchDescriptionPattern().IsMatch(description ?? string.Empty)
        || NamedRepeatedSwitchDescriptionPattern().Matches(description ?? string.Empty)
            .Any(match => match.Groups["switch"].Value.Equals(switchName, StringComparison.OrdinalIgnoreCase));

    private static bool UsesCommaSeparatedList(
        string valueHint,
        string? description,
        bool isFlag,
        bool isStructuredValue,
        bool isKeyValue,
        bool isKnownScalar,
        bool repeatsSwitch) =>
        !isFlag
        && (!isStructuredValue || isKeyValue || RepeatedPairValueHintPattern().IsMatch(valueHint))
        && !isKnownScalar
        // Repeating a switch preserves boundaries between structured records. Merely
        // accepting multiple values does not distinguish a list from repeated switches.
        && !repeatsSwitch
        && ((valueHint.Contains(',') && valueHint.Contains("...", StringComparison.Ordinal))
            || CommaSeparatedListDescriptionPattern().IsMatch(description ?? string.Empty));

    private static string? AddDelimitedListGuidance(
        string? description,
        bool isDelimitedList,
        bool isNumeric,
        CliEnumDefinition? enumDefinition)
    {
        if (!isDelimitedList || isNumeric || enumDefinition is not null)
        {
            return description;
        }

        const string guidance = "Collection entries are joined with commas into one option value. "
            + "For entries containing commas, supply one pre-escaped list value using gcloud topic escaping "
            + "(https://cloud.google.com/sdk/gcloud/reference/topic/escaping).";
        return string.IsNullOrWhiteSpace(description) ? guidance : $"{description} {guidance}";
    }

    private static bool AcceptsMultipleValues(
        string switchName,
        string valueHint,
        string? description,
        bool isFlag,
        bool hasCompositeSyntax) =>
        !isFlag
        && (DescriptionDeclaresValueList(description)
            || DescriptionDeclaresRepeatedSwitch(description, switchName)
            || DescriptionDeclaresRepeatableOption(description ?? string.Empty)
            || (!hasCompositeSyntax && valueHint.Contains("..."))
            || DescriptionRepeatsStructuredOption(description, switchName));

    private static bool IsNumericValue(
        string switchName,
        string valueHint,
        string? description,
        bool isStructuredValue) =>
        !IsTextualIdentifierOption(switchName)
        && !IsFilePathHint(switchName, valueHint)
        && !isStructuredValue
        && !DescriptionDeclaresTextualCategories(description)
        && IsNumericHint(valueHint);

    private static bool IsKeyValue(string valueHint, bool isStructuredValue) =>
        valueHint.Contains("KEY=VALUE")
        || (!isStructuredValue && valueHint.Contains("=VALUE,"));

    private static CliOptionDefinition CreateNegatedOption(
        CliOptionDefinition option,
        string negativeSwitch,
        string? description)
    {
        var propertyName = $"No{option.PropertyName}";
        return option with
        {
            SwitchName = negativeSwitch,
            PropertyName = propertyName,
            CSharpType = "bool?",
            Description = $"Negates {option.SwitchName}. {description}",
            IsGeneratedNegation = true,
            IsFlag = true,
            ValueArity = CliOptionValueArity.Required,
            AcceptsMultipleValues = false,
            GroupValues = false,
            CollectionSeparator = null,
            IsCollection = false,
            IsKeyValue = false,
            IsNumeric = false,
            IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag: true),
            SecretValueKeys = [],
            ValueSeparator = " ",
            EnumDefinition = null,
            ValidationConstraints = null,
        };
    }

    private static bool DescriptionMentionsSwitch(string? description, string switchName) =>
        !string.IsNullOrEmpty(description)
        && Regex.IsMatch(
            description,
            $@"(?<![\w-]){Regex.Escape(switchName)}(?![\w-])",
            RegexOptions.IgnoreCase);

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
            // Same tab-aware units as the CliArgumentGroupParser comparisons.
            Indentation = GetIndentation(match.Groups["indent"].Value),
        };
    }

    private static CliArgumentDefinition? ParseGcloudResourceArgument(string line)
    {
        if (ParseGcloudArgument(line) is { } option)
        {
            return option;
        }

        var match = ResourceOperandPattern().Match(line);
        if (!match.Success)
        {
            return null;
        }

        return new CliArgumentDefinition
        {
            SwitchName = UsageSynopsisParser.GetOperandPropertyName(match.Groups["operand"].Value)!,
            IsPositional = true,
            ValueHint = line.Trim(),
            Indentation = GetIndentation(line),
        };
    }

    [GeneratedRegex(@"^[ \t]+\[?(?:--[ \t]+)?(?<operand>(?:(?:\[[^\s]+\]|\([^()\s]+\)):?)?(?<name>[A-Z][A-Z0-9_/-]*))(?:[ \t]+\[?\k<operand>)?(?:[ \t]*\.\.\.)?\]*[ \t]*$")]
    private static partial Regex ResourceOperandPattern();

    private IReadOnlyList<CliPositionalArgument> ParsePositionalArguments(
        UsageSynopsisParseResult usage, string[] commandPath, IReadOnlyList<CliArgumentGroup> groups,
        IReadOnlyList<CliOptionDefinition> options)
    {
        var usageArguments = GetPositionalArguments(usage, options);
        var arguments = groups.SelectMany(group => group.FlattenArguments())
            .Where(argument => argument.IsPositional)
            .Select((argument, index) =>
            {
                var propertyName = NormalizePropertyName(argument.SwitchName)!;
                var synopsisArgument = usageArguments.FirstOrDefault(candidate =>
                    candidate.PropertyName.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
                if (synopsisArgument is null && usage.HasExtractedSynopses)
                {
                    throw new InvalidOperationException(
                        $"{string.Join(" ", commandPath)} synopsis omits declared positional operand '{argument.SwitchName}'; "
                        + "its order and requiredness cannot be inferred safely.");
                }

                var variadic = argument.ValueHint?.Contains("...", StringComparison.Ordinal) == true;
                var required = argument.ValueHint?.StartsWith('[') != true;
                return (synopsisArgument ?? new CliPositionalArgument
                {
                    PropertyName = propertyName,
                    CSharpType = (variadic ? "IEnumerable<string>" : "string") + (required ? "" : "?"),
                    PositionIndex = index,
                    IsRequired = required,
                    IsVariadic = variadic,
                }) with
                { Description = argument.Documentation };
            })
            .OrderBy(argument => argument.PositionIndex);
        return CliPositionalArgument.MergeDuplicates(arguments);
    }

    private static bool IsNumericHint(string hint)
        => NumericHintPattern().IsMatch(hint);

    private static bool IsTextualIdentifierOption(string switchName)
        => switchName.Equals("--project", StringComparison.OrdinalIgnoreCase)
            || switchName.Equals("--billing-account", StringComparison.OrdinalIgnoreCase)
            || (switchName.Contains("service-account", StringComparison.OrdinalIgnoreCase)
                && !switchName.EndsWith("-project-number", StringComparison.OrdinalIgnoreCase));

    private static bool IsCompositeValueHint(string valueHint)
        => (valueHint.StartsWith('[') && valueHint.Contains('='))
            || valueHint.Count(character => character == '=') > 1;

    private static bool IsFilePathHint(string switchName, string valueHint)
        => switchName.EndsWith("-file", StringComparison.OrdinalIgnoreCase)
            || switchName.EndsWith("-path", StringComparison.OrdinalIgnoreCase)
            || FilePathHintPattern().IsMatch(valueHint);

    private static bool DescriptionDeclaresTextualCategories(string? description)
        => !string.IsNullOrEmpty(description)
            && TextualCategoriesPattern().IsMatch(description);

    private static bool DescriptionDeclaresStructuredValue(string? description)
        => description?.Contains("Shorthand Example:", StringComparison.OrdinalIgnoreCase) is true
           || description?.Contains("JSON Example:", StringComparison.OrdinalIgnoreCase) is true
           || description?.Contains("YAML Example:", StringComparison.OrdinalIgnoreCase) is true
           || description?.Contains("File Example:", StringComparison.OrdinalIgnoreCase) is true;

    private static bool DescriptionRepeatsStructuredOption(string? description, string switchName)
    {
        if (string.IsNullOrEmpty(description))
        {
            return false;
        }

        const string shorthandMarker = "Shorthand Example:";
        var shorthandStart = description.IndexOf(shorthandMarker, StringComparison.OrdinalIgnoreCase);
        if (shorthandStart < 0)
        {
            return false;
        }

        shorthandStart += shorthandMarker.Length;
        var exampleEnd = StructuredExampleEndMarkers
            .Select(marker => description.IndexOf(marker, shorthandStart, StringComparison.OrdinalIgnoreCase))
            .Where(index => index >= 0)
            .DefaultIfEmpty(description.Length)
            .Min();
        var shorthandExample = description[shorthandStart..exampleEnd];

        return Regex.Count(
            shorthandExample,
            $@"(?<![\w-]){Regex.Escape(switchName)}=",
            RegexOptions.IgnoreCase) > 1;
    }

    private static bool DescriptionDeclaresValueList(string? description)
        => description?.StartsWith("List of ", StringComparison.OrdinalIgnoreCase) is true;

    private static CliEnumDefinition? TryDetectEnum(string propertyName, string? description)
    {
        if (string.IsNullOrEmpty(description))
        {
            return null;
        }

        // Pattern 1: "OPTION must be one of: value1, value2, value3" (comma-separated list)
        var match = RequiredEnumValuesPattern().Match(description);
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
        match = InlineEnumValuesPattern().Match(description);
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
            Values = [.. values.Select(v => new CliEnumValue
            {
                MemberName = string.Join("", v.Split(['-', '_'], StringSplitOptions.RemoveEmptyEntries).Select(ToPascalCase)),
                CliValue = v
            })],
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
            return AsCSharpType($"{enumDef.EnumName}?", acceptsMultipleValues);
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

    private const string RepeatableSwitchRegex =
        @"(?:repeatable"
        + @"|repeat\s+to\s+add\s+more"
        + @"|repeat\s+or\s+comma-separate\s+for\s+multiple"
        + @"|(?:(?:can|must|should)\s+be|may\s*be)\s+repeated"
        + @"|(?:is|are)\s+(?:(?:a|an)\s+)?(?:repeatable|repeated)"
        + @"|repeated\s+(?:flag|argument|option)"
        + @"|(?:multiples?|multiple\s+[\w-]+)\s+(?:are\s+)?supported\s+by\s+passing\s+"
        + @"(?:--?[\w-]+|(?<exampleQuote>['""`])--?[\w-]+(?:\s+[^'""`\r\n]+)?\k<exampleQuote>)\s+multiple\s+times"
        + @"|(?:(?:can|must|should)\s+be|may\s*be|is)\s+"
        + @"(?:specified|supplied|provided|used|passed|set|given)\s+"
        + @"(?:(?:one|zero)\s+or\s+more\s+times|multiple\s+times|more\s+than\s+once)"
        + @"|(?:to\s+[^.!?;\r\n]+,\s*)?(?:specify|supply|provide|use|pass|set|give)\s+"
        + @"(?:this|the)\s+(?:flag|argument|option)\s+(?:multiple\s+times|more\s+than\s+once)"
        + @"|(?:accepts?|specify|supply|provide|use|pass|set|give|supports?|takes?|contains?)\s+"
        + @"(?:multiple\s+times|more\s+than\s+once))";

    private const string StatusPrefixPattern = @"(?:\((?:DEPRECATED|ALPHA|BETA)\)\s+)*";

    [GeneratedRegex(@"(?:^|[.!?]\s+)\s*" + StatusPrefixPattern
        + @"(?:(?:this|the)\s+)?(?:(?:flag|argument|option)\s+)?"
        + RepeatableSwitchRegex + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex RepeatedSwitchDescriptionPattern();

    [GeneratedRegex(@"(?:^|[.!?]\s+)\s*(?:these|the following)\s+(?:flags|arguments|options)\s+"
        + RepeatableSwitchRegex + @"\b", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex GroupRepeatedSwitchDescriptionPattern();

    [GeneratedRegex(@"(?:^|[.!?]\s+)\s*" + StatusPrefixPattern
        + @"(?:(?:to\s+[^.!?;\r\n]+,\s*)?(?:specify|supply|provide|use|pass|set|give)\s+"
        + @"(?:the\s+)?(?<switch>--?[\w-]+)\s+(?:(?:flag|argument|option)\s+)?"
        + @"(?:multiple\s+times|more\s+than\s+once)"
        + @"|(?:the\s+)?(?<switch>--?[\w-]+)\s+(?:(?:flag|argument|option)\s+)?"
        + RepeatableSwitchRegex + @")\b", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex NamedRepeatedSwitchDescriptionPattern();

    [GeneratedRegex(@"(?:^|[.!?]\s+)" + StatusPrefixPattern
        + @"(?:(?:(?:at (?:most|least)|exactly) one) of these (?:can|must) be specified:\s+)*"
        + @"(?:(?:(?:this|the)\s+(?:flag|option|argument|value)|(?:the\s+)?string\s+value)\s+"
        + @"(?:must|should|can|may)\s+(?:follow|use|be)\s+(?:(?!--)[^.!?:])*:\s*)?"
        + @"(?:(?:this|the)\s+(?:flag|argument|option|value)\s+|(?:this|it)\s+)?"
        + @"(?:(?:is|accepts?|expects?|specif(?:y|ies)|takes?|contains?|(?:must|can|may)\s+be)\s+)?"
        + @"(?:(?:a|the)\s+)?(?:single\s+[\w-]+(?:\s+[\w-]+)*\s+or\s+(?:a\s+)?)?"
        + @"comma[- ](?:sep[ae]rated|delimited)\s+(?:list|string)\b"
        // Unqualified subjects describe the option only in its opening sentence. Later
        // sentences may describe fields inside a configuration file instead.
        + @"|(?:^|[.!?]\s+(?:this|the)\s+(?:flag|argument|option)\s+)"
        + @"(?:(?!--)[^.!?])*?[,([]\s*sep[ae]rated\s+by\s+commas\b"
        // A repeated subject links the passive sentence to the option's string value.
        + @"|^" + StatusPrefixPattern + @"(?:a|the)\s+string\s+of\s+(?:[\w-]+\s+)*(?<subject>[\w-]+)\.\s+"
        + @"\k<subject>\s+are\s+sep[ae]rated\s+by\s+commas\b"
        + @"|(?:^|[.!?]\s+)" + StatusPrefixPattern + @"(?:provide|supply|pass|specify|use)\s+"
        + @"(?:[a-z][\w-]*\s+)+as\s+(?:a\s+)?comma[- ](?:sep[ae]rated|delimited)\s+(?:list|string)\b"
        + @"|(?:^|[.!?]\s+)" + StatusPrefixPattern + @"use\s+commas\s+to\s+(?:separate|delimit)\s+"
        + @"(?:(?:the|individual|multiple)\s+)?[a-z][\w-]*(?=\s*(?:[.!?]|$))", RegexOptions.IgnoreCase)]
    private static partial Regex CommaSeparatedListDescriptionPattern();

    [GeneratedRegex(@"^(?<outer>\[)?(?<key>[A-Z][A-Z0-9_-]*)=(?<value>[A-Z][A-Z0-9_-]*),(?:\[\k<key>=\k<value>,\.{3}\]|\.{3})(?(outer)\])$")]
    private static partial Regex RepeatedPairValueHintPattern();

    /// <summary>
    /// Matches gcloud flag patterns:
    /// --flag
    /// --[no-]flag
    /// --option=VALUE
    /// Default annotations are display text and may contain spaces, such as Python enum representations.
    /// </summary>
    [GeneratedRegex(
        @"^(?<indent>[ \t]+)(?:(?<negatable>--\[no-\])(?<negatableName>\w[\w-]*)|(?<long>--\w[\w-]*))(?:=(?<value>[^\r\n;]+?))?(?:,\s*-[\w-]+(?:[ =]\S+)?)?(?:;\s*default=[^\r\n]+)?$")]
    private static partial Regex GcloudFlagPattern();

    [GeneratedRegex(
        @"(?<![A-Za-z0-9])(?:counts?|numbers?|sizes?|timeouts?|seconds|iops)(?![A-Za-z0-9])",
        RegexOptions.IgnoreCase)]
    private static partial Regex NumericHintPattern();

    [GeneratedRegex(@"\bdurations?\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex DurationDescriptionPattern();

    [GeneratedRegex(
        @"(?<![A-Za-z0-9])(?:file|filename|filepath|path)(?![A-Za-z0-9])",
        RegexOptions.IgnoreCase)]
    private static partial Regex FilePathHintPattern();

    [GeneratedRegex(
        @"\bmust be one of:\s*[A-Za-z][A-Za-z0-9_-]*\b",
        RegexOptions.IgnoreCase)]
    private static partial Regex TextualCategoriesPattern();
    [GeneratedRegex(@"^\s{5}(\w[\w-]*)\s*$", RegexOptions.Multiline)]
    private static partial Regex SubcommandPattern();

    [GeneratedRegex(@"^NAME\s*\n\s+gcloud[^\n]+-\s*(.+?)(?=\n\n|\nSYNOPSIS)", RegexOptions.Singleline)]
    private static partial Regex CommandDescriptionPattern();

    [GeneratedRegex(@"must be (?:one of:?\s*)([a-zA-Z][a-zA-Z0-9_-]*(?:,\s*[a-zA-Z][a-zA-Z0-9_-]*)+)", RegexOptions.IgnoreCase)]
    private static partial Regex RequiredEnumValuesPattern();

    [GeneratedRegex(@";\s*one of\s+([a-zA-Z][a-zA-Z0-9_-]*(?:,\s*[a-zA-Z][a-zA-Z0-9_-]*)+)", RegexOptions.IgnoreCase)]
    private static partial Regex InlineEnumValuesPattern();

    #endregion
}
