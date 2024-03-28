using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesDebugOptions : KubernetesOptions
{
    [BooleanCommandSwitch("--arguments-only")]
    public bool? ArgumentsOnly { get; set; }

    [BooleanCommandSwitch("--attach")]
    public bool? Attach { get; set; }

    [CommandSwitch("--container")]
    public string? Container { get; set; }

    [CommandSwitch("--copy-to")]
    public string? CopyTo { get; set; }

    [CommandSwitch("--env")]
    public IEnumerable<string>? Env { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--image-pull-policy")]
    public string? ImagePullPolicy { get; set; }

    [PositionalArgument(PlaceholderName = "<Pod>")]
    public string? Pod { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--replace")]
    public bool? Replace { get; set; }

    [BooleanCommandSwitch("--same-node")]
    public bool? SameNode { get; set; }

    [CommandSwitch("--set-image")]
    public IEnumerable<string>? SetImage { get; set; }

    [BooleanCommandSwitch("--share-processes")]
    public bool? ShareProcesses { get; set; }

    [BooleanCommandSwitch("--stdin")]
    public bool? Stdin { get; set; }

    [CommandSwitch("--target")]
    public string? Target { get; set; }

    [BooleanCommandSwitch("--tty")]
    public bool? Tty { get; set; }

    [PositionalArgument(PlaceholderName = "<TypeName>")]
    public string? TypeName { get; set; }
}
