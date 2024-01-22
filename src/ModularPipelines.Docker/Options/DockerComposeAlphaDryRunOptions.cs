using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "alpha", "dry-run")]
[ExcludeFromCodeCoverage]
public record DockerComposeAlphaDryRunOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Command { get; set; }
}
