using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Minikube - local Kubernetes clusters.
/// Minikube uses Cobra for its CLI (Go-based).
///
/// minikube help format (minikube --help):
/// minikube provisions and manages local Kubernetes clusters optimized for development workflows.
///
/// Usage:
///   minikube [command]
///
/// Available Commands:
///   addons         Enable or disable a minikube addon
///   completion     Generate command completion for a shell
///   config         Modify persistent configuration values
///   cp             Copy the specified file into minikube
///   dashboard      Access the Kubernetes dashboard running within the minikube cluster
///   delete         Deletes a local Kubernetes cluster
///   docker-env     Provides instructions to point your terminal's docker-cli to the Docker Engine inside minikube
///   help           Help about any command
///   image          Load images into minikube
///   ip             Retrieves the IP address of the specified node
///   kubectl        Run a kubectl binary matching the cluster version
///   logs           Gets the logs of the running instance
///   mount          Mounts the specified directory into minikube
///   node           Add, remove, or list additional nodes
///   pause          Pause Kubernetes
///   podman-env     Provides instructions to point your terminal's docker-cli to the Docker Engine inside minikube
///   profile        Get or list the current profiles (clusters)
///   service        Returns a URL to connect to a service
///   ssh            Log into minikube environment (for debugging)
///   ssh-key        Retrieve the ssh identity key path of the specified node
///   start          Starts a local Kubernetes cluster
///   status         Gets the status of a local Kubernetes cluster
///   stop           Stops a running local Kubernetes cluster
///   tunnel         Connect to LoadBalancer services
///   unpause        Unpause Kubernetes
///   update-check   Print current and latest version number
///   update-context Update kubeconfig in case of an IP or port change
///   version        Print the version of minikube
/// </summary>
public partial class MinikubeCliScraper : CobraCliScraper
{
    public MinikubeCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<MinikubeCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "minikube";

    public override string NamespacePrefix => "Minikube";

    public override string TargetNamespace => "ModularPipelines.Minikube";

    public override string OutputDirectory => "src/ModularPipelines.Minikube";

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version", "update-check"
    };
}
