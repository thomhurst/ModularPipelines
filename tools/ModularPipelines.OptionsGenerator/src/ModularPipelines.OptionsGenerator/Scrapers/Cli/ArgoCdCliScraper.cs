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
    /// The appset create usage line omits its required file arguments even though the
    /// command accepts one or more filenames or URLs.
    /// </summary>
    protected override IReadOnlyList<CliPositionalArgument> ApplyPositionalArgumentFixes(
        string[] commandParts,
        IReadOnlyList<CliPositionalArgument> positionalArguments)
    {
        if (commandParts is ["appset", "create"] && positionalArguments.Count == 0)
        {
            return
            [
                new CliPositionalArgument
                {
                    PropertyName = "Files",
                    PlaceholderName = "FILE",
                    CSharpType = "IEnumerable<string>",
                    IsRequired = true,
                    PositionIndex = 0,
                    Description = "One or more ApplicationSet filenames or URLs.",
                },
            ];
        }

        return positionalArguments
            .Select(argument => argument with
            {
                PropertyName = NormalizePositionalArgumentName(argument.PropertyName),
            })
            .ToList();
    }

    private static string NormalizePositionalArgumentName(string propertyName) => propertyName switch
    {
        "Appname" => "ApplicationName",
        "Appsetname" => "ApplicationSetName",
        "Keyid" => "KeyId",
        "Policyfile" => "PolicyFile",
        "Reposerver" => "RepositoryServer",
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
}
