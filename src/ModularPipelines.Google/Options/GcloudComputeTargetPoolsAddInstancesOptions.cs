using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-pools", "add-instances")]
public record GcloudComputeTargetPoolsAddInstancesOptions(
[property: CliArgument] string Name,
[property: CliOption("--instances")] string[] Instances
) : GcloudOptions
{
    [CliOption("--instances-zone")]
    public string? InstancesZone { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}