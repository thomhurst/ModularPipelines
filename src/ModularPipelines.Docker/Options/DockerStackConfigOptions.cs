using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("stack", "config")]
[ExcludeFromCodeCoverage]
public record DockerStackConfigOptions : DockerOptions
{
    [CliOption("--compose-file")]
    public virtual string? ComposeFile { get; set; }

    [CliOption("--skip-interpolation")]
    public virtual string? SkipInterpolation { get; set; }
}
