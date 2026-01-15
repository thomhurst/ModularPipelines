using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for HashiCorp Packer - machine image builder.
/// Packer uses a custom help format.
///
/// packer help format (packer --help):
/// Usage: packer [--version] [--help] <command> [<args>]
///
/// Available commands are:
///     build       build image(s) from template
///     console     creates a console for testing variable interpolation
///     fix         fixes templates from old versions
///     ...
/// </summary>
public partial class PackerCliScraper : CliScraperBase
{
    public PackerCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<PackerCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "packer";

    public override string NamespacePrefix => "Packer";

    public override string TargetNamespace => "ModularPipelines.Packer";

    public override string OutputDirectory => "src/ModularPipelines.Packer";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "version"
    };

    /// <summary>
    /// Extracts subcommand names from packer help text.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        var seenCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find "Available commands are:" section
        var commandsSectionMatch = CommandsSectionPattern().Match(helpText);
        if (commandsSectionMatch.Success)
        {
            var sectionStart = commandsSectionMatch.Index + commandsSectionMatch.Length;
            var section = helpText.Substring(sectionStart);
            var lines = section.Split('\n');

            foreach (var line in lines)
            {
                var match = CommandLinePattern().Match(line);
                if (match.Success)
                {
                    var commandName = match.Groups["name"].Value.Trim();
                    if (!string.IsNullOrEmpty(commandName) &&
                        !commandName.StartsWith('-') &&
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
    /// Parses a packer command from its help text.
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
            DocumentationUrl = "https://developer.hashicorp.com/packer/docs/commands",
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

            if (trimmed.StartsWith("Usage:", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (trimmed.Length > 10 && !trimmed.Contains("--") && !trimmed.StartsWith('-'))
            {
                return trimmed;
            }
        }

        return null;
    }

    /// <summary>
    /// Parses options from packer help text.
    /// Format: -color=false       Disables colored output
    ///         -debug             Debug mode enabled
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(string helpText)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Find "Options:" section
        var optionsSectionMatch = OptionsSectionPattern().Match(helpText);
        string section;
        if (optionsSectionMatch.Success)
        {
            var sectionStart = optionsSectionMatch.Index + optionsSectionMatch.Length;
            section = helpText.Substring(sectionStart);
        }
        else
        {
            section = helpText;
        }

        var lines = section.Split('\n');

        foreach (var line in lines)
        {
            var match = PackerOptionPattern().Match(line);
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

            var longForm = flagName.StartsWith("--") ? flagName : $"--{flagName.TrimStart('-')}";

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

            var isFlag = string.IsNullOrEmpty(valueHint) || valueHint.Contains("true") || valueHint.Contains("false");
            var csharpType = isFlag ? "bool?" : "string?";

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
                ValueSeparator = "=",
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
        return helpText.Contains("Options:") || helpText.Contains("-");
    }

    #region Regex Patterns

    /// <summary>
    /// Matches "Available commands are:" section.
    /// </summary>
    [GeneratedRegex(@"Available commands(?: are)?:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex CommandsSectionPattern();

    /// <summary>
    /// Matches "Options:" section.
    /// </summary>
    [GeneratedRegex(@"Options:\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex OptionsSectionPattern();

    /// <summary>
    /// Matches command lines: "    build       build image(s) from template"
    /// </summary>
    [GeneratedRegex(@"^\s{4,}(?<name>[\w-]+)\s{2,}", RegexOptions.Multiline)]
    private static partial Regex CommandLinePattern();

    /// <summary>
    /// Matches packer-style option lines:
    ///   -color=false       Disables colored output
    ///   -debug             Debug mode enabled
    ///   -var 'key=value'   Variable for templates
    /// </summary>
    [GeneratedRegex(@"^\s+(?<flag>-[\w-]+)(?:=(?<value>\S+)|\s+'[^']+')?\s{2,}(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex PackerOptionPattern();

    #endregion
}
