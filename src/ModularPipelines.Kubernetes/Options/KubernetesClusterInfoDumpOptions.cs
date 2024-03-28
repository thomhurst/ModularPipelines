using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesClusterInfoDumpOptions : KubernetesOptions
{
    public KubernetesClusterInfoDumpOptions()
    {
        CommandParts = ["cluster-info", "dump"];
    }

    [BooleanCommandSwitch("--all-namespaces")]
    public bool? AllNamespaces { get; set; }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandSwitch("--namespaces")]
    public IEnumerable<string>? Namespaces { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [CommandSwitch("--output-directory")]
    public string? OutputDirectory { get; set; }

    [CommandSwitch("--pod-running-timeout")]
    public string? PodRunningTimeout { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }
}
