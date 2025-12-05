using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("context", "import")]
[ExcludeFromCodeCoverage]
public record DockerContextImportOptions : DockerOptions
{
}
