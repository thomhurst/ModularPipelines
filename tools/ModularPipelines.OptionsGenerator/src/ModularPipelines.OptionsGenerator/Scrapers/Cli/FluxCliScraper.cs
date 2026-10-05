using Microsoft.Extensions.Logging;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

/// <summary>
/// CLI-first scraper for Flux CLI - GitOps toolkit for Kubernetes.
/// Flux uses Cobra for its CLI.
///
/// flux help format (flux --help):
/// Command line utility for assembling Kubernetes CD pipelines
///
/// Usage:
///   flux [command]
///
/// Available Commands:
///   bootstrap     Bootstrap toolkit components
///   build         Build a resource
///   check         Check requirements and installation
///   create        Create or update sources and resources
///   ...
/// </summary>
public partial class FluxCliScraper : CobraCliScraper
{
    public FluxCliScraper(ICliCommandExecutor executor, IHelpTextCache helpCache, ILogger<FluxCliScraper> logger)
        : base(executor, helpCache, logger)
    {
    }

    public override string ToolName => "flux";

    public override string NamespacePrefix => "Flux";

    public override string TargetNamespace => "ModularPipelines.Flux";

    public override string OutputDirectory => "src/ModularPipelines.Flux";

    // Preserve Flux's existing command-first rendering and Cobra's local --token replacements.
    protected override bool GlobalOptionsBeforeSubcommands => false;

    // Verified root PersistentFlags registrations: see Fixtures/Flux/2.9.6/README.md.
    protected override IReadOnlyList<CliOptionDefinition> ParseGlobalOptions(string helpText) =>
        [.. ParseNamedOptionSection(helpText, "Flags", []).Where(option => option.SwitchName is
            "--as" or "--as-group" or "--as-uid" or "--as-user-extra" or "--cache-dir"
            or "--certificate-authority" or "--client-certificate" or "--client-key" or "--cluster"
            or "--context" or "--disable-compression" or "--insecure-skip-tls-verify"
            or "--kube-api-burst" or "--kube-api-qps" or "--kubeconfig" or "--namespace"
            or "--ns-follows-kube-context" or "--server" or "--timeout" or "--tls-server-name"
            or "--token" or "--user" or "--verbose")];

    protected override IReadOnlyList<CliOptionDefinition> ApplyOptionFixes(
        string[] commandParts,
        IReadOnlyList<CliOptionDefinition> options)
    {
        var globals = EffectiveGlobalOptions;
        var inheritedSwitches = globals.Select(option => option.SwitchName).ToHashSet(StringComparer.Ordinal);
        if (commandParts is ["create", "secret", "receiver"] or ["trigger", "receiver"])
        {
            // These commands register their own webhook token instead of inheriting the API bearer token.
            inheritedSwitches.Remove("--token");
        }

        CliGlobalOptionMerger.Merge(globals.Where(option => inheritedSwitches.Contains(option.SwitchName)),
            options.Where(option => inheritedSwitches.Contains(option.SwitchName)));
        return [.. options.Where(option => !inheritedSwitches.Contains(option.SwitchName))];
    }

    protected override string NormalizeOptionDescription(string description) =>
        description.StartsWith("Default cache directory (default ", StringComparison.Ordinal)
            ? "Default cache directory."
            : description;

    /// <summary>
    /// Skip utility commands.
    /// </summary>
    protected override IReadOnlySet<string> AdditionalSkipSubcommands => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "--help", "-h", "--version", "help", "completion", "version"
    };
}
