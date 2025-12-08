using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliSubCommand("expose")]
[ExcludeFromCodeCoverage]
public record KubernetesExposeOptions : KubernetesOptions
{
    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--cluster-ip")]
    public virtual string? ClusterIp { get; set; }

    [CliOption("--container-port")]
    public virtual string? ContainerPort { get; set; }

    [CliOption("--dry-run")]
    public virtual string? DryRun { get; set; }

    [CliOption("--external-ip")]
    public virtual string? ExternalIp { get; set; }

    [CliOption("--field-manager")]
    public virtual string? FieldManager { get; set; }

    [CliOption("--filename")]
    public virtual string[]? Filename { get; set; }

    [CliOption("--generator")]
    public virtual string? Generator { get; set; }

    [CliOption("--kustomize")]
    public virtual string? Kustomize { get; set; }

    [CliOption("--labels")]
    public virtual string? Labels { get; set; }

    [CliOption("--load-balancer-ip")]
    public virtual string? LoadBalancerIp { get; set; }

    [CliOption("--name")]
    public virtual string? Name { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliOption("--overrides")]
    public virtual string? Overrides { get; set; }

    [CliOption("--port")]
    public virtual string? Port { get; set; }

    [CliOption("--protocol")]
    public virtual string? Protocol { get; set; }

    [CliFlag("--record")]
    public virtual bool? Record { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliFlag("--save-config")]
    public virtual bool? SaveConfig { get; set; }

    [CliOption("--selector")]
    public virtual string? Selector { get; set; }

    [CliOption("--session-affinity")]
    public virtual string? SessionAffinity { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--target-port")]
    public virtual string? TargetPort { get; set; }

    [CliOption("--template")]
    public virtual string? Template { get; set; }

    [CliOption("--type")]
    public virtual string? Type { get; set; }
}