using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
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
public partial class VaultCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<VaultCliScraper> logger) : CliScraperBase(executor, helpCache, logger)
{
    public override string ToolName => "vault";

    public override string NamespacePrefix => "Vault";

    public override string TargetNamespace => "ModularPipelines.Vault";

    public override string OutputDirectory => "src/ModularPipelines.Vault";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "debug"
    };

    /// <inheritdoc />
    protected override bool PreserveCommandGroupPlaceholders => true;

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
        CancellationToken cancellationToken) =>
        throw new InvalidOperationException("Shared traversal must pass its parsed synopsis.");

    protected override Task<CliCommandDefinition?> ParseCommandAsync(
        string[] commandPath,
        string helpText,
        UsageSynopsisParseResult usage,
        CancellationToken cancellationToken)
    {
        var commandParts = commandPath.Skip(1).ToArray();

        if (commandParts.Length == 0)
        {
            return Task.FromResult<CliCommandDefinition?>(null);
        }

        var description = ExtractDescription(helpText);
        var options = ParseOptions(commandParts, helpText);

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
            PositionalArguments = [.. GetPositionalArguments(usage, options).Select(argument =>
            {
                // Vault command groups use [args] for a trailing sequence without an ellipsis.
                var trailingArgs = argument is { PropertyName: "Args", Phase: CommandLinePhase.Passthrough };
                return argument with
                {
                    CSharpType = trailingArgs
                        ? argument.IsRequired ? "IEnumerable<string>" : "IEnumerable<string>?"
                        : argument.CSharpType,
                    IsVariadic = argument.IsVariadic || trailingArgs,
                    IsSecret = argument.IsSecret
                        || commandParts is ["login"] or ["unwrap"] or ["operator", "unseal"]
                            or ["operator", "generate-root"] or ["operator", "rekey"]
                        || GeneratorUtils.IsSecretOption(argument.PropertyName, false, argument.Description),
                };
            })],
            UsageSynopsis = usage.Synopsis,
            HasOperandTakingUsage = usage.HasOperandTokens,
            SubDomainGroup = null,
            Enums = []
        };

        return Task.FromResult<CliCommandDefinition?>(command);
    }

    /// <summary>
    /// Extracts the first description paragraph before examples and option sections.
    /// </summary>
    private static string? ExtractDescription(string helpText)
    {
        var lines = helpText.Split('\n');

        for (var i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
            {
                continue;
            }

            var start = i;
            var firstLine = lines[start].Trim();
            while (i + 1 < lines.Length && !string.IsNullOrWhiteSpace(lines[i + 1]))
            {
                i++;
            }

            // Usage can wrap; a leading note belongs to the whole paragraph, too.
            if (firstLine.StartsWith("Usage:", StringComparison.OrdinalIgnoreCase)
                || firstLine.StartsWith("Note:", StringComparison.OrdinalIgnoreCase)
                || firstLine.StartsWith("Warning:", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            // A missing description must not fall through into examples or sections.
            if (firstLine.StartsWith('$') || firstLine.StartsWith('-') || firstLine.EndsWith(':'))
            {
                return null;
            }

            var indentation = GetIndentation(lines[start]);
            return string.Join(' ', lines.Skip(start).Take(i - start + 1)
                .TakeWhile(line => GetIndentation(line) == indentation)
                .Select(line => line.Trim()));
        }

        return null;
    }

    /// <summary>
    /// Parses options from vault help text.
    /// Format: -address=<string>    Address of the Vault server
    ///         -format=<string>     Print output in the given format
    /// </summary>
    private static List<CliOptionDefinition> ParseOptions(string[] commandParts, string helpText)
    {
        var options = new List<CliOptionDefinition>();
        var seenOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var lines = helpText.Split('\n');

        for (var i = 0; i < lines.Length; i++)
        {
            var match = VaultOptionPattern().Match(lines[i]);
            if (!match.Success)
            {
                continue;
            }

            var flagName = match.Groups["flag"].Value.Trim();
            var valueHint = match.Groups["value"].Value.Trim();
            var description = AccumulateWrappedDescription(lines, ref i, match.Groups["desc"],
                static line => VaultOptionPattern().IsMatch(line));

            if (string.IsNullOrEmpty(flagName))
            {
                continue;
            }

            if (!seenOptions.Add(flagName))
            {
                continue;
            }

            var propertyName = NormalizePropertyName(flagName);
            if (propertyName is null)
            {
                continue;
            }

            options.Add(CreateOptionDefinition(commandParts, flagName, valueHint, propertyName, description));
        }

        // HCP connect's custom help omits flag declarations; its invocation examples
        // are the only advertised string options. Prefer structured help if upstream adds it.
        if (options.Count == 0 && commandParts is ["hcp", "connect"])
        {
            AddExampleStringOptions(commandParts, lines, options, seenOptions);
        }

        return options;
    }

    private static void AddExampleStringOptions(
        string[] commandParts,
        string[] lines,
        List<CliOptionDefinition> options,
        HashSet<string> seenOptions)
    {
        var prefix = "$ vault " + string.Join(' ', commandParts);
        foreach (var line in lines)
        {
            var example = line.Trim();
            if (!example.StartsWith(prefix, StringComparison.Ordinal))
            {
                continue;
            }

            foreach (Match match in ExampleStringOptionPattern().Matches(example[prefix.Length..]))
            {
                var flagName = match.Groups["flag"].Value;
                var propertyName = NormalizePropertyName(flagName);
                if (propertyName is not null && seenOptions.Add(flagName))
                {
                    options.Add(CreateOptionDefinition(commandParts, flagName, "string", propertyName,
                        $"The {flagName} option."));
                }
            }
        }
    }

    private static CliOptionDefinition CreateOptionDefinition(
        string[] commandParts,
        string flagName,
        string valueHint,
        string propertyName,
        string description)
    {
        var isFlag = string.IsNullOrEmpty(valueHint) || valueHint == "bool";
        // MFA uses StringSliceVar even though its help omits the repeatability sentence.
        // Vault's StringMapVar uses <key=value> without necessarily documenting repetition.
        var repeatable = !isFlag && (valueHint == "key=value" || flagName == "-mfa"
            || IsRepeatableValueOption(description, isFlag));
        var csharpType = AsCSharpType(isFlag ? "bool?" : "string?", repeatable);

        return new CliOptionDefinition
        {
            SwitchName = flagName,
            ShortForm = flagName == "-namespace" && description.Contains("-ns can be used", StringComparison.Ordinal) ? "-ns" : null,
            PropertyName = propertyName,
            CSharpType = csharpType,
            Description = description,
            IsFlag = isFlag,
            IsRequired = DescriptionDeclaresRequiredOption(description),
            AcceptsMultipleValues = repeatable,
            IsKeyValue = false,
            IsNumeric = false,
            ValueSeparator = "=",
            EnumDefinition = null,
            IsSecret = IsSecretOption(commandParts, flagName, propertyName, description, isFlag)
        };
    }

    private static bool IsSecretOption(
        string[] commandParts,
        string flagName,
        string propertyName,
        string description,
        bool isFlag) =>
        !isFlag && (flagName is "-mfa" or "-header" or "-unlock-key" or "-otp" or "-decode"
                    || (commandParts is ["token", "create"] && flagName == "-id")
                    || GeneratorUtils.IsSecretOption(propertyName, isFlag, description));

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
    [GeneratedRegex(@"^[ \t]+(?<flag>-[\w-]+)(?:=<(?<value>[^>\r\n]+)>)?(?:[ \t]{2,}(?<desc>.*))?[ \t]*\r?$", RegexOptions.Multiline)]
    private static partial Regex VaultOptionPattern();

    // Consume consecutive assignments from the start, including each whole quoted value,
    // so child commands, shell pipelines, and flag-looking text inside values are excluded.
    [GeneratedRegex("""\G[ \t]+(?<flag>-[\w-]+)=(?:"[^"\r\n]*"|'[^'\r\n]*'|[^\s"'|;&]+)(?=[ \t]|$)""")]
    private static partial Regex ExampleStringOptionPattern();

    #endregion
}
