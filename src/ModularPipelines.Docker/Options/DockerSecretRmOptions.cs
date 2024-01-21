using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("secret", "rm")]
[ExcludeFromCodeCoverage]
public record DockerSecretRmOptions : DockerOptions
{
}
