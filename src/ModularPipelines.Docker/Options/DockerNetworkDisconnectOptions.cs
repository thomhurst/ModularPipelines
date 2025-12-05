using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerNetworkDisconnectOptions : DockerOptions
{
    public DockerNetworkDisconnectOptions(
        string network,
        string container
    )
    {
        CommandParts = ["network", "disconnect"];

        Network = network;

        Container = container;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Network { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Container { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }
}
