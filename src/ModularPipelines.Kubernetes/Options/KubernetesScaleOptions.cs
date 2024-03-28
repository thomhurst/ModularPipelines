using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesScaleOptions : KubernetesOptions
{
    public KubernetesScaleOptions(
        IEnumerable<string> filename,
        int replicas
)
    {
        CommandParts = ["scale"];
        Filename = filename;
        Replicas = replicas;
    }

    public KubernetesScaleOptions(
        string typeName,
        int replicas
)
    {
        CommandParts = ["scale", "<TypeName>"];
        TypeName = typeName;
        Replicas = replicas;
    }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandSwitch("--current-replicas")]
    public int? CurrentReplicas { get; set; }

    [CommandSwitch("--dry-run")]
    public string? DryRun { get; set; }

    [CommandSwitch("--filename")]
    public IEnumerable<string>? Filename { get; set; }

    [CommandSwitch("--kustomize")]
    public string? Kustomize { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--record")]
    public bool? Record { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [CommandSwitch("--replicas")]
    public int? Replicas { get; set; }

    [CommandSwitch("--resource-version")]
    public string? ResourceVersion { get; set; }

    [CommandSwitch("--selector")]
    public string? Selector { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [PositionalArgument(PlaceholderName = "<TypeName>")]
    public string? TypeName { get; set; }
}
