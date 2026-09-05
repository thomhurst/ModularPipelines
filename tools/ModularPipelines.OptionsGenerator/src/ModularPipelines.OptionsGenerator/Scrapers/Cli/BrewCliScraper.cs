using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Homebrew CLI.
/// Homebrew runs on macOS and Linux.
///
/// Homebrew help format:
/// Example usage:
///   brew search TEXT|/REGEX/
///   brew info [FORMULA|CASK...]
///   brew install FORMULA|CASK...
///   brew update
///   brew upgrade [FORMULA|CASK...]
///
/// Commands:
///   install             Install a formula or cask.
///   ...
///
/// Subcommand help (brew install --help):
/// Usage: brew install [options] formula|cask [...]
///
/// Install a formula or cask.
///
///       --formula, --formulae    Treat all named arguments as formulae.
///       --cask, --casks          Treat all named arguments as casks.
///   -d, --debug                  Display any debugging information.
/// </summary>
public partial class BrewCliScraper : CliScraperBase
{
    private const string OptionTypeMarker = "MODULARPIPELINES_BREW_OPTION_TYPE";
    private const string OptionMetadataScript =
        "require 'commands';require 'cli/parser';c=ARGV.shift;s=ARGV.shift;" +
        "p=Homebrew::CLI::Parser.from_cmd_path(Commands.path(c));" +
        "abort 'parser unavailable' if p.nil?;" +
        "t=p.instance_variable_get(:@option_types);" +
        "t.each{|n,k|puts [n,k].join(9.chr) if p.send(:option_allowed_for_subcommand?,n,s)}";

    public BrewCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<BrewCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "brew";

    public override string NamespacePrefix => "Brew";

    public override string TargetNamespace => "ModularPipelines.Homebrew";

    public override string OutputDirectory => "src/ModularPipelines.Homebrew";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "shellenv", "commands", "analytics"
    };

    /// <summary>
    /// Homebrew is available on macOS and Linux, but not Windows.
    /// </summary>
    public override async Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Logger.LogDebug("Homebrew is not available on Windows");
            return false;
        }

        return await base.IsAvailableAsync(cancellationToken);
    }

    protected override async Task<string?> GetHelpTextAsync(
        string[] commandPath,
        CancellationToken cancellationToken)
    {
        var helpText = await base.GetHelpTextAsync(commandPath, cancellationToken);
        if (string.IsNullOrWhiteSpace(helpText))
        {
            return helpText;
        }

        if (commandPath.Length > 1)
        {
            return await AppendOptionTypeMetadataAsync(
                commandPath,
                helpText,
                cancellationToken);
        }

        if (CommandSectionPattern().IsMatch(helpText))
        {
            return helpText;
        }

        var commandInventory = await Executor.ExecuteAsync(
            ExecutablePath,
            "commands --quiet",
            cancellationToken);
        if (!commandInventory.Success)
        {
            Logger.LogWarning(
                "Could not query the complete Homebrew command inventory; brew commands --quiet exited with {ExitCode}",
                commandInventory.ExitCode);
            return helpText;
        }

        if (!string.IsNullOrWhiteSpace(commandInventory.StandardError))
        {
            Logger.LogWarning(
                "brew commands --quiet reported diagnostics: {StandardError}",
                commandInventory.StandardError.Trim());
        }

        var commands = commandInventory.StandardOutput
            .Split((char[]?) null, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(static command => BrewCommandNamePattern().IsMatch(command))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Order(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        if (commands.Length == 0)
        {
            Logger.LogWarning("brew commands --quiet returned no parseable commands");
            return helpText;
        }

        var commandSection = string.Join(
            Environment.NewLine,
            commands.Select(static command => $"  {command}  Discovered by brew commands --quiet."));
        return $"{helpText.TrimEnd()}{Environment.NewLine}{Environment.NewLine}Commands:{Environment.NewLine}{commandSection}";
    }

    private async Task<string> AppendOptionTypeMetadataAsync(
        IReadOnlyList<string> commandPath,
        string helpText,
        CancellationToken cancellationToken)
    {
        var command = commandPath[1];
        var subcommand = commandPath.Count > 2 ? $" {commandPath[2]}" : string.Empty;
        var escapedScript = OptionMetadataScript.Replace("\"", "\\\"", StringComparison.Ordinal);
        var result = await Executor.ExecuteAsync(
            ExecutablePath,
            $"ruby -e \"{escapedScript}\" -- {command}{subcommand}",
            cancellationToken);
        if (!result.Success || string.IsNullOrWhiteSpace(result.StandardOutput))
        {
            Logger.LogDebug(
                "Could not query Homebrew parser metadata for {Command}; using help-text fallback",
                string.Join(' ', commandPath));
            return helpText;
        }

        var metadataLines = result.StandardOutput
            .Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n')
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(static line => line.Count(static character => character == '\t') == 1)
            .Select(static line => $"{OptionTypeMarker}\t{line}")
            .ToArray();
        if (metadataLines.Length == 0)
        {
            return helpText;
        }

        return $"{helpText.TrimEnd()}{Environment.NewLine}{Environment.NewLine}{string.Join(Environment.NewLine, metadataLines)}";
    }

    // Fail closed: Homebrew can print another command's prerequisite help before failing.
    protected override bool ShouldAcceptHelpResult(
        IReadOnlyList<string> commandPath,
        CliCommandResult result) => result.Success;

    /// <summary>
    /// Homebrew command groups render child synopses without a Usage prefix.
    /// </summary>
    protected override IEnumerable<string> GetAdditionalUsageSynopses(
        string[] commandPath,
        string helpText)
    {
        var command = string.Join(' ', commandPath);
        var lines = NormalizeLines(helpText);

        for (var index = 0; index < lines.Length; index++)
        {
            var synopsis = lines[index].Trim();
            if (!StartsWithCommand(synopsis, command))
            {
                continue;
            }

            yield return ReadStandaloneSynopsis(lines, ref index, synopsis);
        }
    }

    private static bool StartsWithCommand(string synopsis, string command)
    {
        var commandStart = synopsis.StartsWith("[sudo] ", StringComparison.OrdinalIgnoreCase)
            ? "[sudo] ".Length
            : 0;
        var candidate = synopsis.AsSpan(commandStart);
        return candidate.StartsWith(command, StringComparison.OrdinalIgnoreCase)
               && (candidate.Length == command.Length
                   || char.IsWhiteSpace(candidate[command.Length]));
    }

    private static string ReadStandaloneSynopsis(
        IReadOnlyList<string> lines,
        ref int index,
        string firstLine)
    {
        var parts = new List<string> { firstLine };
        while (index + 1 < lines.Count
               && UsageSynopsisParser.IsSynopsisContinuation(lines[index + 1]))
        {
            parts.Add(lines[++index].Trim());
        }

        return string.Join(' ', parts);
    }

    /// <summary>
    /// Extracts subcommand names from Homebrew help text.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        var seenCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find "Commands:" or "Subcommands:" section
        var commandSectionMatch = CommandSectionPattern().Match(helpText);
        if (!commandSectionMatch.Success)
        {
            // Try alternate format - lines that look like "  commandname    description"
            return ExtractSubcommandsFromExampleUsage(helpText, seenCommands);
        }

        var sectionStart = commandSectionMatch.Index + commandSectionMatch.Length;

        // Find where this section ends (next section header ending with : or empty line followed by non-indented text)
        var sectionEnd = helpText.Length;
        var nextSectionMatch = NextSectionPattern().Match(helpText, sectionStart);
        if (nextSectionMatch.Success)
        {
            sectionEnd = nextSectionMatch.Index;
        }

        var section = helpText.Substring(sectionStart, sectionEnd - sectionStart);

        // Parse command lines: "  command             description"
        var lines = section.Split('\n');
        foreach (var line in lines)
        {
            // Match pattern: whitespace + command + whitespace + description
            var match = CommandLinePattern().Match(line);
            if (!match.Success)
            {
                match = ColonDelimitedCommandLinePattern().Match(line);
            }

            if (match.Success)
            {
                var commandName = match.Groups["name"].Value.Trim();
                if (!string.IsNullOrEmpty(commandName) && seenCommands.Add(commandName))
                {
                    subcommands.Add(commandName);
                }
            }
        }

        // Also check for commands in "Example usage" section
        var exampleCommands = ExtractSubcommandsFromExampleUsage(helpText, seenCommands);
        subcommands.AddRange(exampleCommands);

        return subcommands;
    }

    /// <summary>
    /// Extracts subcommands from "Example usage:" section.
    /// </summary>
    private IEnumerable<string> ExtractSubcommandsFromExampleUsage(string helpText, HashSet<string> seenCommands)
    {
        var subcommands = new List<string>();

        var exampleMatch = ExampleUsagePattern().Match(helpText);
        if (!exampleMatch.Success)
        {
            return subcommands;
        }

        var sectionStart = exampleMatch.Index + exampleMatch.Length;

        // Find end of example section
        var sectionEnd = helpText.Length;
        var nextSectionMatch = NextSectionPattern().Match(helpText, sectionStart);
        if (nextSectionMatch.Success)
        {
            sectionEnd = nextSectionMatch.Index;
        }

        var section = helpText.Substring(sectionStart, sectionEnd - sectionStart);
        var lines = section.Split('\n');

        foreach (var line in lines)
        {
            // Match: "  brew commandname ..."
            var match = BrewCommandLinePattern().Match(line);
            if (match.Success)
            {
                var commandName = match.Groups["name"].Value.Trim();
                if (!string.IsNullOrEmpty(commandName) && !commandName.StartsWith('-') && seenCommands.Add(commandName))
                {
                    subcommands.Add(commandName);
                }
            }
        }

        return subcommands;
    }

    /// <summary>
    /// Parses a Homebrew command from its help text.
    /// </summary>
    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken) =>
        ParseCommandAsync(
            commandPath,
            helpText,
            ParseUsageSynopsis(commandPath, helpText),
            cancellationToken);

    /// <inheritdoc />
    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        UsageSynopsisParseResult usage,
        CancellationToken cancellationToken)
    {
        var commandParts = commandPath.Skip(1).ToArray(); // Skip tool name

        if (commandParts.Length == 0)
        {
            // This is just the root command, skip it
            return Task.FromResult<CliCommandDefinition?>(null);
        }

        // Parse description from help text
        var description = ExtractDescription(helpText, usage);

        // Wrapper commands can print prerequisite help before their own usage.
        // Only options at or after the selected command synopsis belong here.
        var optionTypes = ParseOptionTypes(NormalizeLines(helpText));
        var options = ParseOptions(
            ExtractHelpFromMatchingUsage(helpText, usage),
            optionTypes);
        var positionalArguments = NormalizePositionalArguments(
            commandParts,
            DisambiguatePositionalArguments(
                GetPositionalArguments(usage, options),
                options));

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
            PositionalArguments = positionalArguments,
            SubDomainGroup = null,
            Enums = enums
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    private static IReadOnlyList<CliPositionalArgument> DisambiguatePositionalArguments(
        IReadOnlyList<CliPositionalArgument> positionalArguments,
        IReadOnlyList<CliOptionDefinition> options)
    {
        var usedPropertyNames = options
            .Select(static option => option.PropertyName)
            .ToHashSet(StringComparer.Ordinal);

        return positionalArguments
            .Select(argument =>
            {
                var propertyName = argument.PropertyName;
                while (!usedPropertyNames.Add(propertyName))
                {
                    propertyName += "Operand";
                }

                return propertyName == argument.PropertyName
                    ? argument
                    : argument with { PropertyName = propertyName };
            })
            .ToArray();
    }

    private static IReadOnlyList<CliPositionalArgument> NormalizePositionalArguments(
        IReadOnlyList<string> commandParts,
        IReadOnlyList<CliPositionalArgument> arguments) =>
        commandParts switch
        {
            ["command"] =>
            [
                RequiredArgument("Cmd", 0) with
                {
                    CSharpType = "IEnumerable<string>",
                    IsVariadic = true,
                },
            ],
            ["exec"] =>
            [
                RequiredArgument("Command", 0),
                VariadicArgument("Arguments", 1),
            ],
            ["sandbox-exec"] =>
            [
                RequiredArgument("Command", 0) with
                {
                    CSharpType = "IEnumerable<string>",
                    IsVariadic = true,
                    PrependOptionTerminator = true,
                },
            ],
            ["generate-zap"] =>
            [
                RequiredArgument("CaskOrName", 0),
            ],
            ["unlink"] =>
            [
                RequiredArgument("InstalledFormula", 0) with
                {
                    CSharpType = "IEnumerable<string>",
                    IsVariadic = true,
                },
            ],
            _ => arguments,
        };

    private static CliPositionalArgument RequiredArgument(string propertyName, int position) => new()
    {
        PropertyName = propertyName,
        CSharpType = "string",
        Description = $"The {propertyName.ToLowerInvariant()} operand.",
        Phase = CommandLinePhase.Passthrough,
        PositionIndex = position,
        IsRequired = true,
    };

    private static CliPositionalArgument VariadicArgument(string propertyName, int position) => new()
    {
        PropertyName = propertyName,
        CSharpType = "IEnumerable<string>?",
        Description = $"The {propertyName.ToLowerInvariant()} operands.",
        Phase = CommandLinePhase.Passthrough,
        PositionIndex = position,
        IsVariadic = true,
    };

    /// <summary>
    /// Extracts description from help text.
    /// </summary>
    private static string? ExtractDescription(
        string helpText,
        UsageSynopsisParseResult usage)
    {
        var lines = NormalizeLines(helpText);
        if (!TryFindMatchingUsage(lines, usage, out _, out var usageEndIndex))
        {
            return null;
        }

        return ReadDescriptionParagraph(lines, usageEndIndex);
    }

    private static bool TryMatchUsageSynopsis(
        IReadOnlyList<string> lines,
        ref int index,
        string expectedSynopsis)
    {
        var usageMatch = UsageLinePattern().Match(lines[index]);
        var synopsisStart = usageMatch.Success
            ? usageMatch.Groups["synopsis"].Value.Trim()
            : lines[index].Trim();
        if (!IsSynopsisPrefix(expectedSynopsis, synopsisStart))
        {
            return false;
        }

        var synopsisParts = new List<string> { synopsisStart };
        while (index + 1 < lines.Count
               && UsageSynopsisParser.IsSynopsisContinuation(lines[index + 1]))
        {
            synopsisParts.Add(lines[++index].Trim());
        }

        return string.Join(' ', synopsisParts).Equals(expectedSynopsis, StringComparison.Ordinal);
    }

    private static bool IsSynopsisPrefix(string expectedSynopsis, string candidate) =>
        !string.IsNullOrWhiteSpace(candidate)
        && expectedSynopsis.StartsWith(candidate, StringComparison.Ordinal)
        && (candidate.Length == expectedSynopsis.Length
            || char.IsWhiteSpace(expectedSynopsis[candidate.Length]));

    private static string? ReadDescriptionParagraph(
        IReadOnlyList<string> lines,
        int usageEndIndex)
    {
        var index = usageEndIndex;
        while (index + 1 < lines.Count && string.IsNullOrWhiteSpace(lines[index + 1]))
        {
            index++;
        }

        var descriptionLines = new List<string>();
        while (index + 1 < lines.Count && !string.IsNullOrWhiteSpace(lines[index + 1]))
        {
            var descriptionLine = lines[++index].Trim();
            if (BrewOptionPattern().IsMatch(lines[index]) || descriptionLine.EndsWith(':'))
            {
                break;
            }

            descriptionLines.Add(descriptionLine);
        }

        return descriptionLines.Count == 0 ? null : string.Join(' ', descriptionLines);
    }

    private static string ExtractHelpFromMatchingUsage(
        string helpText,
        UsageSynopsisParseResult usage)
    {
        var lines = NormalizeLines(helpText);
        if (!TryFindMatchingUsage(
                lines,
                usage,
                out var usageStartIndex,
                out var usageEndIndex))
        {
            return helpText;
        }

        var sectionEndIndex = lines.Length;
        for (var index = usageEndIndex + 1; index < lines.Length; index++)
        {
            if (UsageLinePattern().IsMatch(lines[index]))
            {
                sectionEndIndex = index;
                break;
            }
        }

        return string.Join(Environment.NewLine, lines[usageStartIndex..sectionEndIndex]);
    }

    private static bool TryFindMatchingUsage(
        IReadOnlyList<string> lines,
        UsageSynopsisParseResult usage,
        out int usageStartIndex,
        out int usageEndIndex)
    {
        usageStartIndex = -1;
        usageEndIndex = -1;
        if (!usage.CommandMatched || string.IsNullOrWhiteSpace(usage.Synopsis))
        {
            return false;
        }

        for (var index = 0; index < lines.Count; index++)
        {
            var candidateStartIndex = index;
            if (!TryMatchUsageSynopsis(lines, ref index, usage.Synopsis))
            {
                continue;
            }

            usageStartIndex = candidateStartIndex;
            usageEndIndex = index;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Parses options from Homebrew help text.
    /// Format variations:
    ///       --formula, --formulae    Treat all named arguments as formulae.
    ///   -d, --debug                  Display any debugging information.
    ///       --[no-]quarantine        Enable/disable quarantine of downloads.
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(
        string helpText,
        IReadOnlyDictionary<string, string> optionTypes)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var lines = NormalizeLines(helpText);
        int? descriptionColumn = null;

        for (var lineIndex = 0; lineIndex < lines.Length; lineIndex++)
        {
            var line = lines[lineIndex];
            if (string.IsNullOrWhiteSpace(line))
            {
                // The description column is only consistent within one option block.
                descriptionColumn = null;
                continue;
            }

            // Match option lines
            var match = BrewOptionPattern().Match(line);
            if (!match.Success)
            {
                continue;
            }

            var flagsPart = match.Groups["flags"].Value.Trim();
            var descriptionParts = new List<string>();
            var descriptionGroup = match.Groups["description"];
            var inlineDescription = descriptionGroup.Value.Trim();
            if (!string.IsNullOrEmpty(inlineDescription))
            {
                descriptionParts.Add(inlineDescription);
                descriptionColumn = descriptionGroup.Index;
            }

            while (lineIndex + 1 < lines.Length
                   && !string.IsNullOrWhiteSpace(lines[lineIndex + 1])
                   && IsWrappedDescriptionLine(lines[lineIndex + 1], descriptionColumn))
            {
                descriptionParts.Add(lines[++lineIndex].Trim());
            }

            var descriptionPart = string.Join(' ', descriptionParts);

            // Parse flags (may have multiple: "-d, --debug" or "--formula, --formulae")
            var flags = flagsPart.Split(',').Select(f => f.Trim()).Where(f => !string.IsNullOrEmpty(f)).ToList();

            // Find long form and short form
            var longForm = flags.FirstOrDefault(f => f.StartsWith("--") && !f.Contains("[no-]"));
            var shortForm = flags.FirstOrDefault(f => f.StartsWith("-") && !f.StartsWith("--"));

            // Handle [no-] options (e.g., --[no-]quarantine)
            var noOptPattern = flags.FirstOrDefault(f => f.Contains("[no-]"));
            if (noOptPattern is not null && longForm is null)
            {
                longForm = noOptPattern.Replace("[no-]", "");
            }

            if (longForm is null && shortForm is not null)
            {
                longForm = shortForm;
                shortForm = null;
            }

            if (string.IsNullOrEmpty(longForm))
            {
                continue;
            }

            var hasInlineValue = longForm.Contains('=');
            longForm = longForm.Split('=', 2)[0];

            // Skip duplicates
            if (seenOptions.Contains(longForm))
            {
                continue;
            }

            seenOptions.Add(longForm);

            var propertyName = NormalizePropertyName(longForm);
            if (propertyName is null)
            {
                continue;
            }

            // Homebrew omits value placeholders from option rows, so supplement them
            // from the usage synopsis and constrained description wording.
            var metadataName = longForm.TrimStart('-').Replace('-', '_');
            var isFlag = optionTypes.TryGetValue(metadataName, out var optionType)
                ? optionType.Equals("switch", StringComparison.Ordinal)
                : !hasInlineValue &&
                  !helpText.Contains($"{longForm}=", StringComparison.Ordinal) &&
                  !DescriptionSuggestsValue().IsMatch(descriptionPart);

            var csharpType = isFlag ? "bool?" : "string?";

            options.Add(new CliOptionDefinition
            {
                SwitchName = longForm,
                ShortForm = shortForm,
                PropertyName = propertyName,
                CSharpType = csharpType,
                Description = descriptionPart,
                IsFlag = isFlag,
                IsRequired = false,
                AcceptsMultipleValues = false,
                IsKeyValue = false,
                IsNumeric = false,
                ValueSeparator = isFlag ? " " : "=",
                EnumDefinition = null,
                IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag)
            });
        }

        return options;
    }

    /// <summary>
    /// Homebrew wraps long option descriptions onto lines aligned to the description
    /// column, and a wrapped fragment can itself look like an option row (for example
    /// "--fix-type=released." ending the <c>brew vulns --fix-available</c> description).
    /// Only a line whose flags start before the block's description column begins a new option.
    /// </summary>
    private static bool IsWrappedDescriptionLine(string line, int? descriptionColumn)
    {
        var match = BrewOptionPattern().Match(line);
        return !match.Success
               || (descriptionColumn is { } column && match.Groups["flags"].Index >= column);
    }

    private static IReadOnlyDictionary<string, string> ParseOptionTypes(IEnumerable<string> lines) =>
        lines
            .Where(static line => line.StartsWith($"{OptionTypeMarker}\t", StringComparison.Ordinal))
            .Select(static line => line.Split('\t', 3, StringSplitOptions.TrimEntries))
            .Where(static parts => parts.Length == 3)
            .GroupBy(static parts => parts[1], StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                static group => group.Key,
                static group => group.Last()[2],
                StringComparer.OrdinalIgnoreCase);

    private static string[] NormalizeLines(string helpText) => helpText
        .Replace("\r\n", "\n", StringComparison.Ordinal)
        .Replace('\r', '\n')
        .Split('\n');

    /// <summary>
    /// Checks if help text indicates the command has options.
    /// </summary>
    protected override bool HasOptions(string helpText)
    {
        return helpText.Contains("--") ||
               BrewOptionPattern().IsMatch(helpText);
    }

    #region Regex Patterns

    /// <summary>
    /// Matches "Commands:" section header.
    /// </summary>
    [GeneratedRegex(@"^(?:Commands|Subcommands):\s*$", RegexOptions.Multiline)]
    private static partial Regex CommandSectionPattern();

    /// <summary>
    /// Matches "Example usage:" section.
    /// </summary>
    [GeneratedRegex(@"^Example usage:\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase)]
    private static partial Regex ExampleUsagePattern();

    /// <summary>
    /// Matches next section headers (word followed by colon at start of line).
    /// </summary>
    [GeneratedRegex(@"^\w[\w\s]*:\s*$", RegexOptions.Multiline)]
    private static partial Regex NextSectionPattern();

    /// <summary>
    /// Matches command lines: "  command             description"
    /// </summary>
    [GeneratedRegex(@"^\s{2,}(?<name>[a-z0-9][a-z0-9+_.-]*)\s{2,}", RegexOptions.Multiline | RegexOptions.IgnoreCase)]
    private static partial Regex CommandLinePattern();

    /// <summary>
    /// Matches Homebrew bundle subcommands: "  install:".
    /// </summary>
    [GeneratedRegex(@"^\s{2}(?<name>[a-z0-9][a-z0-9+_.-]*):\s*$", RegexOptions.IgnoreCase)]
    private static partial Regex ColonDelimitedCommandLinePattern();

    /// <summary>
    /// Matches "brew commandname" lines in example usage.
    /// </summary>
    [GeneratedRegex(@"^\s*brew\s+(?<name>[a-z0-9][a-z0-9+_.-]*)", RegexOptions.Multiline | RegexOptions.IgnoreCase)]
    private static partial Regex BrewCommandLinePattern();

    [GeneratedRegex(@"^[a-z0-9][a-z0-9+_.-]*$", RegexOptions.IgnoreCase)]
    private static partial Regex BrewCommandNamePattern();

    /// <summary>
    /// Matches Homebrew-style option lines:
    ///   -d, --debug                  Display any debugging information.
    ///       --formula, --formulae    Treat all named arguments as formulae.
    /// </summary>
    [GeneratedRegex(@"^\s{2,}(?<flags>(?:-\w,\s*)?(?:--[\w\[\]-]+(?:=[^\s,]+)?(?:,\s*--[\w-]+(?:=[^\s,]+)?)*))(?:\s{2,}|\s*$)(?<description>.*)$", RegexOptions.Multiline)]
    private static partial Regex BrewOptionPattern();

    /// <summary>
    /// Matches descriptions that suggest the option takes a value.
    /// </summary>
    [GeneratedRegex(@"comma-separated|how many|which type|this many|default value|specified as|\b(?:set|specify)\s+(?:the\s+)?(?:path|file|directory|name|value|version|license|location)\b|\b(?:writes?|output)\s+to\s+(?:the\s+)?(?:path|file|directory|location)\b|\bfrom\s+(?:this|the|a)\s+(?:path|file|directory|location)\b|\b(?:path|file|directory|name|value|version|license|location)\s+to\b|\bspecified\s+(?:path|file|directory|name|value|version|license|location)\b", RegexOptions.IgnoreCase)]
    private static partial Regex DescriptionSuggestsValue();

    /// <summary>
    /// Matches an inline Homebrew usage synopsis.
    /// </summary>
    [GeneratedRegex(@"^Usage:\s*(?<synopsis>.+)$", RegexOptions.IgnoreCase)]
    private static partial Regex UsageLinePattern();

    #endregion
}
