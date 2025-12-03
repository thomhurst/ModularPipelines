using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "disks", "remove-resource-policies")]
public record GcloudComputeDisksRemoveResourcePoliciesOptions(
[property: CliArgument] string DiskName,
[property: CliOption("--resource-policies")] string[] ResourcePolicies
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}