using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "device-identity", "renew-key")]
public record AzIotHubDeviceIdentityRenewKeyOptions(
[property: CliOption("--device-id")] string DeviceId,
[property: CliOption("--hub-name")] string HubName,
[property: CliOption("--key-type")] string KeyType
) : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}