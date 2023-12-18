using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("secret")]
[ExcludeFromCodeCoverage]
public record DockerSecretOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Command) : DockerOptions;