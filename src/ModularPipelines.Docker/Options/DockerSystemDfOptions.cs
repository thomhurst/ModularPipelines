using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("system df")]
[ExcludeFromCodeCoverage]
public record DockerSystemDfOptions : DockerOptions
{
    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--verbose")]
    public string? Verbose { get; set; }
}
