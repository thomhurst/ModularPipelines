using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("manifest")]
[ExcludeFromCodeCoverage]
public record DockerManifestOptions : DockerOptions
{
}
