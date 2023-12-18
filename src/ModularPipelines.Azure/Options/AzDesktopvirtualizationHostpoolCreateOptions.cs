using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("desktopvirtualization", "hostpool", "create")]
public record AzDesktopvirtualizationHostpoolCreateOptions(
[property: CommandSwitch("--host-pool-type")] string HostPoolType,
[property: CommandSwitch("--load-balancer-type")] string LoadBalancerType,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--preferred-app-group-type")] string PreferredAppGroupType,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--custom-rdp-property")]
    public string? CustomRdpProperty { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--friendly-name")]
    public string? FriendlyName { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--max-session-limit")]
    public string? MaxSessionLimit { get; set; }

    [CommandSwitch("--personal-desktop-assignment-type")]
    public string? PersonalDesktopAssignmentType { get; set; }

    [CommandSwitch("--registration-info")]
    public string? RegistrationInfo { get; set; }

    [CommandSwitch("--ring")]
    public string? Ring { get; set; }

    [CommandSwitch("--sso-client-id")]
    public string? SsoClientId { get; set; }

    [CommandSwitch("--sso-client-secret-key-vault-path")]
    public string? SsoClientSecretKeyVaultPath { get; set; }

    [CommandSwitch("--sso-secret-type")]
    public string? SsoSecretType { get; set; }

    [CommandSwitch("--ssoadfs-authority")]
    public string? SsoadfsAuthority { get; set; }

    [BooleanCommandSwitch("--start-vm-on-connect")]
    public bool? StartVmOnConnect { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [BooleanCommandSwitch("--validation-environment")]
    public bool? ValidationEnvironment { get; set; }

    [CommandSwitch("--vm-template")]
    public string? VmTemplate { get; set; }
}