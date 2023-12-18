using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("debug")]
[ExcludeFromCodeCoverage]
public record KubernetesDebugOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("--arguments-only")]
    public bool? ArgumentsOnly { get; set; }

    [BooleanCommandSwitch("--attach")]
    public bool? Attach { get; set; }

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
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--replace")]
    public bool? Replace { get; set; }

    [BooleanCommandSwitch("--same-node")]
    public bool? SameNode { get; set; }

    [CommandEqualsSeparatorSwitch("--set-image", SwitchValueSeparator = " ")]
    public string[]? SetImage { get; set; }

    [BooleanCommandSwitch("--share-processes")]
    public bool? ShareProcesses { get; set; }

    [BooleanCommandSwitch("--stdin")]
    public bool? Stdin { get; set; }

    [CommandEqualsSeparatorSwitch("--target", SwitchValueSeparator = " ")]
    public string? Target { get; set; }

    [BooleanCommandSwitch("--tty")]
    public bool? Tty { get; set; }
}