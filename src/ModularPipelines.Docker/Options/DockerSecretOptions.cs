using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("secret")]
[ExcludeFromCodeCoverage]
public record DockerSecretOptions : DockerOptions
{
}
