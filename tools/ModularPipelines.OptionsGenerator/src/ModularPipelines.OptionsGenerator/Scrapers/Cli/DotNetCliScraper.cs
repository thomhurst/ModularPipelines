using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for .NET CLI (dotnet).
/// .NET CLI uses standard GNU-style options with --option and -o short forms.
///
/// .NET CLI help format:
/// dotnet --help
/// .NET SDK (version)
///
/// Usage: dotnet [runtime-options] [path-to-application] [arguments]
///
/// Execute a .NET application.
///
/// Options:
///   --additionalprobingpath &lt;path&gt;   Path containing probing policy and assemblies.
///   --additional-deps &lt;path&gt;         Path to additional deps.json file.
///   ...
///
/// SDK commands:
///   add               Add a package or reference to a .NET project.
///   build             Build a .NET project.
///   ...
///
/// dotnet build --help
/// Description:
///   Build a .NET project.
///
/// Usage:
///   dotnet build [&lt;PROJECT | SOLUTION&gt;...] [options]
///
/// Arguments:
///   &lt;PROJECT | SOLUTION&gt;  The project or solution file to operate on.
///
/// Options:
///   -c, --configuration &lt;CONFIGURATION&gt;  The configuration to use.
///   -f, --framework &lt;FRAMEWORK&gt;          The target framework to build for.
///   ...
/// </summary>
public partial class DotNetCliScraper : CliScraperBase
{
    public DotNetCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<DotNetCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "dotnet";

    public override string NamespacePrefix => "DotNet";

    public override string TargetNamespace => "ModularPipelines.DotNet";

    public override string OutputDirectory => "src/ModularPipelines.DotNet";


    /// <summary>
    /// Skip common utility subcommands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "help", "--help", "-h", "--version", "--info", "--list-sdks", "--list-runtimes"
    };

    /// <summary>
    /// Extracts subcommand names from .NET CLI help text.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        var seenCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find command sections (SDK commands:, Additional commands:, Commands:, Subcommands:)
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
    /// Parses a .NET CLI command from its help text.
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

        // Get the first subcommand for grouping (e.g., "nuget" for "nuget push")
        var subDomain = commandParts.Length > 1 ? ToPascalCase(commandParts[0]) : null;

        // Parse description from help text
        var description = ExtractDescription(helpText);

        // Parse options from the help text
        var options = ParseOptions(helpText, commandParts);

        // Parse positional arguments
        var positionalArgs = ParsePositionalArguments(helpText);

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
            PositionalArguments = positionalArgs,
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
        // Look for "Description:" section
        var descMatch = DescriptionSectionPattern().Match(helpText);
        if (descMatch.Success)
        {
            var descStart = descMatch.Index + descMatch.Length;
            var descEnd = helpText.Length;

            // Find where description ends (next section header)
            var nextSection = SectionHeaderPattern().Match(helpText, descStart);
            if (nextSection.Success)
            {
                descEnd = nextSection.Index;
            }

            var description = helpText.Substring(descStart, descEnd - descStart).Trim();
            // Take first line only
            var firstLine = description.Split('\n')[0].Trim();
            if (!string.IsNullOrEmpty(firstLine))
            {
                return firstLine;
            }
        }

        return null;
    }

    /// <summary>
    /// Parses options from .NET CLI help text.
    /// .NET CLI uses GNU-style: --option VALUE, --flag, -o (short forms)
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

            // Match .NET CLI option patterns:
            // -c, --configuration <CONFIGURATION>  Description
            // --no-restore                         Description
            // -v, --verbosity <LEVEL>              Description
            var match = DotNetOptionPattern().Match(line);
            if (!match.Success)
            {
                continue;
            }

            var shortFlag = match.Groups["short"].Value.Trim();
            var longFlag = match.Groups["long"].Value.Trim();
            var valueHint = match.Groups["value"].Value.Trim();
            var description = match.Groups["desc"].Value.Trim();

            // Need at least one flag
            if (string.IsNullOrEmpty(longFlag) && string.IsNullOrEmpty(shortFlag))
            {
                continue;
            }

            // Prefer long flag for naming
            var primaryFlag = !string.IsNullOrEmpty(longFlag) ? longFlag : shortFlag;

            // Skip duplicates
            if (seenOptions.Contains(primaryFlag))
            {
                continue;
            }

            seenOptions.Add(primaryFlag);

            // Skip common global options that are on base class
            if (IsGlobalOption(primaryFlag))
            {
                continue;
            }

            // Accumulate multi-line descriptions
            i = AccumulateMultiLineDescription(lines, i, ref description);

            var propertyName = NormalizePropertyName(primaryFlag);
            if (propertyName is null)
            {
                continue;
            }

            // Determine type based on value hint
            var isFlag = string.IsNullOrEmpty(valueHint);
            var isRequired = false;
            var acceptsMultiple = description.Contains("multiple", StringComparison.OrdinalIgnoreCase) ||
                                  description.Contains("can be specified more than once", StringComparison.OrdinalIgnoreCase);

            var csharpType = DetermineType(valueHint, description, isFlag, acceptsMultiple);

            options.Add(new CliOptionDefinition
            {
                SwitchName = !string.IsNullOrEmpty(longFlag) ? $"--{longFlag}" : $"-{shortFlag}",
                ShortForm = !string.IsNullOrEmpty(shortFlag) && !string.IsNullOrEmpty(longFlag) ? $"-{shortFlag}" : null,
                PropertyName = propertyName,
                CSharpType = csharpType,
                Description = description,
                IsFlag = isFlag,
                IsRequired = isRequired,
                AcceptsMultipleValues = acceptsMultiple,
                IsKeyValue = primaryFlag.Contains("property", StringComparison.OrdinalIgnoreCase),
                IsNumeric = csharpType == "int?",
                ValueSeparator = " ",
                EnumDefinition = null
            });
        }

        return options;
    }

    /// <summary>
    /// Parses positional arguments from help text.
    /// </summary>
    private static List<CliPositionalArgument> ParsePositionalArguments(string helpText)
    {
        var args = new List<CliPositionalArgument>();

        // Find Arguments: section
        var argsMatch = ArgumentsSectionPattern().Match(helpText);
        if (!argsMatch.Success)
        {
            return args;
        }

        var sectionStart = argsMatch.Index + argsMatch.Length;
        var sectionEnd = helpText.Length;

        var nextSection = SectionHeaderPattern().Match(helpText, sectionStart);
        if (nextSection.Success)
        {
            sectionEnd = nextSection.Index;
        }

        var section = helpText.Substring(sectionStart, sectionEnd - sectionStart);
        var lines = section.Split('\n');
        var index = 0;

        foreach (var line in lines)
        {
            var match = PositionalArgPattern().Match(line);
            if (!match.Success)
            {
                continue;
            }

            var argName = match.Groups["name"].Value.Trim();
            var description = match.Groups["desc"].Value.Trim();

            if (string.IsNullOrEmpty(argName))
            {
                continue;
            }

            var propertyName = NormalizePropertyName(argName.Replace(" | ", "_").Replace("|", "_"));

            if (args.All(a => a.PropertyName != propertyName))
            {
                args.Add(new CliPositionalArgument
                {
                    PlaceholderName = argName,
                    PropertyName = propertyName,
                    CSharpType = "string?",
                    Description = description,
                    Placement = PositionalArgumentPosition.AfterOptions,
                    PositionIndex = index++,
                    IsRequired = false
                });
            }
        }

        return args;
    }

    /// <summary>
    /// Determines the C# type based on value hint and description.
    /// </summary>
    private static string DetermineType(string valueHint, string description, bool isFlag, bool acceptsMultiple)
    {
        if (isFlag)
        {
            return "bool?";
        }

        var lowerHint = valueHint.ToLowerInvariant();
        var lowerDesc = description.ToLowerInvariant();

        // Check for numeric types
        if (lowerHint.Contains("count") || lowerHint.Contains("number") ||
            lowerDesc.Contains("integer") || lowerDesc.Contains("number of"))
        {
            return "int?";
        }

        // Check for list types
        if (acceptsMultiple)
        {
            return "IEnumerable<string>?";
        }

        return "string?";
    }

    /// <summary>
    /// Checks if an option is a global option that should be on the base class.
    /// </summary>
    private static bool IsGlobalOption(string optionName)
    {
        var globalOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "help", "h", "version", "diagnostics", "d"
        };

        return globalOptions.Contains(optionName);
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
               helpText.Contains("--help") ||
               DotNetOptionPattern().IsMatch(helpText);
    }

    #region Regex Patterns

    /// <summary>
    /// Matches command section headers: "SDK commands:", "Commands:", etc.
    /// </summary>
    [GeneratedRegex(@"(?:SDK\s+|Additional\s+)?(?:commands|Commands|Subcommands):\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex CommandSectionPattern();

    /// <summary>
    /// Matches section headers like "Options:", "Arguments:", etc.
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
    /// Matches "Arguments:" section header.
    /// </summary>
    [GeneratedRegex(@"^Arguments?:\s*\n", RegexOptions.Multiline | RegexOptions.IgnoreCase)]
    private static partial Regex ArgumentsSectionPattern();

    /// <summary>
    /// Matches "Description:" section header.
    /// </summary>
    [GeneratedRegex(@"^Description:\s*\n", RegexOptions.Multiline | RegexOptions.IgnoreCase)]
    private static partial Regex DescriptionSectionPattern();

    /// <summary>
    /// Matches .NET CLI-style option lines:
    /// -c, --configuration <CONFIGURATION>  Description
    /// --no-restore                         Description
    /// -v, --verbosity <LEVEL>              Description
    /// </summary>
    [GeneratedRegex(@"^\s+(?:-(?<short>\w),\s+)?--(?<long>[\w-]+)(?:\s+<(?<value>[^>]+)>)?\s{2,}(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex DotNetOptionPattern();

    /// <summary>
    /// Matches positional argument lines:
    /// <PROJECT | SOLUTION>  Description
    /// </summary>
    [GeneratedRegex(@"^\s+<(?<name>[^>]+)>\s{2,}(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex PositionalArgPattern();

    #endregion
}
