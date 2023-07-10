using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("delete")]
public record KubernetesDeleteOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("all-namespaces")]
    public bool? AllNamespaces { get; set; }

    [CommandLongSwitch("cascade", SwitchValueSeparator = " ")]
    public string? Cascade { get; set; }

    [CommandLongSwitch("dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandLongSwitch("field-selector", SwitchValueSeparator = " ")]
    public string? FieldSelector { get; set; }

    [CommandLongSwitch("filename", SwitchValueSeparator = " ")]
    public string[]? Filename { get; set; }

    [BooleanCommandSwitch("force")]
    public bool? Force { get; set; }

    [CommandLongSwitch("grace-period", SwitchValueSeparator = " ")]
    public int? GracePeriod { get; set; }

    [BooleanCommandSwitch("ignore-not-found")]
    public bool? IgnoreNotFound { get; set; }

    [CommandLongSwitch("kustomize", SwitchValueSeparator = " ")]
    public string? Kustomize { get; set; }

    [BooleanCommandSwitch("now")]
    public bool? Now { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandLongSwitch("raw", SwitchValueSeparator = " ")]
    public string? Raw { get; set; }

    [BooleanCommandSwitch("recursive")]
    public bool? Recursive { get; set; }

    [CommandLongSwitch("selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [CommandLongSwitch("timeout", SwitchValueSeparator = " ")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("wait")]
    public bool? Wait { get; set; }

}
