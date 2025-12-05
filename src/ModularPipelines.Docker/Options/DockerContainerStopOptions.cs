using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerStopOptions : DockerOptions
{
    public DockerContainerStopOptions(
        IEnumerable<string> container
    )
    {
        CommandParts = ["container", "stop"];

        Container = container;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Container { get; set; }

    [CliOption("--signal")]
    public virtual string? Signal { get; set; }

    [CliOption("--time")]
    public virtual string? Time { get; set; }
}
