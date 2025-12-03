using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "networks", "list")]
public record GcloudVmwareNetworksListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}