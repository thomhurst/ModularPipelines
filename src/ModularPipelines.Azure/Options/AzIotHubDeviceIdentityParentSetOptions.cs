using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "device-identity", "parent", "set")]
public record AzIotHubDeviceIdentityParentSetOptions(
[property: CliOption("--device-id")] string DeviceId,
[property: CliOption("--parent-device-id")] string ParentDeviceId
) : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}