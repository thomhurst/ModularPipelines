using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerRestartOptions : DockerOptions
{
    public DockerContainerRestartOptions(
        IEnumerable<string> container
    )
    {
        CommandParts = ["container", "restart"];

        Container = container;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Container { get; set; }

    [CliOption("--signal")]
    public virtual string? Signal { get; set; }

    [CliOption("--time")]
    public virtual string? Time { get; set; }
}
