using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for pnpm package manager CLI.
/// pnpm uses a Commander.js-style help format.
///
/// pnpm help format (pnpm --help):
/// Usage: pnpm [command] [flags]
///       pnpm [ -h | --help | -v | --version ]
///
/// Manage your dependencies:
///       add                  Installs a package and any packages that it depends on.
///       import               Generates a pnpm-lock.yaml from an npm package-lock.json
///       install              Installs all dependencies of the project
///       ...
///
/// Subcommand help (pnpm add --help):
/// pnpm add &lt;pkg&gt;
///
/// Installs a package and any packages that it depends on.
///
/// Options:
///   -D, --save-dev                    Save package to your `devDependencies`
///   -O, --save-optional               Save package to your `optionalDependencies`
///   ...
/// </summary>
public partial class PnpmCliScraper : CliScraperBase
{
    private static readonly HashSet<string> PlaceholderFreeValueOptions = new(StringComparer.OrdinalIgnoreCase)
    {
        "--allow-build",
        "--edit-dir",
        "--global-dir",
        "--otp",
        "--package",
        "--patches-dir",
        "--publish-branch",
        "--resume-from",
        "--sort-by",
    };

    private static readonly HashSet<string> PlaceholderFreeFlagOptions = new(StringComparer.OrdinalIgnoreCase)
    {
        "--config",
        "--reporter-hide-prefix",
    };

    private static readonly string[] ValueOptionNameIndicators =
    [
        "filter", "dir", "registry", "store", "config", "reporter", "loglevel",
    ];

    private static readonly string[] ValueDescriptionIndicators = ["path", "name", "url", "file"];

    public PnpmCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<PnpmCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "pnpm";

    public override string NamespacePrefix => "Pnpm";

    public override string TargetNamespace => "ModularPipelines.Node";

    public override string OutputDirectory => "src/ModularPipelines.Node";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "-v", "help", "root", "bin", "env"
    };

    /// <summary>
    /// Extracts subcommand names from pnpm help text.
    /// Most pnpm commands are flat, but newer releases also expose nested commands
    /// through an explicit Commands section (for example, audit signatures).
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        var seenCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var isRootHelp = helpText.Contains("Usage: pnpm [command]", StringComparison.OrdinalIgnoreCase)
                         || helpText.Contains("pnpm [command]", StringComparison.OrdinalIgnoreCase);
        if (isRootHelp)
        {
            // pnpm root help uses sections with headers followed by command lines:
            // Manage your dependencies:
            //       add                  Installs a package...
            //       install              Installs all dependencies...
            var commandLineMatches = CommandLinePattern().Matches(helpText);
            foreach (Match match in commandLineMatches)
            {
                AddSubcommand(match.Groups["command"].Value);
            }
        }

        // Also try to find commands in "Commands:" section
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
                    AddSubcommand(match.Groups["name"].Value);
                }
            }
        }

        return subcommands;

        void AddSubcommand(string candidate)
        {
            var commandName = candidate.Trim();
            if (!string.IsNullOrEmpty(commandName)
                && IsValidCommand(commandName)
                && seenCommands.Add(commandName))
            {
                subcommands.Add(commandName);
            }
        }
    }

    /// <inheritdoc />
    protected override IEnumerable<string> ExtractSubcommands(
        string[] commandPath,
        string helpText)
    {
        var subcommands = ExtractSubcommands(helpText).ToList();

        // pnpm returns the parent command-group help for leaf commands. For example,
        // `pnpm stage download --help` repeats the stage subcommand catalog. Once the
        // requested leaf appears in that catalog, rediscovering it would recursively
        // combine every sibling command with every other sibling command.
        var repeatsParentCatalog = commandPath.Length > 1
                                   && subcommands.Contains(commandPath[^1], StringComparer.OrdinalIgnoreCase);

        return repeatsParentCatalog ? [] : subcommands;
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
    /// Parses a pnpm command from its help text.
    /// </summary>
    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken) =>
        throw new InvalidOperationException("Shared traversal must pass its parsed synopsis.");

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
            return Task.FromResult<CliCommandDefinition?>(null);
        }

        usage = NormalizeParentGroupUsage(commandParts, usage);

        // Parse description from help text
        var description = ExtractDescription(helpText);

        // Parse options from the help text
        var options = ParseOptions(helpText, commandParts);

        // Extract enums from options
        var enums = options
            .Where(o => o.EnumDefinition is not null)
            .Select(o => o.EnumDefinition!)
            .ToList();
        var positionalArguments = GetPositionalArguments(usage);

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
            UsageSynopsis = usage.Synopsis,
            HasOperandTakingUsage = usage.HasOperandTokens,
            SubDomainGroup = null,
            Enums = enums
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    private static UsageSynopsisParseResult NormalizeParentGroupUsage(
        IReadOnlyList<string> commandParts,
        UsageSynopsisParseResult usage)
    {
        var normalized = commandParts is ["stage"] or ["audit"]
            ? usage with
            {
                HasOperandTokens = false,
                PositionalArguments = [],
                UnparsedOperandTokens = [],
            }
            : usage;
        return normalized with
        {
            PositionalArguments = normalized.PositionalArguments
                .Select(argument => argument with { Phase = CommandLinePhase.Passthrough })
                .ToArray(),
        };
    }

    /// <inheritdoc />
    protected override UsageSynopsisParseResult NormalizeUsageSynopsis(
        CliCommandDefinition command,
        UsageSynopsisParseResult usage) =>
        NormalizeParentGroupUsage(command.CommandParts, usage);

    /// <summary>
    /// Extracts description from help text.
    /// </summary>
    private static string? ExtractDescription(string helpText)
    {
        var lines = helpText.Split('\n');

        // Skip the usage line and look for the first descriptive line
        var foundUsage = false;
        foreach (var line in lines)
        {
            var trimmed = line.Trim();

            if (trimmed.StartsWith("pnpm", StringComparison.OrdinalIgnoreCase) ||
                trimmed.StartsWith("Usage:", StringComparison.OrdinalIgnoreCase))
            {
                foundUsage = true;
                continue;
            }

            if (!foundUsage)
            {
                continue;
            }

            // Skip empty lines
            if (string.IsNullOrWhiteSpace(trimmed))
            {
                continue;
            }

            // Skip option lines
            if (trimmed.StartsWith('-'))
            {
                continue;
            }

            // Skip section headers
            if (trimmed.EndsWith(':'))
            {
                continue;
            }

            // This looks like a description
            if (trimmed.Length > 10)
            {
                return trimmed;
            }
        }

        return null;
    }

    /// <summary>
    /// Parses options from pnpm help text.
    /// Format: -D, --save-dev                    Description
    ///         --filter &lt;pattern&gt;              Description
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(string helpText, string[] commandParts)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find "Options:" section
        var optionsMatch = OptionsSectionPattern().Match(helpText);
        if (!optionsMatch.Success)
        {
            return options;
        }

        var sectionStart = optionsMatch.Index + optionsMatch.Length;
        var sectionEnd = helpText.Length;

        // Find where this section ends
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

            // Match pnpm option patterns
            var match = PnpmOptionPattern().Match(line);
            if (!match.Success)
            {
                continue;
            }

            var shortForm = match.Groups["short"].Value.Trim();
            var longForm = match.Groups["long"].Value.Trim();
            var valueHint = match.Groups["value"].Value.Trim();
            var description = match.Groups["desc"].Value.Trim();

            if (string.IsNullOrEmpty(longForm))
            {
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

            // pnpm normally represents value-taking options with an explicit placeholder,
            // but some options omit it from their rendered help text.
            var isFlag = string.IsNullOrEmpty(valueHint)
                         && !IsPlaceholderFreeValueOption(longForm, description);
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
                ValueSeparator = " ",
                EnumDefinition = null,
                IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag)
            });
        }

        return options;
    }

    private static bool IsPlaceholderFreeValueOption(string optionName, string description)
    {
        if (PlaceholderFreeFlagOptions.Contains(optionName))
        {
            return false;
        }

        if (PlaceholderFreeValueOptions.Contains(optionName))
        {
            return true;
        }

        var normalizedName = optionName.TrimStart('-');
        if (normalizedName.StartsWith("no-", StringComparison.OrdinalIgnoreCase)
            || normalizedName.StartsWith("ignore-", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return ValueOptionNameIndicators.Any(part =>
                   normalizedName.Contains(part, StringComparison.OrdinalIgnoreCase))
               || ValueDescriptionIndicators.Any(part =>
                   description.Contains(part, StringComparison.OrdinalIgnoreCase));
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
    /// Matches command lines in pnpm help output.
    /// Format: "      add                  Installs a package..."
    /// </summary>
    [GeneratedRegex(@"^\s{4,}(?<command>[\w-]+)\s{2,}", RegexOptions.Multiline)]
    private static partial Regex CommandLinePattern();

    /// <summary>
    /// Matches "Commands:" section header.
    /// </summary>
    [GeneratedRegex(@"Commands?:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex CommandsSectionPattern();

    /// <summary>
    /// Matches "Options:" section.
    /// </summary>
    [GeneratedRegex(@"Options:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex OptionsSectionPattern();

    /// <summary>
    /// Matches next section headers.
    /// </summary>
    [GeneratedRegex(@"\n[A-Z][\w\s]*:\s*\n")]
    private static partial Regex NextSectionPattern();

    /// <summary>
    /// Matches subcommand lines.
    /// </summary>
    [GeneratedRegex(@"^\s{2,}(?<name>[\w-]+)\s{2,}", RegexOptions.Multiline)]
    private static partial Regex SubcommandLinePattern();

    /// <summary>
    /// Matches pnpm-style option lines:
    /// -D, --save-dev                    Description
    /// --filter &lt;pattern&gt;              Description
    /// </summary>
    [GeneratedRegex(@"^\s*(?:(?<short>-\w),\s*)?(?<long>--[\w-]+)(?:\s+(?<value><[^>]+>|\[[^\]]+\]))?\s{2,}(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex PnpmOptionPattern();

    #endregion
}
