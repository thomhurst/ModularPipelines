using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("system", "events")]
[ExcludeFromCodeCoverage]
public record DockerSystemEventsOptions : DockerOptions
{
    [CommandSwitch("--filter")]
    public virtual string? Filter { get; set; }

    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }

    [CommandSwitch("--since")]
    public virtual string? Since { get; set; }

    [CommandSwitch("--until")]
    public virtual string? Until { get; set; }
}
