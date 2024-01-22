using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("logout")]
[ExcludeFromCodeCoverage]
public record DockerLogoutOptions : DockerOptions
{
}
