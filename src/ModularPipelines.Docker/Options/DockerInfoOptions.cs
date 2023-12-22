using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("info")]
[ExcludeFromCodeCoverage]
public record DockerInfoOptions : DockerOptions
{
    [CommandSwitch("--format")]
    public string? Format { get; set; }
}