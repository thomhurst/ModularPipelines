using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("manifest")]
[ExcludeFromCodeCoverage]
public record DockerManifestOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Command) : DockerOptions;