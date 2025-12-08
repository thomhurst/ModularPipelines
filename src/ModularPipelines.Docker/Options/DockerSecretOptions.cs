using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliSubCommand("secret")]
[ExcludeFromCodeCoverage]
public record DockerSecretOptions : DockerOptions
{
}
