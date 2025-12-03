using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerSwarmJoinOptions : DockerOptions
{
    public DockerSwarmJoinOptions(
        string host
    )
    {
        CommandParts = ["swarm", "join"];

        JoinHost = host;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? JoinHost { get; set; }

    [CliOption("--advertise-addr")]
    public virtual string? AdvertiseAddr { get; set; }

    [CliOption("--availability")]
    public virtual string? Availability { get; set; }

    [CliOption("--data-path-addr")]
    public virtual string? DataPathAddr { get; set; }

    [CliOption("--listen-addr")]
    public virtual string? ListenAddr { get; set; }

    [CliOption("--token")]
    public virtual string? Token { get; set; }
}
