using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for ArgoCD CLI - GitOps continuous delivery.
/// ArgoCD uses Cobra for its CLI.
///
/// argocd help format (argocd --help):
/// argocd controls a Argo CD server
///
/// Usage:
///   argocd [flags]
///   argocd [command]
///
/// Available Commands:
///   account     Manage account settings
///   admin       Contains a set of commands useful for Argo CD administrators
///   app         Manage applications
///   ...
/// </summary>
public partial class ArgoCdCliScraper : CobraCliScraper
{
    public ArgoCdCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<ArgoCdCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "argocd";

    public override string NamespacePrefix => "ArgoCd";

    public override string TargetNamespace => "ModularPipelines.ArgoCd";

    public override string OutputDirectory => "src/ModularPipelines.ArgoCd";

    /// <summary>
    /// Argo CD calls the ApplicationSet command "appset". Expanding the compound name
    /// prevents it from colliding with the separate "app set" command in generated code.
    /// </summary>
    protected override string NormalizeCommandIdentifier(string commandPart) =>
        commandPart.Equals("appset", StringComparison.OrdinalIgnoreCase)
            ? "ApplicationSet"
            : base.NormalizeCommandIdentifier(commandPart);

    /// <summary>
    /// Argo CD renders required operands as bare uppercase names. Keep that parsing
    /// local so other Cobra tools retain the conservative bracketed-operand parser.
    /// </summary>
    protected override List<CliPositionalArgument> ParsePositionalArguments(string helpText)
    {
        var usageLine = helpText
            .Split('\n')
            .Select(line => line.Trim())
            .FirstOrDefault(line => line.StartsWith("argocd ", StringComparison.Ordinal));

        if (usageLine is null)
        {
            return [];
        }

        var arguments = new List<CliPositionalArgument>();
        foreach (Match match in ArgoCdPositionalArgumentPattern().Matches(usageLine))
        {
            var argumentName = match.Groups["name"].Value;
            var propertyName = NormalizePropertyName(argumentName);
            if (propertyName is null)
            {
                continue;
            }

            var isRequired = !match.Value.StartsWith('[');
            var isMultiple = match.Groups["multiple"].Success;
            var csharpType = isMultiple ? "IEnumerable<string>?" : "string?";

            arguments.Add(new CliPositionalArgument
            {
                PropertyName = propertyName,
                PlaceholderName = argumentName,
                CSharpType = isRequired ? csharpType.TrimEnd('?') : csharpType,
                IsRequired = isRequired,
                PositionIndex = arguments.Count,
                Description = null,
            });
        }

        return arguments;
    }

    /// <summary>
    /// The appset create usage line omits its required file arguments even though the
    /// command accepts one or more filenames or URLs.
    /// </summary>
    protected override IReadOnlyList<CliPositionalArgument> ApplyPositionalArgumentFixes(
        string[] commandParts,
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        if (commandParts is ["app", "sync"] or ["app", "wait"])
        {
            return
            [
                new CliPositionalArgument
                {
                    PropertyName = "ApplicationNames",
                    PlaceholderName = "APPNAME...",
                    CSharpType = "IEnumerable<string>?",
                    IsRequired = false,
                    PositionIndex = 0,
                    Description = "Optional application names to target.",
                },
            ];
        }

        if (commandParts is ["cluster", "get"] or ["cluster", "rm"])
        {
            return
            [
                RequiredArgument(
                    "ServerOrName",
                    "SERVER/NAME",
                    "string",
                    "Cluster server address or configured name."),
            ];
        }

        if (commandParts is ["proj", "add-destination"])
        {
            return
            [
                RequiredArgument("Project", "PROJECT", "string", "Project name.", 0),
                RequiredArgument(
                    "ServerOrName",
                    "SERVER/NAME",
                    "string",
                    "Destination server address or configured name.",
                    1),
                RequiredArgument("Namespace", "NAMESPACE", "string", "Destination namespace.", 2),
            ];
        }

        if (commandParts is ["admin", "settings", "rbac", "can"])
        {
            return
            [
                RequiredArgument("RoleSubject", "ROLE/SUBJECT", "string", "Role or subject to check.", 0),
                RequiredArgument("Action", "ACTION", "string", "Action to check.", 1),
                RequiredArgument("Resource", "RESOURCE", "string", "Resource to check.", 2),
                new CliPositionalArgument
                {
                    PropertyName = "SubResource",
                    PlaceholderName = "SUB-RESOURCE",
                    CSharpType = "string?",
                    IsRequired = false,
                    PositionIndex = 3,
                    Description = "Optional sub-resource to check.",
                },
            ];
        }

        if (positionalArguments.Count == 0)
        {
            var missingArguments = commandParts switch
            {
                ["appset", "create"] => RequiredArgument(
                    "Files", "FILE", "IEnumerable<string>", "One or more ApplicationSet filenames or URLs."),
                ["appset", "generate"] => RequiredArgument(
                    "File", "FILE", "string", "ApplicationSet filename or URL."),
                ["appset", "delete"] => RequiredArgument(
                    "ApplicationSetNames", "APPSETNAME", "IEnumerable<string>", "One or more ApplicationSet names."),
                _ => null,
            };

            if (missingArguments is not null)
            {
                return [missingArguments];
            }
        }

        return positionalArguments
            .Select(argument => argument with
            {
                PropertyName = NormalizePositionalArgumentName(argument.PropertyName),
            })
            .ToList();
    }

    protected override bool IsBooleanValueOption(
        string[] commandParts,
        string switchName,
        string description) =>
        switchName == "--prompts-enabled"
        || base.IsBooleanValueOption(commandParts, switchName, description);

    protected override string NormalizeOptionDescription(string description) =>
        UnixHomeDirectoryPattern().Replace(
            WindowsHomeDirectoryPattern().Replace(description, "<home>"),
            "<home>");

    private static CliPositionalArgument RequiredArgument(
        string propertyName,
        string placeholderName,
        string csharpType,
        string description,
        int positionIndex = 0) => new()
        {
            PropertyName = propertyName,
            PlaceholderName = placeholderName,
            CSharpType = csharpType,
            IsRequired = true,
            PositionIndex = positionIndex,
            Description = description,
        };

    private static string NormalizePositionalArgumentName(string propertyName) => propertyName switch
    {
        "Appname" => "ApplicationName",
        "Appsetname" => "ApplicationSetName",
        "Keyid" => "KeyId",
        "Policyfile" => "PolicyFile",
        "Repourl" => "RepositoryUrl",
        "Reposerver" => "RepositoryServer",
        "Credsurl" => "CredentialsUrl",
        "Servername" => "ServerName",
        _ => propertyName,
    };

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version"
    };

    [GeneratedRegex(@"(?<![\w<\[])(?:<(?<name>[A-Z][A-Z0-9_-]*)(?<multiple>\.\.\.)?>|\[(?<name>[A-Z][A-Z0-9_-]*)(?<multiple>\.\.\.)?\]|(?<name>[A-Z][A-Z0-9_-]*)(?<multiple>\.\.\.)?)(?![\w>\]])")]
    private static partial Regex ArgoCdPositionalArgumentPattern();

    [GeneratedRegex(@"(?i)[A-Z]:[\\/]+Users[\\/]+[^\\/\s\""')]+(?=[\\/]\.config[\\/]argocd[\\/]config)")]
    private static partial Regex WindowsHomeDirectoryPattern();

    [GeneratedRegex(@"/(?:home|Users)/[^/\s\""')]+(?=/\.config/argocd/config)")]
    private static partial Regex UnixHomeDirectoryPattern();
}
