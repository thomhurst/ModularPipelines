using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "remove-resource-policies")]
public record GcloudComputeInstancesRemoveResourcePoliciesOptions(
[property: CliArgument] string InstanceName,
[property: CliOption("--resource-policies")] string[] ResourcePolicies
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}