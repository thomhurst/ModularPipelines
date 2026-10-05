using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
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
    private const string GlobalOptionsHeading = "The following options can be passed to any command";

    // Audited persistent settings in Minikube 1.39.0. Additions are allowed; removals
    // require a new upstream audit instead of silently shrinking the generated API.
    private static readonly string[] RequiredGlobalSwitches =
    [
        "--add_dir_header", "--alsologtostderr", "--alsologtostderrthreshold", "--bootstrapper",
        "--legacy_stderr_threshold_behavior", "--log_backtrace_at", "--log_dir", "--log_file",
        "--log_file_max_size", "--logtostderr", "--one_output", "--profile", "--rootless",
        "--skip-audit", "--skip_headers", "--skip_log_headers", "--stderrthreshold", "--user", "--v", "--vmodule",
    ];

    public MinikubeCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<MinikubeCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "minikube";

    public override string NamespacePrefix => "Minikube";

    public override string TargetNamespace => "ModularPipelines.Minikube";

    public override string OutputDirectory => "src/ModularPipelines.Minikube";

    protected override string VersionArguments => "version --short";

    /// <summary>
    /// Minikube help can initialize multiple provider clients. Bound concurrent
    /// processes to keep discovery stable on shared CI runners.
    /// </summary>
    protected override int MaxParallelism => 4;

    protected override string GetHelpArguments(string[] commandPath) =>
        commandPath is [_, "options"] ? "options" : base.GetHelpArguments(commandPath);

    protected override bool ShouldAcceptHelpResult(IReadOnlyList<string> commandPath, CliCommandResult result) =>
        commandPath is [_, "options"]
            ? !result.Unavailable && result.ExitCode == 0
            : base.ShouldAcceptHelpResult(commandPath, result);

    protected override async Task<string?> GetHelpTextAsync(string[] commandPath, CancellationToken cancellationToken)
    {
        var help = await base.GetHelpTextAsync(commandPath, cancellationToken).ConfigureAwait(false);
        if (commandPath.Length != 1 || help is null
            || !ExtractSubcommands(help).Contains("options", StringComparer.Ordinal))
        {
            return help;
        }

        // Minikube's kubectl-style templates put inherited flags in a separate help command.
        var globalHelp = await base.GetHelpTextAsync([ToolName, "options"], cancellationToken).ConfigureAwait(false);
        if (globalHelp is null || !globalHelp.Contains(GlobalOptionsHeading + ":", StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Minikube global option help is unavailable.");
        }

        var missing = RequiredGlobalSwitches.Except(
            ParseGlobalOptions(globalHelp).Select(option => option.SwitchName), StringComparer.Ordinal).ToArray();
        if (missing.Length > 0)
        {
            throw new InvalidOperationException($"Minikube global option help is incomplete; missing: {string.Join(", ", missing)}.");
        }

        return help + "\n" + globalHelp;
    }

    protected override IReadOnlyList<CliOptionDefinition> ParseGlobalOptions(string helpText) =>
        [.. ParseNamedOptionSection(helpText, GlobalOptionsHeading, [])
            .Where(option => option.SwitchName != "--help")
            .Select(NormalizePersistentOption)];

    protected override IReadOnlyList<CliOptionDefinition> ApplyOptionFixes(
        string[] commandParts, IReadOnlyList<CliOptionDefinition> options)
    {
        var globals = EffectiveGlobalOptions;
        var globalSwitches = globals.Select(option => option.SwitchName).ToHashSet(StringComparer.Ordinal);
        CliGlobalOptionMerger.Merge(globals, options
            .Where(option => globalSwitches.Contains(option.SwitchName))
            .Select(NormalizePersistentOption));
        return [.. options.Where(option => !globalSwitches.Contains(option.SwitchName))];
    }

    private static CliOptionDefinition NormalizePersistentOption(CliOptionDefinition option) =>
        option.SwitchName switch
        {
            // klog severity values accept either integer levels or names such as WARNING.
            "--stderrthreshold" or "--alsologtostderrthreshold" => option with
            {
                CSharpType = "string?",
                IsNumeric = false,
            },
            // klog registers this value with flag.Uint64Var, despite its small default.
            "--log_file_max_size" => option with { CSharpType = "ulong?" },
            _ when option.CSharpType == "bool?" => option with
            {
                IsFlag = false,
                ValueSeparator = "=",
                ValueArity = CliOptionValueArity.Optional,
            },
            _ => option,
        };

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version", "update-check"
    };
}
