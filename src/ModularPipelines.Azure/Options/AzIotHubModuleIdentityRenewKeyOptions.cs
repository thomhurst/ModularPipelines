using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "module-identity", "renew-key")]
public record AzIotHubModuleIdentityRenewKeyOptions(
[property: CommandSwitch("--device-id")] string DeviceId,
[property: CommandSwitch("--hub-name")] string HubName,
[property: CommandSwitch("--key-type")] string KeyType,
[property: CommandSwitch("--module-id")] string ModuleId
) : AzOptions
{
    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

