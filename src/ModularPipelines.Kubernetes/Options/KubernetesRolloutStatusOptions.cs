using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("rollout", "status")]
public record KubernetesRolloutStatusOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [CommandEqualsSeparatorSwitch("--filename", SwitchValueSeparator = " ")]
    public string[]? Filename { get; set; }

    [CommandEqualsSeparatorSwitch("--kustomize", SwitchValueSeparator = " ")]
    public string? Kustomize { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [CommandEqualsSeparatorSwitch("--revision", SwitchValueSeparator = " ")]
    public int? Revision { get; set; }

    [CommandEqualsSeparatorSwitch("--timeout", SwitchValueSeparator = " ")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("--watch")]
    public bool? Watch { get; set; }
}
