using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-groups", "managed", "wait-until")]
public record GcloudComputeInstanceGroupsManagedWaitUntilOptions(
[property: CliArgument] string Name,
[property: CliFlag("--stable")] bool Stable,
[property: CliFlag("--version-target-reached")] bool VersionTargetReached
) : GcloudOptions
{
    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}