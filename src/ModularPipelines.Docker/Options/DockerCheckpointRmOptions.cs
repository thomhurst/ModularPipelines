using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("checkpoint rm")]
[ExcludeFromCodeCoverage]
public record DockerCheckpointRmOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Container, [property: PositionalArgument(Position = Position.AfterSwitches)] string Checkpoint) : DockerOptions
{
    [CommandSwitch("--checkpoint-dir")]
    public string? CheckpointDir { get; set; }
}
