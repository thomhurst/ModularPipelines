using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("apply", "view-last-applied")]
public record KubernetesApplyViewLastAppliedOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("all")]
    public bool? All { get; set; }

    [CommandLongSwitch("filename", SwitchValueSeparator = " ")]
    public string[]? Filename { get; set; }

    [CommandLongSwitch("kustomize", SwitchValueSeparator = " ")]
    public string? Kustomize { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("recursive")]
    public bool? Recursive { get; set; }

    [CommandLongSwitch("selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

}
