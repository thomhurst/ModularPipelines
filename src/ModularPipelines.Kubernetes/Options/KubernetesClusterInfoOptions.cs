using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("cluster-info")]
[ExcludeFromCodeCoverage]
public record KubernetesClusterInfoOptions : KubernetesOptions
{
    [CliFlag("--all-namespaces")]
    public virtual bool? AllNamespaces { get; set; }

    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--namespaces")]
    public string[]? Namespaces { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }

    [CliOption("--output-directory")]
    public string? OutputDirectory { get; set; }

    [CliOption("--pod-running-timeout")]
    public string? PodRunningTimeout { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public string? Template { get; set; }
}