using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("describe")]
public record KubernetesDescribeOptions : KubernetesOptions
{
    [BooleanCommandSwitch("all-namespaces")]
    public bool? AllNamespaces { get; set; }

    [CommandLongSwitch("chunk-size", SwitchValueSeparator = " ")]
    public int? ChunkSize { get; set; }

    [CommandLongSwitch("filename", SwitchValueSeparator = " ")]
    public string[]? Filename { get; set; }

    [CommandLongSwitch("kustomize", SwitchValueSeparator = " ")]
    public string? Kustomize { get; set; }

    [BooleanCommandSwitch("recursive")]
    public bool? Recursive { get; set; }

    [CommandLongSwitch("selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [BooleanCommandSwitch("show-events")]
    public bool? ShowEvents { get; set; }

}
