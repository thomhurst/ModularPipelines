using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin")]
[ExcludeFromCodeCoverage]
public record DockerPluginOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Command) : DockerOptions
{
}
