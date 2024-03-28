using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesDiffOptions : KubernetesOptions
{
    public KubernetesDiffOptions(
        IEnumerable<string> filename
)
    {
        CommandParts = ["diff"];
        Filename = filename;
    }

    [CommandSwitch("--field-manager")]
    public string? FieldManager { get; set; }

    [CommandSwitch("--filename")]
    public IEnumerable<string>? Filename { get; set; }

    [BooleanCommandSwitch("--force-conflicts")]
    public bool? ForceConflicts { get; set; }

    [CommandSwitch("--kustomize")]
    public string? Kustomize { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [CommandSwitch("--selector")]
    public string? Selector { get; set; }

    [BooleanCommandSwitch("--server-side")]
    public bool? ServerSide { get; set; }
}
