using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "digital-twin", "update")]
public record AzIotHubDigitalTwinUpdateOptions(
[property: CliOption("--device-id")] string DeviceId,
[property: CliOption("--json-patch")] string JsonPatch
) : AzOptions
{
    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}