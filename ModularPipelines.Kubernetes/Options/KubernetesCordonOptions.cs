using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("cordon")]
public record KubernetesCordonOptions([property: PositionalArgument] string Node) : KubernetesOptions
{
    [CommandLongSwitch("dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandLongSwitch("selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

}
