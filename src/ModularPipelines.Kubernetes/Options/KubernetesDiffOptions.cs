using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("diff")]
[ExcludeFromCodeCoverage]
public record KubernetesDiffOptions : KubernetesOptions
{
    [CommandEqualsSeparatorSwitch("--field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [CommandEqualsSeparatorSwitch("--filename", SwitchValueSeparator = " ")]
    public string[]? Filename { get; set; }

    [BooleanCommandSwitch("--force-conflicts")]
    public bool? ForceConflicts { get; set; }

    [CommandEqualsSeparatorSwitch("--kustomize", SwitchValueSeparator = " ")]
    public string? Kustomize { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [CommandEqualsSeparatorSwitch("--selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [BooleanCommandSwitch("--server-side")]
    public bool? ServerSide { get; set; }
}
