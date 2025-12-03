using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "module-identity", "renew-key")]
public record AzIotHubModuleIdentityRenewKeyOptions(
[property: CliOption("--device-id")] string DeviceId,
[property: CliOption("--hub-name")] string HubName,
[property: CliOption("--key-type")] string KeyType,
[property: CliOption("--module-id")] string ModuleId
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