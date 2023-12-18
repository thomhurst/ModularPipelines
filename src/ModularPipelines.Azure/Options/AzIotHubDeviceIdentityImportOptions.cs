using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "device-identity", "import")]
public record AzIotHubDeviceIdentityImportOptions : AzOptions
{
    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

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