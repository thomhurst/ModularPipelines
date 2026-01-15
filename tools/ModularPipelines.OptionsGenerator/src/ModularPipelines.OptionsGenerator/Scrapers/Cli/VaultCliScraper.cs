using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for HashiCorp Vault - secrets management.
/// Vault uses a custom help format.
///
/// vault help format (vault --help):
/// Usage: vault <command> [args]
///
/// Common commands:
///     read        Read data and retrieves secrets
///     write       Write data, configuration, and secrets
///     delete      Delete secrets and configuration
///     ...
///
/// Other commands:
///     audit       Interact with audit devices
///     auth        Interact with auth methods
///     ...
/// </summary>
public partial class VaultCliScraper : CliScraperBase
{
    public VaultCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<VaultCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "vault";

    public override string NamespacePrefix => "Vault";

    public override string TargetNamespace => "ModularPipelines.Vault";

    public override string OutputDirectory => "src/ModularPipelines.Vault";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "version", "debug"
    };

    /// <summary>
    /// Extracts subcommand names from vault help text.
    /// </summary>
    protected override IEnumerable<string> ExtractSubcommands(string helpText)
    {
        var subcommands = new List<string>();
        var seenCommands = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var lines = helpText.Split('\n');

        foreach (var line in lines)
        {
            var match = CommandLinePattern().Match(line);
            if (match.Success)
            {
                var commandName = match.Groups["name"].Value.Trim();
                if (!string.IsNullOrEmpty(commandName) &&
                    !commandName.StartsWith('-') &&
                    commandName.All(c => char.IsLower(c) || c == '-') &&
                    seenCommands.Add(commandName))
                {
                    subcommands.Add(commandName);
                }
            }
        }

        return subcommands;
    }

    /// <summary>
    /// Parses a vault command from its help text.
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
            DocumentationUrl = "https://developer.hashicorp.com/vault/docs/commands",
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

            if (trimmed.EndsWith(':'))
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
    /// Parses options from vault help text.
    /// Format: -address=<string>    Address of the Vault server
    ///         -format=<string>     Print output in the given format
    /// </summary>
    private List<CliOptionDefinition> ParseOptions(string helpText)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var lines = helpText.Split('\n');

        foreach (var line in lines)
        {
            var match = VaultOptionPattern().Match(line);
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

            var isFlag = string.IsNullOrEmpty(valueHint) || valueHint == "bool";
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
        return helpText.Contains("-address") || helpText.Contains("-format") || VaultOptionPattern().IsMatch(helpText);
    }

    #region Regex Patterns

    /// <summary>
    /// Matches command lines: "    read        Read data and retrieves secrets"
    /// </summary>
    [GeneratedRegex(@"^\s{4,}(?<name>[\w-]+)\s{2,}", RegexOptions.Multiline)]
    private static partial Regex CommandLinePattern();

    /// <summary>
    /// Matches vault-style option lines:
    ///   -address=<string>    Address of the Vault server
    ///   -format=<string>     Print output in the given format
    ///   -tls-skip-verify     Skip TLS verification
    /// </summary>
    [GeneratedRegex(@"^\s+(?<flag>-[\w-]+)(?:=<(?<value>\w+)>)?\s{2,}(?<desc>.*)$", RegexOptions.Multiline)]
    private static partial Regex VaultOptionPattern();

    #endregion
}
