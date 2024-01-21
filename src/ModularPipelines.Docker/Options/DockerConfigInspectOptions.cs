using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerConfigInspectOptions : DockerOptions
{
    public DockerConfigInspectOptions(
        IEnumerable<string> config
    )
    {
        CommandParts = ["config", "inspect"];

        InspectConfig = config;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? InspectConfig { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--pretty")]
    public string? Pretty { get; set; }
}
