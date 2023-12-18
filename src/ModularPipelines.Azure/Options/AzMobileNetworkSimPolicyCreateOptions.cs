using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile-network", "sim", "policy", "create")]
public record AzMobileNetworkSimPolicyCreateOptions(
[property: CommandSwitch("--default-slice")] string DefaultSlice,
[property: CommandSwitch("--mobile-network-name")] string MobileNetworkName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--slice-config")] string SliceConfig,
[property: CommandSwitch("--ue-ambr")] string UeAmbr
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--registration-timer")]
    public string? RegistrationTimer { get; set; }

    [CommandSwitch("--rfsp-index")]
    public string? RfspIndex { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}