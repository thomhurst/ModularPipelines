using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin")]
[ExcludeFromCodeCoverage]
public record DockerPluginOptions : DockerOptions
{
}
