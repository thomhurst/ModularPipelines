using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("manifest", "rm")]
[ExcludeFromCodeCoverage]
public record DockerManifestRmOptions : DockerOptions
{
}
