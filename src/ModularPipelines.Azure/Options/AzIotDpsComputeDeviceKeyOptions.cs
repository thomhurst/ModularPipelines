using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "dps", "compute-device-key")]
public record AzIotDpsComputeDeviceKeyOptions(
[property: CommandSwitch("--registration-id")] string RegistrationId
) : AzOptions
{
    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--dps-name")]
    public string? DpsName { get; set; }

    [CommandSwitch("--eid")]
    public string? Eid { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}