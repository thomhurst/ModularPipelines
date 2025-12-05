using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerKillOptions : DockerOptions
{
    public DockerContainerKillOptions(
        IEnumerable<string> container
    )
    {
        CommandParts = ["container", "kill"];

        Container = container;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Container { get; set; }

    [CliOption("--signal")]
    public virtual string? Signal { get; set; }
}
