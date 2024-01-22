using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerServiceInspectOptions : DockerOptions
{
    public DockerServiceInspectOptions(
        IEnumerable<string> service
    )
    {
        CommandParts = ["service", "inspect"];

        Service = service;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Service { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--pretty")]
    public string? Pretty { get; set; }
}
