using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerRmOptions : DockerOptions
{
    public DockerContainerRmOptions(
        IEnumerable<string> container
    )
    {
        CommandParts = ["container", "rm"];

        Container = container;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public IEnumerable<string>? Container { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliOption("--link")]
    public virtual string? Link { get; set; }

    [CliOption("--volumes")]
    public virtual string? Volumes { get; set; }
}
