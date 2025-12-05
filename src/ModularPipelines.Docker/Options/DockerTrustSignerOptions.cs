using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("trust", "signer")]
[ExcludeFromCodeCoverage]
public record DockerTrustSignerOptions : DockerOptions
{
}
