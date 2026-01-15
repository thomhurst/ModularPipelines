using Microsoft.Extensions.Logging;
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
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version"
    };
}
