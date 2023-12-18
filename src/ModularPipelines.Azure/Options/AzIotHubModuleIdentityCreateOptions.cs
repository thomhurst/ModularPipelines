using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "module-identity", "create")]
public record AzIotHubModuleIdentityCreateOptions(
[property: CommandSwitch("--device-id")] string DeviceId,
[property: CommandSwitch("--module-id")] string ModuleId
) : AzOptions
{
    [CommandSwitch("--am")]
    public string? Am { get; set; }

    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--od")]
    public string? Od { get; set; }

    [CommandSwitch("--pk")]
    public string? Pk { get; set; }

    [CommandSwitch("--primary-thumbprint")]
    public string? PrimaryThumbprint { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--secondary-key")]
    public string? SecondaryKey { get; set; }

    [CommandSwitch("--secondary-thumbprint")]
    public string? SecondaryThumbprint { get; set; }

    [CommandSwitch("--valid-days")]
    public string? ValidDays { get; set; }
}

