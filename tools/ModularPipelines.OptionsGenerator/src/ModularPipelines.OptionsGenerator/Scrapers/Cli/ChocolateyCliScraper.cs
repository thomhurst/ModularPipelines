using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Chocolatey CLI.
/// Chocolatey is Windows-specific and uses -? for help with a distinctive format.
///
/// Chocolatey help format:
/// Chocolatey v2.x
///
/// Commands
/// =========
///  * list - lists remote or local packages
///  * search - searches remote or local packages
///  * info - retrieves package information
///  * install - installs packages
///  ...
///
/// Subcommand help (choco install -?):
/// Usage
/// =====
///     choco install &lt;pkg|packages.config&gt; [&lt;options/switches&gt;]
///
/// Options and Switches
/// ====================
///  -?, --help, -h
///      Prints out the help menu.
///
///  -d, --debug
///      Debug - Show debug messaging.
///
///      --source=VALUE
///      Source - The source to find the package(s).
/// </summary>
public partial class ChocolateyCliScraper : CliScraperBase
{
    public ChocolateyCliScraper(ICliCommandExecutor executor, ILogger<ChocolateyCliScraper> logger)
        : base(executor, logger)
    {
    }

    public override string ToolName => "choco";

    public override string NamespacePrefix => "Choco";

    public override string TargetNamespace => "ModularPipelines.Chocolatey";

    public override string OutputDirectory => "src/ModularPipelines.Chocolatey";

    /// <summary>
    /// Chocolatey has flat command structure.
    /// </summary>
    protected override int MaxDiscoveryDepth => 2;

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "-?", "--help", "-h", "--version", "help"
    };

    /// <summary>
    /// Chocolatey is only available on Windows.
    /// </summary>
    public override async Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Logger.LogDebug("Chocolatey is only available on Windows");
            return false;
        }

        return await base.IsAvailableAsync(cancellationToken);
    }

    /// <summary>
    /// Chocolatey uses -? instead of --help.
    /// </summary>
    protected override async Task<string?> GetHelpTextAsync(string[] commandPath, CancellationToken cancellationToken)
    {
        var cacheKey = string.Join(" ", commandPath);

        // Build the arguments: everything after the tool name, plus -?
        var args = commandPath.Length > 1
            ? string.Join(" ", commandPath.Skip(1)) + " -?"
            : "-?";

        var result = await Executor.ExecuteAsync(ExecutablePath, args, cancellationToken);

        // Chocolatey outputs help to stdout
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
    /// Extracts subcommand names from Chocolatey help text.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        var seenCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find "Commands" section (followed by ===== underline)
        var commandSectionMatch = CommandSectionPattern().Match(helpText);
        if (!commandSectionMatch.Success)
        {
            return subcommands;
        }

        var sectionStart = commandSectionMatch.Index + commandSectionMatch.Length;

        // Find where this section ends (next section with ===== or end of text)
        var sectionEnd = helpText.Length;
        var nextSectionMatch = NextSectionPattern().Match(helpText, sectionStart);
        if (nextSectionMatch.Success)
        {
            sectionEnd = nextSectionMatch.Index;
        }

        var section = helpText.Substring(sectionStart, sectionEnd - sectionStart);

        // Parse command lines: " * command - description" or "  command    description"
        var lines = section.Split('\n');
        foreach (var line in lines)
        {
            // Try bullet point format first: " * command - description"
            var match = BulletCommandLinePattern().Match(line);
            if (match.Success)
            {
                var commandName = match.Groups["name"].Value.Trim();
                if (!string.IsNullOrEmpty(commandName) && seenCommands.Add(commandName))
                {
                    subcommands.Add(commandName);
                }
                continue;
            }

            // Try indented format: "  command    description"
            match = SubcommandLinePattern().Match(line);
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

        return subcommands;
    }

    /// <summary>
    /// Parses a Chocolatey command from its help text.
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

        // Look for description after Usage section
        var foundUsage = false;
        foreach (var line in lines)
        {
            var trimmed = line.Trim();

            // Skip Chocolatey header
            if (trimmed.StartsWith("Chocolatey v"))
            {
                continue;
            }

            if (trimmed.StartsWith("Usage"))
            {
                foundUsage = true;
                continue;
            }

            // Skip underlines and empty lines
            if (string.IsNullOrEmpty(trimmed) || trimmed.All(c => c == '='))
            {
                continue;
            }

            // Skip command usage lines
            if (trimmed.StartsWith("choco "))
            {
                continue;
            }

            // Skip section headers
            if (trimmed.EndsWith(':') || NextSectionPattern().IsMatch(trimmed))
            {
                continue;
            }

            // Found a description line
            if (foundUsage && trimmed.Length > 10)
            {
                return trimmed;
            }
        }

        return null;
    }

    /// <summary>
    /// Parses options from Chocolatey help text.
    /// Format variations:
    ///  -?, --help, -h
    ///      Prints out the help menu.
    ///
    ///      --source=VALUE
    ///      Source - The source to find the package(s).
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(string helpText, string[] commandParts)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var className = GenerateClassName([ToolName, .. commandParts]);

        // Find "Options and Switches" section
        var optionsMatch = OptionsSectionPattern().Match(helpText);
        if (!optionsMatch.Success)
        {
            return options;
        }

        var sectionStart = optionsMatch.Index + optionsMatch.Length;

        // Find where this section ends
        var sectionEnd = helpText.Length;
        var nextSectionMatch = NextSectionPattern().Match(helpText, sectionStart);
        if (nextSectionMatch.Success)
        {
            sectionEnd = nextSectionMatch.Index;
        }

        var section = helpText.Substring(sectionStart, sectionEnd - sectionStart);
        var lines = section.Split('\n');

        string? currentLongForm = null;
        string? currentShortForm = null;
        var currentDescription = new List<string>();
        var hasValue = false;

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var trimmed = line.Trim();

            // Skip empty lines and underlines
            if (string.IsNullOrWhiteSpace(trimmed) || trimmed.All(c => c == '='))
            {
                // If we have a pending option, save it
                if (currentLongForm is not null)
                {
                    var option = CreateOption(currentLongForm, currentShortForm, string.Join(" ", currentDescription), hasValue, seenOptions, className);
                    if (option is not null)
                    {
                        options.Add(option);
                    }
                    currentLongForm = null;
                    currentShortForm = null;
                    currentDescription.Clear();
                    hasValue = false;
                }
                continue;
            }

            // Check if this is an option line
            var optionMatch = ChocolateyOptionPattern().Match(trimmed);
            if (optionMatch.Success)
            {
                // Save previous option if any
                if (currentLongForm is not null)
                {
                    var option = CreateOption(currentLongForm, currentShortForm, string.Join(" ", currentDescription), hasValue, seenOptions, className);
                    if (option is not null)
                    {
                        options.Add(option);
                    }
                    currentDescription.Clear();
                }

                // Parse new option
                var flags = optionMatch.Groups["flags"].Value;
                hasValue = flags.Contains("=VALUE");
                flags = flags.Replace("=VALUE", "");

                // Extract long and short forms
                var parts = flags.Split(',').Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p)).ToList();
                currentLongForm = parts.FirstOrDefault(p => p.StartsWith("--"));
                currentShortForm = parts.FirstOrDefault(p => p.StartsWith("-") && !p.StartsWith("--"));

                if (currentLongForm is null && currentShortForm is not null)
                {
                    // Use short form as the main form if no long form
                    currentLongForm = currentShortForm;
                    currentShortForm = null;
                }
            }
            else if (currentLongForm is not null)
            {
                // This is a description line
                currentDescription.Add(trimmed);
            }
        }

        // Don't forget the last option
        if (currentLongForm is not null)
        {
            var option = CreateOption(currentLongForm, currentShortForm, string.Join(" ", currentDescription), hasValue, seenOptions, className);
            if (option is not null)
            {
                options.Add(option);
            }
        }

        return options;
    }

    /// <summary>
    /// Creates a CLI option definition.
    /// </summary>
    private static CliOptionDefinition? CreateOption(
        string longForm,
        string? shortForm,
        string description,
        bool hasValue,
        HashSet<string> seenOptions,
        string className)
    {
        if (string.IsNullOrEmpty(longForm))
        {
            return null;
        }

        // Skip duplicates
        if (seenOptions.Contains(longForm))
        {
            return null;
        }

        seenOptions.Add(longForm);

        var propertyName = NormalizePropertyName(longForm);
        if (propertyName is null)
        {
            return null;
        }

        // Determine type based on whether it has =VALUE suffix
        var isFlag = !hasValue;
        var csharpType = isFlag ? "bool?" : "string?";

        return new CliOptionDefinition
        {
            SwitchName = longForm,
            ShortForm = shortForm,
            PropertyName = propertyName,
            CSharpType = csharpType,
            Description = description,
            IsFlag = isFlag,
            IsRequired = false,
            AcceptsMultipleValues = false,
            IsKeyValue = false,
            IsNumeric = false,
            ValueSeparator = isFlag ? " " : "=",
            EnumDefinition = null
        };
    }

    /// <summary>
    /// Checks if help text indicates the command has options.
    /// </summary>
    protected override bool HasOptions(string helpText)
    {
        return helpText.Contains("Options and Switches") ||
               helpText.Contains("--") ||
               helpText.Contains("-?");
    }

    #region Regex Patterns

    /// <summary>
    /// Matches "Commands" section followed by ===== underline.
    /// </summary>
    [GeneratedRegex(@"Commands\s*\n\s*=+\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex CommandSectionPattern();

    /// <summary>
    /// Matches "Options and Switches" section followed by ===== underline.
    /// </summary>
    [GeneratedRegex(@"Options and Switches\s*\n\s*=+\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex OptionsSectionPattern();

    /// <summary>
    /// Matches next section headers (with ===== underline).
    /// </summary>
    [GeneratedRegex(@"\n[A-Z][\w\s]*\n\s*=+", RegexOptions.IgnoreCase)]
    private static partial Regex NextSectionPattern();

    /// <summary>
    /// Matches bullet point command lines: " * command - description"
    /// </summary>
    [GeneratedRegex(@"^\s*\*\s*(?<name>[\w-]+)\s*-", RegexOptions.Multiline)]
    private static partial Regex BulletCommandLinePattern();

    /// <summary>
    /// Matches subcommand lines: "  command    description"
    /// </summary>
    [GeneratedRegex(@"^\s{2,}(?<name>[\w-]+)\s{2,}", RegexOptions.Multiline)]
    private static partial Regex SubcommandLinePattern();

    /// <summary>
    /// Matches Chocolatey-style option lines:
    /// -?, --help, -h
    /// --source=VALUE
    /// </summary>
    [GeneratedRegex(@"^(?<flags>(?:-[\w?],?\s*)*(?:--[\w-]+(?:=VALUE)?))$")]
    private static partial Regex ChocolateyOptionPattern();

    #endregion
}
