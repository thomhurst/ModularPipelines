using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "private-clouds", "dns-forwarding", "describe")]
public record GcloudVmwarePrivateCloudsDnsForwardingDescribeOptions(
[property: CommandSwitch("--private-cloud")] string PrivateCloud,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;