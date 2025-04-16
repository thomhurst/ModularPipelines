using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

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
    public virtual string? Format { get; set; }

    [CommandSwitch("--pretty")]
    public virtual string? Pretty { get; set; }
}
