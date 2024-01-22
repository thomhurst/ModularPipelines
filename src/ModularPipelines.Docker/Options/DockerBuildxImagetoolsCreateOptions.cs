using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx", "imagetools", "create")]
[ExcludeFromCodeCoverage]
public record DockerBuildxImagetoolsCreateOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Source { get; set; }

    [CommandSwitch("--annotation")]
    public string? Annotation { get; set; }

    [CommandSwitch("--append")]
    public string? Append { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [CommandSwitch("--file")]
    public string? File { get; set; }

    [CommandSwitch("--progress")]
    public string? Progress { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }
}
