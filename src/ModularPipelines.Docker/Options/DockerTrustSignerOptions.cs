using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust signer")]
[ExcludeFromCodeCoverage]
public record DockerTrustSignerOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Command) : DockerOptions;