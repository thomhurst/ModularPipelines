using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Yarn package manager CLI.
/// Supports Yarn Berry (v2+) help format.
///
/// Yarn help format (yarn --help):
/// Usage:
///
/// $ yarn &lt;command&gt;
///
/// Where &lt;command&gt; is one of:
///   - add, bin, cache, ...
///
/// Subcommand help (yarn add --help):
/// Usage:
///
/// $ yarn add [--json] [-F,--fixed] [-E,--exact] ...
///
/// Details:
///
/// This command adds a package to the package.json...
///
/// Examples:
///
/// yarn add lodash
///
/// Options:
///
///   --json                     Format output as NDJSON
///   -F,--fixed                 Store the exact version
/// </summary>
public partial class YarnCliScraper : CliScraperBase
{
    public YarnCliScraper(ICliCommandExecutor executor, ILogger<YarnCliScraper> logger)
        : base(executor, logger)
    {
    }

    public override string ToolName => "yarn";

    public override string NamespacePrefix => "Yarn";

    public override string TargetNamespace => "ModularPipelines.Yarn";

    public override string OutputDirectory => "src/ModularPipelines.Yarn";

    /// <summary>
    /// Yarn has mostly flat structure with some nesting (npm/*, plugin/*, workspace/*).
    /// </summary>
    protected override int MaxDiscoveryDepth => 2;

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "-v", "help"
    };

    /// <summary>
    /// Extracts subcommand names from Yarn help text.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        var seenCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Try to find "Where <command> is one of:" section
        var whereMatch = WhereCommandPattern().Match(helpText);
        if (whereMatch.Success)
        {
            var sectionStart = whereMatch.Index + whereMatch.Length;

            // Find section content until double newline or end
            var sectionEnd = helpText.Length;
            var doubleNewline = helpText.IndexOf("\n\n", sectionStart, StringComparison.Ordinal);
            if (doubleNewline > 0)
            {
                sectionEnd = doubleNewline;
            }

            var section = helpText.Substring(sectionStart, sectionEnd - sectionStart);

            // Parse command list: "  - add, bin, cache, ..." or separate lines
            var lines = section.Split('\n');
            foreach (var line in lines)
            {
                var trimmed = line.Trim().TrimStart('-').Trim();
                if (string.IsNullOrEmpty(trimmed))
                {
                    continue;
                }

                // Handle comma-separated list
                var parts = trimmed.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in parts)
                {
                    var commandName = part.Trim();
                    if (!string.IsNullOrEmpty(commandName) &&
                        !commandName.Contains(' ') &&
                        IsValidCommand(commandName) &&
                        seenCommands.Add(commandName))
                    {
                        subcommands.Add(commandName);
                    }
                }
            }
        }

        // Also try to find commands in "Commands:" or similar sections
        var commandsSectionMatch = CommandsSectionPattern().Match(helpText);
        if (commandsSectionMatch.Success)
        {
            var sectionStart = commandsSectionMatch.Index + commandsSectionMatch.Length;
            var sectionEnd = helpText.Length;

            var nextSection = NextSectionPattern().Match(helpText, sectionStart);
            if (nextSection.Success)
            {
                sectionEnd = nextSection.Index;
            }

            var section = helpText.Substring(sectionStart, sectionEnd - sectionStart);
            var lines = section.Split('\n');

            foreach (var line in lines)
            {
                var match = SubcommandLinePattern().Match(line);
                if (match.Success)
                {
                    var commandName = match.Groups["name"].Value.Trim();
                    if (!string.IsNullOrEmpty(commandName) &&
                        IsValidCommand(commandName) &&
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
    /// Checks if a string looks like a valid command name.
    /// </summary>
    private static bool IsValidCommand(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
        {
            return false;
        }

        // Commands should be lowercase and contain only letters, digits, and hyphens
        return name.All(c => char.IsLower(c) || char.IsDigit(c) || c == '-');
    }

    /// <summary>
    /// Parses a Yarn command from its help text.
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

        // Get subdomain for grouping (e.g., "npm" for "npm info")
        var subDomain = commandParts.Length > 1 ? ToPascalCase(commandParts[0]) : null;

        // Parse description from Details section
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
            SubDomainGroup = subDomain,
            Enums = enums
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    /// <summary>
    /// Extracts description from help text (from Details section).
    /// </summary>
    private static string? ExtractDescription(string helpText)
    {
        // Look for "Details:" section
        var detailsMatch = DetailsSectionPattern().Match(helpText);
        if (detailsMatch.Success)
        {
            var sectionStart = detailsMatch.Index + detailsMatch.Length;
            var sectionEnd = helpText.Length;

            // Find end of section (next section or double newline)
            var nextSection = NextSectionPattern().Match(helpText, sectionStart);
            if (nextSection.Success)
            {
                sectionEnd = nextSection.Index;
            }

            var section = helpText.Substring(sectionStart, sectionEnd - sectionStart).Trim();
            var lines = section.Split('\n');

            // Get first non-empty line as description
            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                if (!string.IsNullOrEmpty(trimmed) && trimmed.Length > 10)
                {
                    return trimmed;
                }
            }
        }

        // Fallback: look for first descriptive line after Usage
        var usageMatch = UsagePattern().Match(helpText);
        if (usageMatch.Success)
        {
            var afterUsage = helpText.Substring(usageMatch.Index + usageMatch.Length);
            var lines = afterUsage.Split('\n');

            foreach (var line in lines)
            {
                var trimmed = line.Trim();

                // Skip empty lines and command examples
                if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("$") || trimmed.StartsWith("yarn"))
                {
                    continue;
                }

                // Skip section headers
                if (trimmed.EndsWith(":") || trimmed.StartsWith("Where"))
                {
                    continue;
                }

                if (trimmed.Length > 10)
                {
                    return trimmed;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Parses options from Yarn help text.
    /// Format: --option                Description
    ///         -S,--short              Description
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(string helpText, string[] commandParts)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var className = GenerateClassName([ToolName, .. commandParts]);

        // Find "Options:" section
        var optionsMatch = OptionsSectionPattern().Match(helpText);
        if (!optionsMatch.Success)
        {
            return options;
        }

        var sectionStart = optionsMatch.Index + optionsMatch.Length;

        // Find where this section ends
        var sectionEnd = helpText.Length;
        var nextSection = NextSectionPattern().Match(helpText, sectionStart);
        if (nextSection.Success)
        {
            sectionEnd = nextSection.Index;
        }

        var section = helpText.Substring(sectionStart, sectionEnd - sectionStart);
        var lines = section.Split('\n');

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];

            // Match Yarn option patterns
            var match = YarnOptionPattern().Match(line);
            if (!match.Success)
            {
                continue;
            }

            var shortForm = match.Groups["short"].Value.Trim();
            var longForm = match.Groups["long"].Value.Trim();
            var description = match.Groups["desc"].Value.Trim();

            if (string.IsNullOrEmpty(longForm))
            {
                // If only short form, use it as the main form
                if (!string.IsNullOrEmpty(shortForm))
                {
                    longForm = shortForm;
                    shortForm = string.Empty;
                }
                else
                {
                    continue;
                }
            }

            // Skip duplicates
            if (seenOptions.Contains(longForm))
            {
                continue;
            }

            seenOptions.Add(longForm);

            // Accumulate multi-line descriptions
            i = AccumulateMultiLineDescription(lines, i, ref description);

            var propertyName = NormalizePropertyName(longForm);
            if (propertyName is null)
            {
                continue;
            }

            // Determine if this is a flag (boolean) or takes a value
            var isFlag = IsBooleanOption(longForm, description);
            var csharpType = isFlag ? "bool?" : "string?";

            options.Add(new CliOptionDefinition
            {
                SwitchName = longForm,
                ShortForm = string.IsNullOrEmpty(shortForm) ? null : shortForm,
                PropertyName = propertyName,
                CSharpType = csharpType,
                Description = description,
                IsFlag = isFlag,
                IsRequired = false,
                AcceptsMultipleValues = false,
                IsKeyValue = false,
                IsNumeric = false,
                ValueSeparator = isFlag ? " " : " ",
                EnumDefinition = null
            });
        }

        return options;
    }

    /// <summary>
    /// Determines if an option is a boolean flag.
    /// </summary>
    private static bool IsBooleanOption(string optionName, string description)
    {
        // Options that typically take values
        var valueOptions = new[] { "registry", "cwd", "path", "name", "version", "config", "scope" };
        var cleanName = optionName.TrimStart('-').ToLowerInvariant();

        if (valueOptions.Any(v => cleanName.Contains(v)))
        {
            return false;
        }

        // Check description for value hints
        var lowerDesc = description.ToLowerInvariant();
        if (lowerDesc.Contains("path") || lowerDesc.Contains("name") ||
            lowerDesc.Contains("url") || lowerDesc.Contains("file"))
        {
            return false;
        }

        // Common boolean option patterns
        if (cleanName.StartsWith("no-") ||
            cleanName == "json" ||
            cleanName == "verbose" ||
            cleanName == "silent" ||
            cleanName == "offline" ||
            cleanName == "prefer-offline" ||
            cleanName == "exact" ||
            cleanName == "tilde" ||
            cleanName == "caret" ||
            cleanName == "dev" ||
            cleanName == "peer" ||
            cleanName == "optional" ||
            cleanName == "cached" ||
            cleanName == "mode")
        {
            return true;
        }

        // Default to string for safety
        return false;
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

            // Section header
            if (trimmedNext.EndsWith(':') && trimmedNext.Length < 30)
            {
                break;
            }

            // Line must be indented to be a continuation
            var leadingSpaces = nextLine.Length - nextLine.TrimStart().Length;
            if (leadingSpaces < 20)
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
        return helpText.Contains("Options:") ||
               helpText.Contains("--");
    }

    #region Regex Patterns

    /// <summary>
    /// Matches "Where <command> is one of:" section.
    /// </summary>
    [GeneratedRegex(@"Where\s+<command>\s+is\s+one\s+of:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex WhereCommandPattern();

    /// <summary>
    /// Matches "Commands:" section header.
    /// </summary>
    [GeneratedRegex(@"Commands?:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex CommandsSectionPattern();

    /// <summary>
    /// Matches "Details:" section.
    /// </summary>
    [GeneratedRegex(@"Details:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex DetailsSectionPattern();

    /// <summary>
    /// Matches "Options:" section.
    /// </summary>
    [GeneratedRegex(@"Options:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex OptionsSectionPattern();

    /// <summary>
    /// Matches "Usage:" section.
    /// </summary>
    [GeneratedRegex(@"Usage:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex UsagePattern();

    /// <summary>
    /// Matches next section headers.
    /// </summary>
    [GeneratedRegex(@"\n[A-Z][\w\s]*:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex NextSectionPattern();

    /// <summary>
    /// Matches subcommand lines: "  command    description"
    /// </summary>
    [GeneratedRegex(@"^\s{2,}(?<name>[\w-]+)\s{2,}", RegexOptions.Multiline)]
    private static partial Regex SubcommandLinePattern();

    /// <summary>
    /// Matches Yarn-style option lines:
    /// --json                     Description
    /// -F,--fixed                 Description
    /// </summary>
    [GeneratedRegex(@"^\s*(?:(?<short>-\w),)?(?<long>--[\w-]+)\s{2,}(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex YarnOptionPattern();

    #endregion
}
