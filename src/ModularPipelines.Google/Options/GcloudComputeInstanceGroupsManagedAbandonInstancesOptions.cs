using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-groups", "managed", "abandon-instances")]
public record GcloudComputeInstanceGroupsManagedAbandonInstancesOptions(
[property: CliArgument] string Name,
[property: CliOption("--instances")] string[] Instances
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}