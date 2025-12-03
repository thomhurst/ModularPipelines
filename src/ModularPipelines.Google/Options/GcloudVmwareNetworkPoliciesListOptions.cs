using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "network-policies", "list")]
public record GcloudVmwareNetworkPoliciesListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}