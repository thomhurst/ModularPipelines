using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-groups", "managed", "instance-configs", "delete")]
public record GcloudComputeInstanceGroupsManagedInstanceConfigsDeleteOptions(
[property: CliArgument] string Name,
[property: CliOption("--instances")] string[] Instances
) : GcloudOptions
{
    [CliOption("--instance-update-minimal-action")]
    public string? InstanceUpdateMinimalAction { get; set; }

    [CliFlag("--update-instance")]
    public bool? UpdateInstance { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}