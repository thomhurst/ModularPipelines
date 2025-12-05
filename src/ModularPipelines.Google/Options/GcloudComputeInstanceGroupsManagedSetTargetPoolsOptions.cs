using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-groups", "managed", "set-target-pools")]
public record GcloudComputeInstanceGroupsManagedSetTargetPoolsOptions(
[property: CliArgument] string Name,
[property: CliOption("--target-pools")] string[] TargetPools
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}