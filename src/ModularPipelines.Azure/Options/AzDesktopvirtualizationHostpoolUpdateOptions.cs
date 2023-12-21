using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("desktopvirtualization", "hostpool", "update")]
public record AzDesktopvirtualizationHostpoolUpdateOptions : AzOptions
{
    [CommandSwitch("--custom-rdp-property")]
    public string? CustomRdpProperty { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--friendly-name")]
    public string? FriendlyName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--load-balancer-type")]
    public string? LoadBalancerType { get; set; }

    [CommandSwitch("--max-session-limit")]
    public string? MaxSessionLimit { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--personal-desktop-assignment-type")]
    public string? PersonalDesktopAssignmentType { get; set; }

    [CommandSwitch("--preferred-app-group-type")]
    public string? PreferredAppGroupType { get; set; }

    [CommandSwitch("--registration-info")]
    public string? RegistrationInfo { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

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

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [BooleanCommandSwitch("--validation-environment")]
    public bool? ValidationEnvironment { get; set; }

    [CommandSwitch("--vm-template")]
    public string? VmTemplate { get; set; }
}