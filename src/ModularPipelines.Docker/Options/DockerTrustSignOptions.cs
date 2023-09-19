using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust sign")]
[ExcludeFromCodeCoverage]
public record DockerTrustSignOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Image) : DockerOptions
{
    [CommandSwitch("--local")]
    public string? Local { get; set; }
}
