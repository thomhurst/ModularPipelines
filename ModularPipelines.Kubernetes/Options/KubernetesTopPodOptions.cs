using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("top", "pod")]
public record KubernetesTopPodOptions(string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("all-namespaces")]
    public bool? AllNamespaces { get; set; }

    [BooleanCommandSwitch("containers")]
    public bool? Containers { get; set; }

    [CommandLongSwitch("field-selector", SwitchValueSeparator = " ")]
    public string? FieldSelector { get; set; }

    [BooleanCommandSwitch("no-headers")]
    public bool? NoHeaders { get; set; }

    [CommandLongSwitch("selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [CommandLongSwitch("sort-by", SwitchValueSeparator = " ")]
    public string? SortBy { get; set; }

    [BooleanCommandSwitch("use-protocol-buffers")]
    public bool? UseProtocolBuffers { get; set; }

}
