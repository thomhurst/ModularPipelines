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

    protected override string VersionArguments => "version --client";

    /// <summary>
    /// Argo CD calls the ApplicationSet command "appset". Expanding the compound name
    /// prevents it from colliding with the separate "app set" command in generated code.
    /// </summary>
    protected override string NormalizeCommandIdentifier(string commandPart) =>
        commandPart.Equals("appset", StringComparison.OrdinalIgnoreCase)
            ? "ApplicationSet"
            : base.NormalizeCommandIdentifier(commandPart);

    /// <summary>
    /// The appset create usage line omits its required file arguments even though the
    /// command accepts one or more filenames or URLs.
    /// </summary>
    protected override IReadOnlyList<CliPositionalArgument> ApplyPositionalArgumentFixes(
        string[] commandParts,
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        positionalArguments = positionalArguments
            .Where(argument => !UsageSynopsisParser.IsCommandGroupPlaceholder(argument))
            .ToArray();
        var commandSpecificArguments = GetCommandSpecificArguments(commandParts, positionalArguments);
        if (commandSpecificArguments is not null)
        {
            return commandSpecificArguments;
        }

        if (positionalArguments.Count == 0)
        {
            var missingArgument = commandParts switch
            {
                ["account", "delete-token"] => RequiredArgument(
                    "Id", "string", "Token identifier."),
                ["appset", "create"] => RequiredArgument(
                    "Files", "IEnumerable<string>", "One or more ApplicationSet filenames or URLs."),
                ["appset", "generate"] => RequiredArgument(
                    "Files", "IEnumerable<string>", "One or more ApplicationSet filenames or URLs."),
                ["appset", "delete"] => RequiredArgument(
                    "ApplicationSetNames", "IEnumerable<string>", "One or more ApplicationSet names."),
                _ => null,
            };

            if (missingArgument is not null)
            {
                return [missingArgument];
            }
        }

        return positionalArguments
            .Select(argument => argument with
            {
                PropertyName = NormalizePositionalArgumentName(argument.PropertyName),
            })
            .ToList();
    }

    protected override UsageSynopsisParseResult NormalizeUsageSynopsis(
        CliCommandDefinition command,
        UsageSynopsisParseResult usage) =>
        UsageSynopsisParser.RemoveCommandGroupPlaceholders(usage);

    private static IReadOnlyList<CliPositionalArgument>? GetCommandSpecificArguments(
        string[] commandParts,
        IReadOnlyList<CliPositionalArgument> positionalArguments) => commandParts switch
        {
            ["context"] => positionalArguments
                .Select(argument => argument with { PropertyName = "ContextName" })
                .ToList(),
            ["admin", "cluster", "kubeconfig"] => positionalArguments
                .Select(argument => argument with
                {
                    CSharpType = "string?",
                    IsRequired = false,
                })
                .ToList(),
            ["app", "delete"] or ["app", "sync"] or ["app", "wait"] => [ApplicationNamesArgument()],
            ["app", "unset"] =>
            [
                RequiredArgument(
                    "ApplicationName",
                    "string",
                    "Application name."),
            ],
            ["repo", "rm"] =>
            [
                RequiredArgument(
                    "Repositories",
                    "IEnumerable<string>",
                    "One or more repository URLs."),
            ],
            ["cluster", "set"] => positionalArguments
                .Select(argument => argument with
                {
                    PropertyName = argument.PropertyName == "Name"
                        ? "ClusterName"
                        : NormalizePositionalArgumentName(argument.PropertyName),
                })
                .ToList(),
            ["cert", "add-tls"] => positionalArguments
                .Select(argument => argument with
                {
                    PropertyName = argument.PropertyName == "Servername"
                        ? "RepositoryServerName"
                        : NormalizePositionalArgumentName(argument.PropertyName),
                })
                .ToList(),
            ["cluster", "get"] or ["cluster", "rm"] or ["cluster", "rotate-auth"] =>
            [
                RequiredArgument(
                    "ServerOrName",
                    "string",
                    "Cluster server address or configured name."),
            ],
            ["proj", "remove-destination"]
                or ["proj", "add-destination-service-account"]
                or ["proj", "remove-destination-service-account"] => positionalArguments
                    .Select(argument => argument with
                    {
                        PropertyName = argument.PropertyName == "Server"
                            ? "DestinationServer"
                            : NormalizePositionalArgumentName(argument.PropertyName),
                    })
                    .ToList(),
            ["proj", "add-destination"] => ProjectDestinationArguments(),
            ["admin", "settings", "rbac", "can"] => RbacCanArguments(),
            _ => null,
        };

    private static CliPositionalArgument ApplicationNamesArgument() => new()
    {
        PropertyName = "ApplicationNames",
        CSharpType = "IEnumerable<string>?",
        IsRequired = false,
        PositionIndex = 0,
        Description = "Optional application names to target.",
    };

    private static IReadOnlyList<CliPositionalArgument> ProjectDestinationArguments() =>
    [
        RequiredArgument("Project", "string", "Project name.", 0),
        RequiredArgument(
            "ServerOrName",
            "string",
            "Destination server address or configured name.",
            1),
        RequiredArgument("Namespace", "string", "Destination namespace.", 2),
    ];

    private static IReadOnlyList<CliPositionalArgument> RbacCanArguments() =>
    [
        RequiredArgument("RoleSubject", "string", "Role or subject to check.", 0),
        RequiredArgument("Action", "string", "Action to check.", 1),
        RequiredArgument("Resource", "string", "Resource to check.", 2),
        new CliPositionalArgument
        {
            PropertyName = "SubResource",
            CSharpType = "string?",
            IsRequired = false,
            PositionIndex = 3,
            Description = "Optional sub-resource to check.",
        },
    ];

    protected override bool IsBooleanValueOption(
        string[] commandParts,
        string switchName,
        string description) =>
        switchName == "--prompts-enabled"
        || base.IsBooleanValueOption(commandParts, switchName, description);

    protected override string NormalizeOptionTypeHint(
        string[] commandParts,
        string switchName,
        string typeHint,
        string description) =>
        switchName == "--sync-option"
            ? "stringArray"
            : base.NormalizeOptionTypeHint(commandParts, switchName, typeHint, description);

    protected override string NormalizeOptionDescription(string description) =>
        UnixHomeDirectoryPattern().Replace(
            WindowsHomeDirectoryPattern().Replace(description, "<home>"),
            "<home>");

    private static CliPositionalArgument RequiredArgument(
        string propertyName,
        string csharpType,
        string description,
        int positionIndex = 0) => new()
        {
            PropertyName = propertyName,
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

    [GeneratedRegex(@"(?i)[A-Z]:[\\/]+Users[\\/]+[^\\/\s\""')]+(?=[\\/]\.config[\\/]argocd[\\/]config)")]
    private static partial Regex WindowsHomeDirectoryPattern();

    [GeneratedRegex(@"/(?:home|Users)/[^/\s\""')]+(?=/\.config/argocd/config)")]
    private static partial Regex UnixHomeDirectoryPattern();
}
