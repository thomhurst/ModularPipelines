using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("secret")]
[ExcludeFromCodeCoverage]
public record DockerSecretOptions : DockerOptions
{
}
