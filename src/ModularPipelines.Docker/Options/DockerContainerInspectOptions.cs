using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerInspectOptions : DockerOptions
{
    public DockerContainerInspectOptions(
        IEnumerable<string> container
    )
    {
        CommandParts = ["container", "inspect"];

        Container = container;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Container { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliOption("--size")]
    public virtual string? Size { get; set; }
}
