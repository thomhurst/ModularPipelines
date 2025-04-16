using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("system", "df")]
[ExcludeFromCodeCoverage]
public record DockerSystemDfOptions : DockerOptions
{
    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }

    [CommandSwitch("--verbose")]
    public virtual string? Verbose { get; set; }
}
