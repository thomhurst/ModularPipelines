using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Go tooling CLI.
/// Go uses a custom help format.
///
/// go help format (go help):
/// Go is a tool for managing Go source code.
///
/// Usage:
///         go &lt;command&gt; [arguments]
///
/// The commands are:
///         bug         start a bug report
///         build       compile packages and dependencies
///         clean       remove object files and cached files
///         ...
///
/// Subcommand help (go help build):
/// usage: go build [-o output] [build flags] [packages]
///
/// Build compiles the packages named by the import paths...
///
/// The build flags are:
///         -a          force rebuilding of packages that are already up-to-date
///         -n          print the commands but do not run them
///         ...
/// </summary>
public partial class GoCliScraper : CliScraperBase
{
    public GoCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<GoCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "go";

    public override string NamespacePrefix => "Go";

    public override string TargetNamespace => "ModularPipelines.Go";

    public override string OutputDirectory => "src/ModularPipelines.Go";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "help", "bug", "doc", "version"
    };

    /// <summary>
    /// Gets help text for go commands using "go help &lt;command&gt;" format.
    /// </summary>
    protected override async Task<string?> GetHelpTextAsync(string[] commandPath, CancellationToken cancellationToken)
    {
        var cacheKey = string.Join(" ", commandPath);

        if (HelpCache.TryGet(cacheKey, out var cached))
        {
            return cached;
        }

        // Go uses "go help <command>" instead of "go <command> --help"
        var args = commandPath.Length > 1
            ? "help " + string.Join(" ", commandPath.Skip(1))
            : "help";

        var result = await Executor.ExecuteAsync(ExecutablePath, args, cancellationToken);

        var helpText = !string.IsNullOrEmpty(result.StandardOutput)
            ? result.StandardOutput
            : result.StandardError;

        if (!string.IsNullOrWhiteSpace(helpText))
        {
            HelpCache.Set(cacheKey, helpText);
            return helpText;
        }

        Logger.LogWarning("No help text for command: {Command}", cacheKey);
        return null;
    }

    /// <summary>
    /// Extracts subcommand names from go help text.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        var seenCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find "The commands are:" section
        var commandsSectionMatch = CommandsSectionPattern().Match(helpText);
        if (commandsSectionMatch.Success)
        {
            var sectionStart = commandsSectionMatch.Index + commandsSectionMatch.Length;
            var sectionEnd = helpText.Length;

            // Find where this section ends (next blank line or section)
            var nextSection = NextSectionPattern().Match(helpText, sectionStart);
            if (nextSection.Success)
            {
                sectionEnd = nextSection.Index;
            }

            var section = helpText.Substring(sectionStart, sectionEnd - sectionStart);
            var lines = section.Split('\n');

            foreach (var line in lines)
            {
                var match = CommandLinePattern().Match(line);
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

        return name.All(c => char.IsLower(c) || char.IsDigit(c) || c == '-');
    }

    /// <summary>
    /// Parses a go command from its help text.
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

        // Skip usage line and look for first descriptive paragraph
        var foundUsage = false;
        foreach (var line in lines)
        {
            var trimmed = line.Trim();

            if (trimmed.StartsWith("usage:", StringComparison.OrdinalIgnoreCase) ||
                trimmed.StartsWith("Usage:", StringComparison.OrdinalIgnoreCase))
            {
                foundUsage = true;
                continue;
            }

            if (!foundUsage)
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(trimmed))
            {
                continue;
            }

            // Skip flag lines
            if (trimmed.StartsWith('-'))
            {
                continue;
            }

            // Skip section headers
            if (trimmed.EndsWith(':'))
            {
                continue;
            }

            if (trimmed.Length > 10)
            {
                return trimmed;
            }
        }

        return null;
    }

    /// <summary>
    /// Parses options from go help text.
    /// Format: -flag    description
    ///         -flag value    description
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(string helpText, string[] commandParts)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find flags sections
        var flagsSectionMatch = FlagsSectionPattern().Match(helpText);
        if (!flagsSectionMatch.Success)
        {
            return options;
        }

        var sectionStart = flagsSectionMatch.Index + flagsSectionMatch.Length;
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
            var match = GoOptionPattern().Match(line);
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

            // Convert single-dash flags to double-dash for consistency
            var longForm = flagName.StartsWith("--") ? flagName : $"--{flagName.TrimStart('-')}";

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

            var isFlag = string.IsNullOrEmpty(valueHint);
            var csharpType = isFlag ? "bool?" : "string?";

            options.Add(new CliOptionDefinition
            {
                SwitchName = flagName.TrimStart('-'),
                ShortForm = null,
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

            if (string.IsNullOrWhiteSpace(trimmedNext))
            {
                break;
            }

            if (trimmedNext.StartsWith('-'))
            {
                break;
            }

            if (trimmedNext.EndsWith(':') && trimmedNext.Length < 30)
            {
                break;
            }

            var leadingSpaces = nextLine.Length - nextLine.TrimStart().Length;
            if (leadingSpaces < 8)
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
        return helpText.Contains("flags are:") ||
               helpText.Contains("Flags:") ||
               FlagLinePattern().IsMatch(helpText);
    }

    #region Regex Patterns

    /// <summary>
    /// Matches "The commands are:" section.
    /// </summary>
    [GeneratedRegex(@"The commands are:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex CommandsSectionPattern();

    /// <summary>
    /// Matches command lines: "        build       compile packages..."
    /// </summary>
    [GeneratedRegex(@"^\s{4,}(?<name>[\w-]+)\s{2,}", RegexOptions.Multiline)]
    private static partial Regex CommandLinePattern();

    /// <summary>
    /// Matches flags sections like "The build flags are:" or "Flags:".
    /// </summary>
    [GeneratedRegex(@"(?:The\s+\w+\s+)?[Ff]lags(?:\s+are)?:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex FlagsSectionPattern();

    /// <summary>
    /// Matches next section or blank line.
    /// </summary>
    [GeneratedRegex(@"\n\n|\n[A-Z]")]
    private static partial Regex NextSectionPattern();

    /// <summary>
    /// Matches go-style option lines:
    /// -a          force rebuilding...
    /// -n          print the commands...
    /// -o file     write output to file
    /// </summary>
    [GeneratedRegex(@"^\s+(?<flag>-[\w-]+)(?:\s+(?<value>\w+))?\s{2,}(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex GoOptionPattern();

    /// <summary>
    /// Matches flag lines.
    /// </summary>
    [GeneratedRegex(@"^\s+-\w", RegexOptions.Multiline)]
    private static partial Regex FlagLinePattern();

    #endregion
}
