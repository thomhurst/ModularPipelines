using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("get")]
public record KubernetesGetOptions(string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("all-namespaces")]
    public bool? AllNamespaces { get; set; }

    [BooleanCommandSwitch("allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandLongSwitch("chunk-size", SwitchValueSeparator = " ")]
    public int? ChunkSize { get; set; }

    [CommandLongSwitch("field-selector", SwitchValueSeparator = " ")]
    public string? FieldSelector { get; set; }

    [CommandLongSwitch("filename", SwitchValueSeparator = " ")]
    public string[]? Filename { get; set; }

    [BooleanCommandSwitch("ignore-not-found")]
    public bool? IgnoreNotFound { get; set; }

    [CommandLongSwitch("kustomize", SwitchValueSeparator = " ")]
    public string? Kustomize { get; set; }

    [CommandLongSwitch("label-columns", SwitchValueSeparator = " ")]
    public string[]? LabelColumns { get; set; }

    [BooleanCommandSwitch("no-headers")]
    public bool? NoHeaders { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("output-watch-events")]
    public bool? OutputWatchEvents { get; set; }

    [CommandLongSwitch("raw", SwitchValueSeparator = " ")]
    public string? Raw { get; set; }

    [BooleanCommandSwitch("recursive")]
    public bool? Recursive { get; set; }

    [CommandLongSwitch("selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [BooleanCommandSwitch("server-print")]
    public bool? ServerPrint { get; set; }

    [BooleanCommandSwitch("show-kind")]
    public bool? ShowKind { get; set; }

    [BooleanCommandSwitch("show-labels")]
    public bool? ShowLabels { get; set; }

    [BooleanCommandSwitch("show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandLongSwitch("sort-by", SwitchValueSeparator = " ")]
    public string? SortBy { get; set; }

    [CommandLongSwitch("template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("use-openapi-print-columns")]
    public bool? UseOpenapiPrintColumns { get; set; }

    [BooleanCommandSwitch("watch")]
    public bool? Watch { get; set; }

    [BooleanCommandSwitch("watch-only")]
    public bool? WatchOnly { get; set; }

}
