using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContainerExportOptions : DockerOptions
{
    public DockerContainerExportOptions(
        string container
    )
    {
        CommandParts = ["container", "export"];

        Container = container;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Container { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }
}
