using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for WinGet (Windows Package Manager) CLI.
/// WinGet is Windows-specific and uses standard --help format.
///
/// WinGet help format:
/// Windows Package Manager v1.x
/// Copyright (c) Microsoft Corporation. All rights reserved.
///
/// The winget command line utility enables installing applications...
///
/// usage: winget [&lt;command&gt;] [&lt;options&gt;]
///
/// The following commands are available:
///   install    Installs the given package
///   show       Shows information about a package
///   search     Find and show basic info of packages
///
/// Subcommand help (winget install --help):
/// Installs the given package
///
/// usage: winget install [[-q] &lt;query&gt;] [&lt;options&gt;]
///
/// The following arguments are available:
///   -q,--query                       The query used to search for a package
///
/// The following options are available:
///   -m,--manifest                    The path to the manifest
///   --id                             Filter results by id
///   --name                           Filter results by name
/// </summary>
public partial class WinGetCliScraper : CliScraperBase
{
    public WinGetCliScraper(ICliCommandExecutor executor, ILogger<WinGetCliScraper> logger)
        : base(executor, logger)
    {
    }

    public override string ToolName => "winget";

    public override string NamespacePrefix => "Winget";

    public override string TargetNamespace => "ModularPipelines.WinGet";

    public override string OutputDirectory => "src/ModularPipelines.WinGet";

    /// <summary>
    /// WinGet has flat command structure with no deep nesting.
    /// </summary>
    protected override int MaxDiscoveryDepth => 2;

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--version", "-v", "--info", "--help", "-?"
    };

    /// <summary>
    /// WinGet is only available on Windows.
    /// </summary>
    public override async Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Logger.LogDebug("WinGet is only available on Windows");
            return false;
        }

        return await base.IsAvailableAsync(cancellationToken);
    }

    /// <summary>
    /// Extracts subcommand names from WinGet help text.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        var seenCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find "The following commands are available:" section
        var commandSectionMatch = CommandSectionPattern().Match(helpText);
        if (!commandSectionMatch.Success)
        {
            return subcommands;
        }

        var sectionStart = commandSectionMatch.Index + commandSectionMatch.Length;

        // Find where this section ends (next major section or end of text)
        var sectionEnd = helpText.Length;
        var nextSectionMatch = NextSectionPattern().Match(helpText, sectionStart);
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

        return subcommands;
    }

    /// <summary>
    /// Parses a WinGet command from its help text.
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

        // Parse description from help text (first non-empty line before usage)
        var description = ExtractDescription(helpText);

        // Parse options from the help text
        var options = ParseOptions(helpText, commandParts);

        // Parse arguments from the help text
        var arguments = ParseArguments(helpText);
        options.AddRange(arguments);

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

        foreach (var line in lines)
        {
            var trimmed = line.Trim();

            // Skip empty lines
            if (string.IsNullOrEmpty(trimmed))
            {
                continue;
            }

            // Skip copyright/version lines
            if (trimmed.StartsWith("Windows Package Manager") ||
                trimmed.StartsWith("Copyright") ||
                trimmed.StartsWith("usage:"))
            {
                continue;
            }

            // Skip section headers
            if (trimmed.StartsWith("The following") ||
                trimmed.EndsWith(':'))
            {
                continue;
            }

            // Found a description line
            if (trimmed.Length > 10)
            {
                return trimmed;
            }
        }

        return null;
    }

    /// <summary>
    /// Parses options from WinGet help text.
    /// Format: -short,--long     Description
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(string helpText, string[] commandParts)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var className = GenerateClassName([ToolName, .. commandParts]);

        // Find "The following options are available:" section
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

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var option = ParseOptionLine(line, seenOptions, className, ref i, lines);
            if (option is not null)
            {
                options.Add(option);
            }
        }

        return options;
    }

    /// <summary>
    /// Parses arguments from WinGet help text.
    /// Format: -short,--long     Description
    /// </summary>
    private List<CliOptionDefinition> ParseArguments(string helpText)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find "The following arguments are available:" section
        var argsMatch = ArgumentsSectionPattern().Match(helpText);
        if (!argsMatch.Success)
        {
            return options;
        }

        var sectionStart = argsMatch.Index + argsMatch.Length;

        // Find where this section ends
        var sectionEnd = helpText.Length;
        var nextSectionMatch = NextSectionPattern().Match(helpText, sectionStart);
        if (nextSectionMatch.Success)
        {
            sectionEnd = nextSectionMatch.Index;
        }

        var section = helpText.Substring(sectionStart, sectionEnd - sectionStart);
        var lines = section.Split('\n');

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var option = ParseOptionLine(line, seenOptions, "Winget", ref i, lines);
            if (option is not null)
            {
                options.Add(option);
            }
        }

        return options;
    }

    /// <summary>
    /// Parses a single option line.
    /// </summary>
    private static CliOptionDefinition? ParseOptionLine(
        string line,
        HashSet<string> seenOptions,
        string className,
        ref int lineIndex,
        string[] allLines)
    {
        var match = WinGetOptionPattern().Match(line);
        if (!match.Success)
        {
            return null;
        }

        var shortForm = match.Groups["short"].Value.Trim();
        var longForm = match.Groups["long"].Value.Trim();
        var description = match.Groups["desc"].Value.Trim();

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

        // Accumulate multi-line descriptions
        lineIndex = AccumulateMultiLineDescription(allLines, lineIndex, ref description);

        var propertyName = NormalizePropertyName(longForm);
        if (propertyName is null)
        {
            return null;
        }

        // WinGet options are generally strings or booleans
        // Boolean flags typically have descriptions like "Enable...", "Disable...", etc.
        var isFlag = IsBooleanDescription(description);
        var csharpType = isFlag ? "bool?" : "string?";

        return new CliOptionDefinition
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
        };
    }

    /// <summary>
    /// Checks if description indicates a boolean flag.
    /// </summary>
    private static bool IsBooleanDescription(string description)
    {
        if (string.IsNullOrEmpty(description))
        {
            return false;
        }

        var lowerDesc = description.ToLowerInvariant();
        return lowerDesc.Contains("enable") ||
               lowerDesc.Contains("disable") ||
               lowerDesc.Contains("force") ||
               lowerDesc.Contains("silent") ||
               lowerDesc.Contains("interactive") ||
               lowerDesc.Contains("accept") ||
               lowerDesc.Contains("ignore") ||
               lowerDesc.Contains("skip") ||
               lowerDesc.Contains("purge") ||
               lowerDesc.Contains("all user") ||
               lowerDesc.StartsWith("use ");
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
            if (trimmedNext.StartsWith("The following"))
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
        return helpText.Contains("The following options are available:") ||
               helpText.Contains("The following arguments are available:") ||
               helpText.Contains("--");
    }

    #region Regex Patterns

    /// <summary>
    /// Matches "The following commands are available:" section.
    /// </summary>
    [GeneratedRegex(@"The following commands are available:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex CommandSectionPattern();

    /// <summary>
    /// Matches "The following options are available:" section.
    /// </summary>
    [GeneratedRegex(@"The following options are available:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex OptionsSectionPattern();

    /// <summary>
    /// Matches "The following arguments are available:" section.
    /// </summary>
    [GeneratedRegex(@"The following arguments are available:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex ArgumentsSectionPattern();

    /// <summary>
    /// Matches next section patterns (to find section boundaries).
    /// </summary>
    [GeneratedRegex(@"(?:The following|For more|usage:|More help)", RegexOptions.IgnoreCase)]
    private static partial Regex NextSectionPattern();

    /// <summary>
    /// Matches subcommand lines: "  command    description"
    /// </summary>
    [GeneratedRegex(@"^\s{2,}(?<name>[\w-]+)\s{2,}", RegexOptions.Multiline)]
    private static partial Regex SubcommandLinePattern();

    /// <summary>
    /// Matches WinGet-style option lines:
    /// -q,--query                       Description
    /// --id                             Description
    /// </summary>
    [GeneratedRegex(@"^\s*(?:(?<short>-\w),)?(?<long>--[\w-]+)\s{2,}(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex WinGetOptionPattern();

    #endregion
}
