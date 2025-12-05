using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "dns-forwarding", "update")]
public record GcloudVmwarePrivateCloudsDnsForwardingUpdateOptions(
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--location")] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--rule")]
    public string[]? Rule { get; set; }
}