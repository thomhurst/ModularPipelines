using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx", "du")]
[ExcludeFromCodeCoverage]
public record DockerBuildxDuOptions : DockerOptions
{
    [CommandSwitch("--filter")]
    public virtual string? Filter { get; set; }

    [CommandSwitch("--verbose")]
    public virtual string? Verbose { get; set; }
}
