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
    public SnykCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<SnykCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "snyk";

    public override string NamespacePrefix => "Snyk";

    public override string TargetNamespace => "ModularPipelines.Snyk";

    public override string OutputDirectory => "src/ModularPipelines.Snyk";

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

        // Normalize line endings for consistent parsing
        var normalizedText = helpText.Replace("\r\n", "\n").Replace("\r", "\n");

        // Find "Commands:" section
        var commandsSectionMatch = CommandsSectionPattern().Match(normalizedText);
        if (!commandsSectionMatch.Success)
        {
            Logger.LogWarning("[snyk] No Commands section found in help text. Pattern: CommandsSectionPattern");
            return subcommands;
        }

        Logger.LogDebug("[snyk] Found Commands section at index {Index}", commandsSectionMatch.Index);

        var sectionStart = commandsSectionMatch.Index + commandsSectionMatch.Length;
        var sectionEnd = normalizedText.Length;

        // Find where this section ends
        var nextSection = NextSectionPattern().Match(normalizedText, sectionStart);
        if (nextSection.Success)
        {
            sectionEnd = nextSection.Index;
        }

        var section = normalizedText.Substring(sectionStart, sectionEnd - sectionStart);
        var lines = section.Split('\n');

        foreach (var line in lines)
        {
            // Try primary pattern
            var match = SnykCommandLinePattern().Match(line);
            if (!match.Success)
            {
                // Try fallback pattern for standard indented format
                match = FallbackCommandLinePattern().Match(line);
            }

            if (match.Success)
            {
                var commandName = match.Groups["command"].Value.Trim();
                if (!string.IsNullOrEmpty(commandName) &&
                    !commandName.Contains(' ') &&
                    seenCommands.Add(commandName))
                {
                    subcommands.Add(commandName);
                }
            }
        }

        Logger.LogInformation("[snyk] Extracted {Count} subcommands", subcommands.Count);
        return subcommands;
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
            PositionalArguments = [],
            SubDomainGroup = null,
            Enums = []
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

        // Find "Options:" section
        var optionsMatch = OptionsSectionPattern().Match(helpText);
        if (!optionsMatch.Success)
        {
            // Try to find options anywhere in the text
            var lines = helpText.Split('\n');
            ParseOptionLines(lines, options, seenOptions);
            return options;
        }

        var sectionStart = optionsMatch.Index + optionsMatch.Length;
        var sectionEnd = helpText.Length;

        var nextSection = NextSectionPattern().Match(helpText, sectionStart);
        if (nextSection.Success)
        {
            sectionEnd = nextSection.Index;
        }

        var section = helpText.Substring(sectionStart, sectionEnd - sectionStart);
        ParseOptionLines(section.Split('\n'), options, seenOptions);

        return options;
    }

    private void ParseOptionLines(string[] lines, List<CliOptionDefinition> options, HashSet<string> seenOptions)
    {
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];

            var match = SnykOptionPattern().Match(line);
            if (!match.Success)
            {
                continue;
            }

            var longForm = match.Groups["long"].Value.Trim();
            var valueHint = match.Groups["value"].Value.Trim();

            if (string.IsNullOrEmpty(longForm))
            {
                continue;
            }

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

            // Get description from next lines or inline
            var description = ExtractOptionDescription(lines, i);

            var isFlag = string.IsNullOrEmpty(valueHint);
            var csharpType = isFlag ? "bool?" : "string?";

            // Check for enum values in value hint
            CliEnumDefinition? enumDef = null;
            if (!string.IsNullOrEmpty(valueHint) && valueHint.Contains('|'))
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
                            MemberName = ToPascalCase(v.Replace("-", "")),
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
                IsRequired = false,
                AcceptsMultipleValues = false,
                IsKeyValue = false,
                IsNumeric = false,
                ValueSeparator = isFlag ? " " : "=",
                EnumDefinition = enumDef,
                IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag)
            });
        }
    }

    private static string? ExtractOptionDescription(string[] lines, int currentIndex)
    {
        // Look for description on same line or next lines
        var nextIndex = currentIndex + 1;
        var descParts = new List<string>();

        while (nextIndex < lines.Length)
        {
            var line = lines[nextIndex].Trim();
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("--"))
            {
                break;
            }

            descParts.Add(line);
            nextIndex++;
        }

        return descParts.Count > 0 ? string.Join(" ", descParts) : null;
    }

    /// <summary>
    /// Checks if help text indicates the command has options.
    /// </summary>
    protected override bool HasOptions(string helpText)
    {
        return helpText.Contains("--") || helpText.Contains("Options:");
    }

    #region Regex Patterns

    /// <summary>
    /// Matches "Commands:" or "Available commands:" section header.
    /// Newer versions of snyk use "Available commands" format.
    /// </summary>
    [GeneratedRegex(@"(?:Available\s+)?Commands?:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex CommandsSectionPattern();

    /// <summary>
    /// Matches "Options:" section header.
    /// </summary>
    [GeneratedRegex(@"Options?:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex OptionsSectionPattern();

    /// <summary>
    /// Matches next section headers.
    /// </summary>
    [GeneratedRegex(@"\n[A-Z][\w\s]*:\s*\n")]
    private static partial Regex NextSectionPattern();

    /// <summary>
    /// Matches Snyk command lines: "  test                   Test a project"
    /// </summary>
    [GeneratedRegex(@"^\s{2,}(?<command>[\w-]+)(?:\s+\[[^\]]+\])?\s{2,}", RegexOptions.Multiline)]
    private static partial Regex SnykCommandLinePattern();

    /// <summary>
    /// Fallback pattern for command lines with simpler format: "  command    description"
    /// Also matches format: "  command [args]    description"
    /// </summary>
    [GeneratedRegex(@"^\s{2,}(?<command>[\w-]+)\s+", RegexOptions.Multiline)]
    private static partial Regex FallbackCommandLinePattern();

    /// <summary>
    /// Matches Snyk-style option lines:
    /// --severity-threshold=&lt;low|medium|high|critical&gt;
    /// --json
    /// </summary>
    [GeneratedRegex(@"^\s*(?<long>--[\w-]+)(?:=<(?<value>[^>]+)>)?", RegexOptions.Multiline)]
    private static partial Regex SnykOptionPattern();

    #endregion
}
