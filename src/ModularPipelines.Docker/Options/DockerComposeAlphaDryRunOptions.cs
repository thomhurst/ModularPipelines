using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "alpha", "dry-run")]
[ExcludeFromCodeCoverage]
public record DockerComposeAlphaDryRunOptions : DockerOptions
{
}
