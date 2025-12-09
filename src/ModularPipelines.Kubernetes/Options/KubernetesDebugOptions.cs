using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliSubCommand("debug")]
[ExcludeFromCodeCoverage]
public record KubernetesDebugOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliFlag("--arguments-only")]
    public virtual bool? ArgumentsOnly { get; set; }

    [CliFlag("--attach")]
    public virtual bool? Attach { get; set; }

    [CliOption("--container")]
    public virtual string? Container { get; set; }

    [CliOption("--copy-to")]
    public virtual string? CopyTo { get; set; }

    [CliOption("--env")]
    public virtual string[]? Env { get; set; }

    [CliOption("--image")]
    public virtual string? Image { get; set; }

    [CliOption("--image-pull-policy")]
    public virtual string? ImagePullPolicy { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--replace")]
    public virtual bool? Replace { get; set; }

    [CliFlag("--same-node")]
    public virtual bool? SameNode { get; set; }

    [CliOption("--set-image")]
    public virtual string[]? SetImage { get; set; }

    [CliFlag("--share-processes")]
    public virtual bool? ShareProcesses { get; set; }

    [CliFlag("--stdin")]
    public virtual bool? Stdin { get; set; }

    [CliOption("--target")]
    public virtual string? Target { get; set; }

    [CliFlag("--tty")]
    public virtual bool? Tty { get; set; }
}