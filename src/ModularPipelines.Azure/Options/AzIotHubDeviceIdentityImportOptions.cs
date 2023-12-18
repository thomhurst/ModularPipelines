using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "device-identity", "import")]
public record AzIotHubDeviceIdentityImportOptions(
[property: CommandSwitch("--device-id")] string DeviceId,
[property: CommandSwitch("--hub-name")] string HubName,
[property: CommandSwitch("--key-type")] string KeyType
) : AzOptions
{
    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--ibc")]
    public string? Ibc { get; set; }

    [CommandSwitch("--ibcu")]
    public string? Ibcu { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--input-storage-account")]
    public int? InputStorageAccount { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--obc")]
    public string? Obc { get; set; }

    [CommandSwitch("--obcu")]
    public string? Obcu { get; set; }

    [CommandSwitch("--osa")]
    public string? Osa { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}