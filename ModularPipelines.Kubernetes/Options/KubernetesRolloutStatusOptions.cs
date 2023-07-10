using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("rollout", "status")]
public record KubernetesRolloutStatusOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [CommandLongSwitch("filename", SwitchValueSeparator = " ")]
    public string[]? Filename { get; set; }

    [CommandLongSwitch("kustomize", SwitchValueSeparator = " ")]
    public string? Kustomize { get; set; }

    [BooleanCommandSwitch("recursive")]
    public bool? Recursive { get; set; }

    [CommandLongSwitch("revision", SwitchValueSeparator = " ")]
    public int? Revision { get; set; }

    [CommandLongSwitch("timeout", SwitchValueSeparator = " ")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("watch")]
    public bool? Watch { get; set; }

}
