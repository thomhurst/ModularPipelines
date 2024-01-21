using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust", "signer")]
[ExcludeFromCodeCoverage]
public record DockerTrustSignerOptions : DockerOptions
{
}
