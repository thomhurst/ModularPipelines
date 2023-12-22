using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("logout")]
[ExcludeFromCodeCoverage]
public record DockerLogoutOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Server { get; set; }
}