using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile-network", "sim", "create")]
public record AzMobileNetworkSimCreateOptions(
[property: CommandSwitch("--international-msi")] string InternationalMsi,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sim-group-name")] string SimGroupName
) : AzOptions
{
    [CommandSwitch("--authentication-key")]
    public string? AuthenticationKey { get; set; }

    [CommandSwitch("--device-type")]
    public string? DeviceType { get; set; }

    [CommandSwitch("--icc-id")]
    public string? IccId { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--operator-key-code")]
    public string? OperatorKeyCode { get; set; }

    [CommandSwitch("--sim-policy")]
    public string? SimPolicy { get; set; }

    [CommandSwitch("--static-ip-config")]
    public string? StaticIpConfig { get; set; }
}