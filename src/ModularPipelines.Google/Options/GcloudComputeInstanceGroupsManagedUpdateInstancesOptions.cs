using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-groups", "managed", "update-instances")]
public record GcloudComputeInstanceGroupsManagedUpdateInstancesOptions(
[property: CliArgument] string Name,
[property: CliFlag("--all-instances")] bool AllInstances,
[property: CliOption("--instances")] string[] Instances
) : GcloudOptions
{
    [CliOption("--minimal-action")]
    public string? MinimalAction { get; set; }

    [CliOption("--most-disruptive-allowed-action")]
    public string? MostDisruptiveAllowedAction { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}