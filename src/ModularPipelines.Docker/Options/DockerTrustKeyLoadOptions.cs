using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust key load")]
[ExcludeFromCodeCoverage]
public record DockerTrustKeyLoadOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Keyfile) : DockerOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }
}