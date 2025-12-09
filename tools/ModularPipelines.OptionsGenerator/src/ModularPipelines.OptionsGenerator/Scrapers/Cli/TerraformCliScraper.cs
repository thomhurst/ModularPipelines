using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Terraform CLI.
/// Terraform uses Go's built-in flag package with single-dash flags (-flag instead of --flag).
///
/// Terraform help format:
/// Usage: terraform [global options] &lt;subcommand&gt; [args]
///
/// The available commands for execution are listed below.
///
/// Main commands:
///   init          Prepare your working directory
///   validate      Check whether the configuration is valid
///   plan          Show changes required by the current configuration
///   apply         Create or update infrastructure
///   destroy       Destroy previously-created infrastructure
///
/// All other commands:
///   console       Try Terraform expressions
///   ...
///
/// Subcommand help (terraform init -help):
/// Usage: terraform [global options] init [options]
///
/// Options:
///   -backend=true             Configure the backend
///   -backend-config=path      Configuration to be merged
///   -force-copy               Suppress prompts about moving state data
/// </summary>
public partial class TerraformCliScraper : CliScraperBase
{
    public TerraformCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<TerraformCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "terraform";

    public override string NamespacePrefix => "Terraform";

    public override string TargetNamespace => "ModularPipelines.Terraform";

    public override string OutputDirectory => "src/ModularPipelines.Terraform";


    /// <summary>
    /// Skip less useful commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "version", "-version", "-help", "help"
    };

    /// <summary>
    /// Terraform uses -help instead of --help for subcommands.
    /// </summary>
    protected override async Task<string?> GetHelpTextAsync(string[] commandPath, CancellationToken cancellationToken)
    {
        var cacheKey = string.Join(" ", commandPath);

        // Build the arguments: everything after the tool name, plus -help
        var args = commandPath.Length > 1
            ? string.Join(" ", commandPath.Skip(1)) + " -help"
            : "-help";

        var result = await Executor.ExecuteAsync(ExecutablePath, args, cancellationToken);

        // Terraform outputs help to stdout
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
    /// Extracts subcommand names from Terraform help text.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        var seenCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find command sections (Main commands:, All other commands:, Subcommands:)
        var commandSectionMatches = CommandSectionPattern().Matches(helpText);
        if (commandSectionMatches.Count == 0)
        {
            return subcommands;
        }

        foreach (Match sectionMatch in commandSectionMatches)
        {
            var sectionStart = sectionMatch.Index + sectionMatch.Length;

            // Find where this section ends (next section header or end of text)
            var sectionEnd = helpText.Length;
            var nextSectionMatch = SectionHeaderPattern().Match(helpText, sectionStart);
            if (nextSectionMatch.Success)
            {
                sectionEnd = nextSectionMatch.Index;
            }

            var section = helpText.Substring(sectionStart, sectionEnd - sectionStart);

            // Parse command lines: "  command    description"
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
    /// Parses a Terraform command from its help text.
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

        // Get the first subcommand for grouping (e.g., "workspace" for "workspace list")
        var subDomain = commandParts.Length > 1 ? ToPascalCase(commandParts[0]) : null;

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
            SubDomainGroup = subDomain,
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

            if (foundUsage && !string.IsNullOrEmpty(trimmed) && !trimmed.StartsWith('-'))
            {
                // Skip section headers
                if (trimmed.EndsWith(':') && trimmed.Length < 50)
                {
                    continue;
                }

                // Found a description line
                if (trimmed.Length > 10)
                {
                    return trimmed;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Parses options from Terraform help text.
    /// Terraform uses single-dash flags: -flag or -option=value
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(string helpText, string[] commandParts)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var className = GenerateClassName([ToolName, .. commandParts]);

        // Find Options: section
        var optionsMatch = OptionsSectionPattern().Match(helpText);
        if (!optionsMatch.Success)
        {
            return options;
        }

        var sectionStart = optionsMatch.Index + optionsMatch.Length;

        // Find where this section ends
        var sectionEnd = helpText.Length;
        var nextSectionMatch = SectionHeaderPattern().Match(helpText, sectionStart);
        if (nextSectionMatch.Success)
        {
            sectionEnd = nextSectionMatch.Index;
        }

        var section = helpText.Substring(sectionStart, sectionEnd - sectionStart);
        var lines = section.Split('\n');

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];

            // Match Terraform option patterns:
            // -flag           Boolean flag
            // -option=value   Value option with default
            // -option=PATH    Value option with type hint
            var match = TerraformOptionPattern().Match(line);
            if (!match.Success)
            {
                continue;
            }

            var flagName = match.Groups["flag"].Value.Trim();
            var valueHint = match.Groups["value"].Value.Trim();
            var description = match.Groups["desc"].Value.Trim();

            if (string.IsNullOrEmpty(flagName))
            {
                continue;
            }

            // Skip duplicates
            if (seenOptions.Contains(flagName))
            {
                continue;
            }

            seenOptions.Add(flagName);

            // Accumulate multi-line descriptions
            i = AccumulateMultiLineDescription(lines, i, ref description);

            var propertyName = NormalizePropertyName(flagName);
            if (propertyName is null)
            {
                continue;
            }

            // Determine type based on value hint
            var isFlag = string.IsNullOrEmpty(valueHint);
            var isBoolean = valueHint.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                            valueHint.Equals("false", StringComparison.OrdinalIgnoreCase) ||
                            valueHint.Equals("bool", StringComparison.OrdinalIgnoreCase);
            var isInteger = IsNumericHint(valueHint);

            if (isBoolean)
            {
                isFlag = true;
            }

            var csharpType = isFlag ? "bool?" : (isInteger ? "int?" : "string?");

            options.Add(new CliOptionDefinition
            {
                SwitchName = $"-{flagName}",
                ShortForm = null,
                PropertyName = propertyName,
                CSharpType = csharpType,
                Description = description,
                IsFlag = isFlag,
                IsRequired = false,
                AcceptsMultipleValues = false,
                IsKeyValue = false,
                IsNumeric = isInteger,
                ValueSeparator = isFlag ? " " : "=",
                EnumDefinition = null
            });
        }

        return options;
    }

    /// <summary>
    /// Checks if a value hint indicates a numeric type.
    /// </summary>
    private static bool IsNumericHint(string hint)
    {
        if (string.IsNullOrEmpty(hint))
        {
            return false;
        }

        // Check for numeric defaults
        if (int.TryParse(hint, out _))
        {
            return true;
        }

        // Check for numeric type names
        var numericHints = new[] { "n", "N", "count", "number", "int", "parallelism" };
        return numericHints.Contains(hint, StringComparer.OrdinalIgnoreCase);
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
            if (trimmedNext.EndsWith(':') && trimmedNext.Length < 50)
            {
                break;
            }

            // Line must be indented to be a continuation
            var leadingSpaces = nextLine.Length - nextLine.TrimStart().Length;
            if (leadingSpaces < 10)
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
               helpText.Contains("-help") ||
               TerraformOptionPattern().IsMatch(helpText);
    }

    #region Regex Patterns

    /// <summary>
    /// Matches command section headers: "Main commands:", "All other commands:", "Subcommands:"
    /// </summary>
    [GeneratedRegex(@"(?:Main\s+|All\s+other\s+)?(?:commands|Subcommands):\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex CommandSectionPattern();

    /// <summary>
    /// Matches section headers.
    /// </summary>
    [GeneratedRegex(@"^[A-Z][\w\s]*:\s*$", RegexOptions.Multiline)]
    private static partial Regex SectionHeaderPattern();

    /// <summary>
    /// Matches subcommand lines: "  command    description"
    /// </summary>
    [GeneratedRegex(@"^\s{2,}(?<name>[\w-]+)\s{2,}", RegexOptions.Multiline)]
    private static partial Regex SubcommandLinePattern();

    /// <summary>
    /// Matches "Options:" section header.
    /// </summary>
    [GeneratedRegex(@"^Options?:\s*\n", RegexOptions.Multiline | RegexOptions.IgnoreCase)]
    private static partial Regex OptionsSectionPattern();

    /// <summary>
    /// Matches Terraform-style option lines:
    /// -flag           Description
    /// -option=value   Description
    /// -option=PATH    Description
    /// </summary>
    [GeneratedRegex(@"^\s*-(?<flag>[\w-]+)(?:=(?<value>\S+))?\s{2,}(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex TerraformOptionPattern();

    #endregion
}
