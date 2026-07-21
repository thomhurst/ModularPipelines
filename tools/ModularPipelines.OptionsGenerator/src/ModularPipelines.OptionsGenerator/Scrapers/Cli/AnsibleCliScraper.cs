using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Ansible - configuration management and automation.
/// Ansible uses argparse-style help format.
///
/// ansible help format (ansible --help):
/// usage: ansible [-h] [--version] [-v] [-b] ...
///
/// Define and run a single task 'playbook' against a set of hosts
///
/// positional arguments:
///   pattern               host pattern
///
/// optional arguments:
///   -h, --help            show this help message and exit
///   --version             show program's version number and exit
///   -v, --verbose         verbose mode (-vvv for more)
///   ...
/// </summary>
public partial class AnsibleCliScraper : CliScraperBase
{
    public AnsibleCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<AnsibleCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "ansible";

    public override string NamespacePrefix => "Ansible";

    public override string TargetNamespace => "ModularPipelines.Ansible";

    public override string OutputDirectory => "src/ModularPipelines.Ansible";

    /// <summary>
    /// Ansible has multiple executables, we'll generate for main ones.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help"
    };

    /// <summary>
    /// Ansible doesn't have traditional subcommands - it has separate executables.
    /// We only scrape the main 'ansible' command.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        return [];
    }

    /// <summary>
    /// Parses the ansible command from its help text.
    /// </summary>
    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        CancellationToken cancellationToken)
    {
        var commandParts = commandPath.Skip(1).ToArray();

        // For the root command, create the options class
        var description = "Define and run a single task 'playbook' against a set of hosts";
        var options = ParseOptions(helpText);

        var className = commandParts.Length == 0 ? "AnsibleOptions" : GenerateClassName(commandPath);

        var command = new CliCommandDefinition
        {
            FullCommand = "ansible",
            CommandParts = commandParts.Length == 0 ? [] : commandParts,
            ClassName = className,
            ParentClassName = BaseOptionsClassName,
            ToolNamespacePrefix = NamespacePrefix,
            Description = description,
            DocumentationUrl = "https://docs.ansible.com/ansible/latest/cli/ansible.html",
            Options = options,
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "Pattern",
                    PlaceholderName = "pattern",
                    CSharpType = "string",
                    IsRequired = true,
                    PositionIndex = 0,
                    Description = "Host pattern"
                }
            ],
            SubDomainGroup = null,
            Enums = []
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    /// <summary>
    /// Parses options from ansible help text.
    /// Format: -h, --help            show this help message
    ///         -v, --verbose         verbose mode
    ///         -i INVENTORY, --inventory INVENTORY
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(string helpText)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var lines = GetOptionsSection(helpText).Split('\n');

        for (var i = 0; i < lines.Length; i++)
        {
            var match = AnsibleOptionPattern().Match(lines[i].TrimEnd('\r'));
            if (!match.Success)
            {
                continue;
            }

            var description = match.Groups["desc"].Value.Trim();
            i = AccumulateMultiLineDescription(lines, i, ref description);
            var option = ParseOption(match.Groups["spec"].Value, description);
            if (option is not null && seenOptions.Add(option.SwitchName))
            {
                options.Add(option);
            }
        }

        return options;
    }

    private static string GetOptionsSection(string helpText)
    {
        var match = OptionsSectionPattern().Match(helpText);
        return match.Success ? helpText[(match.Index + match.Length)..] : helpText;
    }

    private CliOptionDefinition? ParseOption(string specification, string description)
    {
        var optionParts = specification.Split(
            ',',
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var longOption = optionParts.FirstOrDefault(part => part.StartsWith("--", StringComparison.Ordinal));
        var longForm = longOption is null ? null : GetOptionName(longOption);
        var propertyName = longForm is null ? null : NormalizePropertyName(longForm);
        if (longForm is null || propertyName is null)
        {
            return null;
        }

        var shortOption = optionParts.FirstOrDefault(part =>
            part.StartsWith('-') && !part.StartsWith("--", StringComparison.Ordinal));
        var valueHint = optionParts.Select(GetValueHint).FirstOrDefault(value => value is not null);
        var isFlag = valueHint is null;
        var isCountedFlag = isFlag && longForm == "--verbose";
        var acceptsMultipleValues = !isFlag && description.Contains(
            "specified multiple times",
            StringComparison.OrdinalIgnoreCase);
        var isNumeric = !isFlag && IsNumericOption(longForm);

        return new CliOptionDefinition
        {
            SwitchName = longForm,
            ShortForm = shortOption is null ? null : GetOptionName(shortOption),
            PropertyName = propertyName,
            CSharpType = GetCSharpType(isFlag, isCountedFlag, acceptsMultipleValues, isNumeric),
            Description = description,
            IsFlag = isFlag,
            IsRequired = false,
            AcceptsMultipleValues = acceptsMultipleValues,
            IsKeyValue = false,
            IsNumeric = isNumeric,
            ValueSeparator = " ",
            EnumDefinition = null,
            IsSecret = GeneratorUtils.IsSecretOption(propertyName, isFlag),
            ValidationConstraints = isCountedFlag
                ? new CliValidationConstraints { MinValue = 0, MaxValue = 6 }
                : null,
        };
    }

    private static string GetOptionName(string option) => option.Split(' ', 2)[0];

    private static string? GetValueHint(string option)
    {
        var parts = option.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        return parts.Length == 2 ? parts[1] : null;
    }

    private static bool IsNumericOption(string optionName) => optionName is
        "--background" or
        "--forks" or
        "--poll" or
        "--task-timeout" or
        "--timeout";

    private static string GetCSharpType(
        bool isFlag,
        bool isCountedFlag,
        bool acceptsMultipleValues,
        bool isNumeric)
    {
        if (isCountedFlag)
        {
            return "int?";
        }

        if (isFlag)
        {
            return "bool?";
        }

        if (acceptsMultipleValues)
        {
            return "IEnumerable<string>?";
        }

        return isNumeric ? "int?" : "string?";
    }

    private static int AccumulateMultiLineDescription(
        string[] lines,
        int currentIndex,
        ref string description)
    {
        var descriptionParts = new List<string>();
        if (!string.IsNullOrEmpty(description))
        {
            descriptionParts.Add(description);
        }

        var nextIndex = currentIndex + 1;
        while (nextIndex < lines.Length)
        {
            var nextLine = lines[nextIndex].TrimEnd('\r');
            var trimmedLine = nextLine.Trim();
            if (trimmedLine.Length == 0 ||
                AnsibleOptionPattern().IsMatch(nextLine) ||
                nextLine.Length == nextLine.TrimStart().Length)
            {
                break;
            }

            descriptionParts.Add(trimmedLine);
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
        return helpText.Contains("optional arguments:") || helpText.Contains("options:");
    }

    #region Regex Patterns

    /// <summary>
    /// Matches "optional arguments:" or "options:" section.
    /// </summary>
    [GeneratedRegex(@"(?:optional arguments|options):\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex OptionsSectionPattern();

    /// <summary>
    /// Matches ansible-style option specifications and an optional inline description:
    ///   -h, --help            show this help message
    ///   -i INVENTORY, --inventory INVENTORY
    ///   --vault-id VAULT_IDS
    /// </summary>
    [GeneratedRegex(@"^ {2}(?<spec>-\S(?:.*?\S)?)(?:\s{2,}(?<desc>\S.*))?$")]
    private static partial Regex AnsibleOptionPattern();

    #endregion
}
