using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context", "import")]
[ExcludeFromCodeCoverage]
public record DockerContextImportOptions : DockerOptions
{
}
