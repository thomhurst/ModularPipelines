using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context", "import")]
[ExcludeFromCodeCoverage]
public record DockerContextImportOptions : DockerOptions
{
}
