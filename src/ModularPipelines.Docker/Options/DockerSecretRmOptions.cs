using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("secret rm")]
[ExcludeFromCodeCoverage]
public record DockerSecretRmOptions([property: PositionalArgument(Position = Position.AfterSwitches)] IEnumerable<string> Secret) : DockerOptions;