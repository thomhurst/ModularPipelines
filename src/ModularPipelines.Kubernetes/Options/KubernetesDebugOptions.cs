using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("debug")]
[ExcludeFromCodeCoverage]
public record KubernetesDebugOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("--arguments-only")]
    public virtual bool? ArgumentsOnly { get; set; }

    [BooleanCommandSwitch("--attach")]
    public virtual bool? Attach { get; set; }

    [CommandEqualsSeparatorSwitch("--container", SwitchValueSeparator = " ")]
    public string? Container { get; set; }

    [CommandEqualsSeparatorSwitch("--copy-to", SwitchValueSeparator = " ")]
    public string? CopyTo { get; set; }

    [CommandEqualsSeparatorSwitch("--env", SwitchValueSeparator = " ")]
    public string[]? Env { get; set; }

    [CommandEqualsSeparatorSwitch("--image", SwitchValueSeparator = " ")]
    public string? Image { get; set; }

    [CommandEqualsSeparatorSwitch("--image-pull-policy", SwitchValueSeparator = " ")]
    public string? ImagePullPolicy { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--replace")]
    public virtual bool? Replace { get; set; }

    [BooleanCommandSwitch("--same-node")]
    public virtual bool? SameNode { get; set; }

    [CommandEqualsSeparatorSwitch("--set-image", SwitchValueSeparator = " ")]
    public string[]? SetImage { get; set; }

    [BooleanCommandSwitch("--share-processes")]
    public virtual bool? ShareProcesses { get; set; }

    [BooleanCommandSwitch("--stdin")]
    public virtual bool? Stdin { get; set; }

    [CommandEqualsSeparatorSwitch("--target", SwitchValueSeparator = " ")]
    public string? Target { get; set; }

    [BooleanCommandSwitch("--tty")]
    public virtual bool? Tty { get; set; }
}