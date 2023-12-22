using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust key generate")]
[ExcludeFromCodeCoverage]
public record DockerTrustKeyGenerateOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Name) : DockerOptions
{
    [CommandSwitch("--dir")]
    public string? Dir { get; set; }
}