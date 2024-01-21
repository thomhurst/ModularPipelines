using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

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

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Container { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }
}
