using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
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
                    CSharpType = "string?",
                    IsRequired = false,
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

        // Find "optional arguments:" or "options:" section
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
            var match = AnsibleOptionPattern().Match(line);
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

            var isFlag = string.IsNullOrEmpty(valueHint);
            var csharpType = isFlag ? "bool?" : "string?";

            // Handle common array-type options
            if (longForm is "--extra-vars" or "-e" or "--limit" or "-l" or "--tags" or "-t" or "--skip-tags")
            {
                csharpType = "IEnumerable<string>?";
            }

            options.Add(new CliOptionDefinition
            {
                SwitchName = longForm,
                ShortForm = string.IsNullOrEmpty(shortForm) ? null : shortForm,
                PropertyName = propertyName,
                CSharpType = csharpType,
                Description = description,
                IsFlag = isFlag,
                IsRequired = false,
                AcceptsMultipleValues = csharpType.Contains("IEnumerable"),
                IsKeyValue = false,
                IsNumeric = false,
                ValueSeparator = " ",
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
        return helpText.Contains("optional arguments:") || helpText.Contains("options:");
    }

    #region Regex Patterns

    /// <summary>
    /// Matches "optional arguments:" or "options:" section.
    /// </summary>
    [GeneratedRegex(@"(?:optional arguments|options):\s*\n", RegexOptions.IgnoreCase)]
    private static partial Regex OptionsSectionPattern();

    /// <summary>
    /// Matches ansible-style option lines:
    ///   -h, --help            show this help message
    ///   -v, --verbose         verbose mode
    ///   -i INVENTORY, --inventory INVENTORY
    /// </summary>
    [GeneratedRegex(@"^\s+(?:(?<short>-\w),\s+)?(?<long>--[\w-]+)(?:\s+(?<value>[A-Z_]+)(?:,\s+--[\w-]+\s+[A-Z_]+)?)?\s*(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex AnsibleOptionPattern();

    #endregion
}
