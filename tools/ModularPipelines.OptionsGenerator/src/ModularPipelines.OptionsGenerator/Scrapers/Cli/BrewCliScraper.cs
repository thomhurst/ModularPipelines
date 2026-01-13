using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
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

    /// <summary>
    /// Extracts subcommand names from Homebrew help text.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        var seenCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find "Commands:" section
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
        CancellationToken cancellationToken)
    {
        var commandParts = commandPath.Skip(1).ToArray(); // Skip tool name

        if (commandParts.Length == 0)
        {
            // This is just the root command, skip it
            return Task.FromResult<CliCommandDefinition?>(null);
        }

        // Parse description from help text
        var description = ExtractDescription(helpText);

        // Parse options from the help text
        var options = ParseOptions(helpText, commandParts);

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

        // Look for description after "Usage:" line
        var foundUsage = false;
        foreach (var line in lines)
        {
            var trimmed = line.Trim();

            if (trimmed.StartsWith("Usage:"))
            {
                foundUsage = true;
                continue;
            }

            // Skip empty lines after Usage
            if (string.IsNullOrEmpty(trimmed))
            {
                continue;
            }

            // Skip lines that look like options or sections
            if (trimmed.StartsWith('-') || trimmed.EndsWith(':'))
            {
                continue;
            }

            // Found a description line
            if (foundUsage && trimmed.Length > 5 && !trimmed.Contains("--"))
            {
                return trimmed;
            }
        }

        return null;
    }

    /// <summary>
    /// Parses options from Homebrew help text.
    /// Format variations:
    ///       --formula, --formulae    Treat all named arguments as formulae.
    ///   -d, --debug                  Display any debugging information.
    ///       --[no-]quarantine        Enable/disable quarantine of downloads.
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(string helpText, string[] commandParts)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var className = GenerateClassName([ToolName, .. commandParts]);

        var lines = helpText.Split('\n');

        foreach (var line in lines)
        {
            // Match option lines
            var match = BrewOptionPattern().Match(line);
            if (!match.Success)
            {
                continue;
            }

            var flagsPart = match.Groups["flags"].Value.Trim();
            var descriptionPart = match.Groups["description"].Value.Trim();

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

            // Determine if it's a flag or takes a value
            // Homebrew options are mostly flags unless they mention a value in description
            var isFlag = !descriptionPart.Contains("=") &&
                         !longForm.Contains("=") &&
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
                EnumDefinition = null
            });
        }

        return options;
    }

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
    [GeneratedRegex(@"^Commands:\s*$", RegexOptions.Multiline)]
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
    [GeneratedRegex(@"^\s{2,}(?<name>[\w-]+)\s{2,}", RegexOptions.Multiline)]
    private static partial Regex CommandLinePattern();

    /// <summary>
    /// Matches "brew commandname" lines in example usage.
    /// </summary>
    [GeneratedRegex(@"^\s*brew\s+(?<name>[\w-]+)", RegexOptions.Multiline)]
    private static partial Regex BrewCommandLinePattern();

    /// <summary>
    /// Matches Homebrew-style option lines:
    ///   -d, --debug                  Display any debugging information.
    ///       --formula, --formulae    Treat all named arguments as formulae.
    /// </summary>
    [GeneratedRegex(@"^\s{2,}(?<flags>(?:-\w,\s*)?(?:--[\w\[\]-]+(?:,\s*--[\w-]+)*))(?:\s{2,}|\s*$)(?<description>.*)$", RegexOptions.Multiline)]
    private static partial Regex BrewOptionPattern();

    /// <summary>
    /// Matches descriptions that suggest the option takes a value.
    /// </summary>
    [GeneratedRegex(@"\b(?:set|specify|use|path|file|directory|number|name|value)\b", RegexOptions.IgnoreCase)]
    private static partial Regex DescriptionSuggestsValue();

    #endregion
}
