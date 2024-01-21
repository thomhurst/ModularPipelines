using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin", "set")]
[ExcludeFromCodeCoverage]
public record DockerPluginSetOptions : DockerOptions
{
}
