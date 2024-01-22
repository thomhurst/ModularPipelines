using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("secret")]
[ExcludeFromCodeCoverage]
public record DockerSecretOptions : DockerOptions
{
}
