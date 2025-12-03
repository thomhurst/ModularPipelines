using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("expose")]
[ExcludeFromCodeCoverage]
public record KubernetesExposeOptions : KubernetesOptions
{
    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--cluster-ip")]
    public string? ClusterIp { get; set; }

    [CliOption("--container-port")]
    public string? ContainerPort { get; set; }

    [CliOption("--dry-run")]
    public string? DryRun { get; set; }

    [CliOption("--external-ip")]
    public string? ExternalIp { get; set; }

    [CliOption("--field-manager")]
    public string? FieldManager { get; set; }

    [CliOption("--filename")]
    public string[]? Filename { get; set; }

    [CliOption("--generator")]
    public string? Generator { get; set; }

    [CliOption("--kustomize")]
    public string? Kustomize { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliOption("--load-balancer-ip")]
    public string? LoadBalancerIp { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }

    [CliOption("--overrides")]
    public string? Overrides { get; set; }

    [CliOption("--port")]
    public string? Port { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliFlag("--record")]
    public virtual bool? Record { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliFlag("--save-config")]
    public virtual bool? SaveConfig { get; set; }

    [CliOption("--selector")]
    public string? Selector { get; set; }

    [CliOption("--session-affinity")]
    public string? SessionAffinity { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--target-port")]
    public string? TargetPort { get; set; }

    [CliOption("--template")]
    public string? Template { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}