using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-clouds", "dns-forwarding", "describe")]
public record GcloudVmwarePrivateCloudsDnsForwardingDescribeOptions(
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--location")] string Location
) : GcloudOptions;