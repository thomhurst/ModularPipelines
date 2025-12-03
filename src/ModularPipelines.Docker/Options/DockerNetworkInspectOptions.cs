using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerNetworkInspectOptions : DockerOptions
{
    public DockerNetworkInspectOptions(
        IEnumerable<string> network
    )
    {
        CommandParts = ["network", "inspect"];

        Network = network;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Network { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliOption("--verbose")]
    public virtual string? Verbose { get; set; }
}
