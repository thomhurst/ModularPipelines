using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for GitHub CLI (gh).
/// gh uses a variant of Cobra-style help format with colons after command names.
///
/// gh help format (gh --help):
/// Work seamlessly with GitHub from the command line.
///
/// USAGE
///   gh &lt;command&gt; &lt;subcommand&gt; [flags]
///
/// CORE COMMANDS
///   auth:        Authenticate gh and git with GitHub
///   browse:      Open the repository in the browser
///   ...
///
/// HELP TOPICS
///   accessibility:  Learn about GitHub CLI's accessibility experiences
///   ...
/// </summary>
public partial class GhCliScraper : CobraCliScraper
{
    public GhCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<GhCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "gh";

    public override string NamespacePrefix => "Gh";

    public override string TargetNamespace => "ModularPipelines.GitHub";

    public override string OutputDirectory => "src/ModularPipelines.GitHub";

    /// <summary>
    /// Skip utility commands and help topics.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "alias", "co",
        // Help topics (not real commands)
        "accessibility", "actions", "environment", "exit-codes", "formatting", "mintty", "reference"
    };

    /// <summary>
    /// Extracts subcommand names from gh help text.
    /// gh uses "command:" format (with colon after command name).
    /// Only extracts from COMMANDS sections, not HELP TOPICS.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        var seenCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Normalize line endings for consistent parsing
        var normalizedText = helpText.Replace("\r\n", "\n").Replace("\r", "\n");

        // Collect all command section matches
        var sectionMatches = new List<Match>();

        // Try primary pattern for gh-specific sections (CORE COMMANDS, etc.)
        var commandsSectionMatches = GhCommandsSectionPattern().Matches(normalizedText);
        foreach (Match m in commandsSectionMatches)
        {
            sectionMatches.Add(m);
        }

        // Also try fallback pattern for standard Cobra "Commands:" sections
        var fallbackMatches = FallbackCommandsSectionPattern().Matches(normalizedText);
        foreach (Match m in fallbackMatches)
        {
            sectionMatches.Add(m);
        }

        if (sectionMatches.Count == 0)
        {
            Logger.LogWarning("[gh] No COMMANDS sections found in help text. Tried GhCommandsSectionPattern and FallbackCommandsSectionPattern");
            return subcommands;
        }

        Logger.LogDebug("[gh] Found {Count} command sections in help text", sectionMatches.Count);

        foreach (var sectionMatch in sectionMatches)
        {
            var sectionStart = sectionMatch.Index + sectionMatch.Length;

            // Find where this section ends (next ALL-CAPS section header)
            var sectionEnd = normalizedText.Length;
            var nextSectionMatch = GhSectionHeaderPattern().Match(normalizedText, sectionStart);
            if (nextSectionMatch.Success)
            {
                sectionEnd = nextSectionMatch.Index;
            }

            var section = normalizedText.Substring(sectionStart, sectionEnd - sectionStart);

            // Parse command lines in this section only
            var lines = section.Split('\n');
            foreach (var line in lines)
            {
                // Try gh-specific pattern first (command: description)
                var match = GhSubcommandLinePattern().Match(line);
                if (!match.Success)
                {
                    // Try standard Cobra pattern (command    description)
                    match = FallbackSubcommandLinePattern().Match(line);
                }

                if (match.Success)
                {
                    var commandName = match.Groups["name"].Value.Trim();
                    if (!string.IsNullOrEmpty(commandName) &&
                        !commandName.Contains(' ') &&
                        commandName.Length > 1 &&
                        seenCommands.Add(commandName))
                    {
                        subcommands.Add(commandName);
                    }
                }
            }
        }

        Logger.LogInformation("Extracted {Count} subcommands from gh help", subcommands.Count);
        return subcommands;
    }

    #region Regex Patterns

    /// <summary>
    /// Matches COMMANDS section headers (but NOT HELP TOPICS or other non-command sections).
    /// Includes AVAILABLE COMMANDS which is used in newer gh versions and subcommand help.
    /// </summary>
    [GeneratedRegex(@"^(?:CORE|ADDITIONAL|GITHUB\s+ACTIONS|ALIAS|AVAILABLE)\s+COMMANDS\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase)]
    private static partial Regex GhCommandsSectionPattern();

    /// <summary>
    /// Matches any ALL-CAPS section header (used to find section boundaries).
    /// </summary>
    [GeneratedRegex(@"^[A-Z][A-Z\s]+$", RegexOptions.Multiline)]
    private static partial Regex GhSectionHeaderPattern();

    /// <summary>
    /// Matches FLAGS section header.
    /// </summary>
    [GeneratedRegex(@"^FLAGS\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase)]
    private static partial Regex GhFlagsSectionPattern();

    /// <summary>
    /// Matches gh subcommand lines: "  command:    description"
    /// </summary>
    [GeneratedRegex(@"^\s{2}(?<name>[\w-]+):\s+\S", RegexOptions.Multiline)]
    private static partial Regex GhSubcommandLinePattern();

    /// <summary>
    /// Fallback pattern for standard Cobra-style "Commands:" or "Available Commands:" sections.
    /// </summary>
    [GeneratedRegex(@"(?:\w+\s+)?[Cc]ommands?:\s*\n", RegexOptions.Multiline)]
    private static partial Regex FallbackCommandsSectionPattern();

    /// <summary>
    /// Fallback pattern for standard Cobra subcommand lines: "  command    description"
    /// </summary>
    [GeneratedRegex(@"^\s{2,}(?<name>[\w-]+)\s{2,}", RegexOptions.Multiline)]
    private static partial Regex FallbackSubcommandLinePattern();

    #endregion
}
