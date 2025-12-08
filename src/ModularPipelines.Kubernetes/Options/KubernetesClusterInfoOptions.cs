using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliSubCommand("cluster-info")]
[ExcludeFromCodeCoverage]
public record KubernetesClusterInfoOptions : KubernetesOptions
{
    [CliFlag("--all-namespaces")]
    public virtual bool? AllNamespaces { get; set; }

    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--namespaces")]
    public virtual string[]? Namespaces { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliOption("--output-directory")]
    public virtual string? OutputDirectory { get; set; }

    [CliOption("--pod-running-timeout")]
    public virtual string? PodRunningTimeout { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public virtual string? Template { get; set; }
}