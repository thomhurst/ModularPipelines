using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerNetworkConnectOptions : DockerOptions
{
    public DockerNetworkConnectOptions(
        string network,
        string container
    )
    {
        CommandParts = ["network", "connect"];

        Network = network;

        Container = container;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Network { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Container { get; set; }

    [CliOption("--alias")]
    public virtual string? Alias { get; set; }

    [CliOption("--driver-opt")]
    public virtual string? DriverOpt { get; set; }

    [CliOption("--ip")]
    public virtual string? Ip { get; set; }

    [CliOption("--ip6")]
    public virtual string? Ip6 { get; set; }

    [CliOption("--link")]
    public virtual string? Link { get; set; }

    [CliOption("--link-local-ip")]
    public virtual string? LinkLocalIp { get; set; }
}
