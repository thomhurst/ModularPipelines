using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("logout")]
[ExcludeFromCodeCoverage]
public record DockerLogoutOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string? Server { get; set; }
}
