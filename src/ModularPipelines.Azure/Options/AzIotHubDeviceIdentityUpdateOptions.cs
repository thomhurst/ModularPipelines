using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "device-identity", "update")]
public record AzIotHubDeviceIdentityUpdateOptions(
[property: CliOption("--device-id")] string DeviceId
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--am")]
    public string? Am { get; set; }

    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliFlag("--edge-enabled")]
    public bool? EdgeEnabled { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--pk")]
    public string? Pk { get; set; }

    [CliOption("--primary-thumbprint")]
    public string? PrimaryThumbprint { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--secondary-key")]
    public string? SecondaryKey { get; set; }

    [CliOption("--secondary-thumbprint")]
    public string? SecondaryThumbprint { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--sta")]
    public string? Sta { get; set; }

    [CliOption("--star")]
    public string? Star { get; set; }
}