using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "device", "attestation", "create")]
public record AzIotCentralDeviceAttestationCreateOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--content")] string Content,
[property: CliOption("--device-id")] string DeviceId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}