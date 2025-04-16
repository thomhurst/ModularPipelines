using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx", "imagetools", "create")]
[ExcludeFromCodeCoverage]
public record DockerBuildxImagetoolsCreateOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Source { get; set; }

    [CommandSwitch("--annotation")]
    public virtual string? Annotation { get; set; }

    [CommandSwitch("--append")]
    public virtual string? Append { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CommandSwitch("--file")]
    public virtual string? File { get; set; }

    [CommandSwitch("--progress")]
    public virtual string? Progress { get; set; }

    [CommandSwitch("--tag")]
    public virtual string? Tag { get; set; }
}
