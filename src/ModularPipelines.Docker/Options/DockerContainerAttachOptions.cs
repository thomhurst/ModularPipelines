using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerAttachOptions : DockerOptions
{
    public DockerContainerAttachOptions(
        string container
    )
    {
        CommandParts = ["container", "attach"];

        Container = container;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Container { get; set; }

    [CliOption("--detach-keys")]
    public virtual string? DetachKeys { get; set; }

    [CliOption("--no-stdin")]
    public virtual string? NoStdin { get; set; }

    [CliFlag("--sig-proxy")]
    public virtual bool? SigProxy { get; set; }
}
