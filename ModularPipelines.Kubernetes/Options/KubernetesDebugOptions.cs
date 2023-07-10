using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("debug")]
public record KubernetesDebugOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("arguments-only")]
    public bool? ArgumentsOnly { get; set; }

    [BooleanCommandSwitch("attach")]
    public bool? Attach { get; set; }

    [CommandLongSwitch("container", SwitchValueSeparator = " ")]
    public string? Container { get; set; }

    [CommandLongSwitch("copy-to", SwitchValueSeparator = " ")]
    public string? CopyTo { get; set; }

    [CommandLongSwitch("env", SwitchValueSeparator = " ")]
    public string[]? Env { get; set; }

    [CommandLongSwitch("image", SwitchValueSeparator = " ")]
    public string? Image { get; set; }

    [CommandLongSwitch("image-pull-policy", SwitchValueSeparator = " ")]
    public string? ImagePullPolicy { get; set; }

    [BooleanCommandSwitch("quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("replace")]
    public bool? Replace { get; set; }

    [BooleanCommandSwitch("same-node")]
    public bool? SameNode { get; set; }

    [CommandLongSwitch("set-image", SwitchValueSeparator = " ")]
    public string[]? SetImage { get; set; }

    [BooleanCommandSwitch("share-processes")]
    public bool? ShareProcesses { get; set; }

    [BooleanCommandSwitch("stdin")]
    public bool? Stdin { get; set; }

    [CommandLongSwitch("target", SwitchValueSeparator = " ")]
    public string? Target { get; set; }

    [BooleanCommandSwitch("tty")]
    public bool? Tty { get; set; }

}
