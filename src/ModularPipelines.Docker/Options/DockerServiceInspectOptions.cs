using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

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
    public virtual string? Format { get; set; }

    [CommandSwitch("--pretty")]
    public virtual string? Pretty { get; set; }
}
