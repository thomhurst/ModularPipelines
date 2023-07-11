using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("cp", "<file-spec-src>", "<file-spec-dest>")]
public record KubernetesCpOptions : KubernetesOptions
{
    [CommandEqualsSeparatorSwitch("--container", SwitchValueSeparator = " ")]
    public string? Container { get; set; }

    [BooleanCommandSwitch("--no-preserve")]
    public bool? NoPreserve { get; set; }

}
