using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("desktopvirtualization", "hostpool", "update")]
public record AzDesktopvirtualizationHostpoolUpdateOptions : AzOptions
{
    [CliOption("--custom-rdp-property")]
    public string? CustomRdpProperty { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--friendly-name")]
    public string? FriendlyName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--load-balancer-type")]
    public string? LoadBalancerType { get; set; }

    [CliOption("--max-session-limit")]
    public string? MaxSessionLimit { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--personal-desktop-assignment-type")]
    public string? PersonalDesktopAssignmentType { get; set; }

    [CliOption("--preferred-app-group-type")]
    public string? PreferredAppGroupType { get; set; }

    [CliOption("--registration-info")]
    public string? RegistrationInfo { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--ring")]
    public string? Ring { get; set; }

    [CliOption("--sso-client-id")]
    public string? SsoClientId { get; set; }

    [CliOption("--sso-client-secret-key-vault-path")]
    public string? SsoClientSecretKeyVaultPath { get; set; }

    [CliOption("--sso-secret-type")]
    public string? SsoSecretType { get; set; }

    [CliOption("--ssoadfs-authority")]
    public string? SsoadfsAuthority { get; set; }

    [CliFlag("--start-vm-on-connect")]
    public bool? StartVmOnConnect { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--validation-environment")]
    public bool? ValidationEnvironment { get; set; }

    [CliOption("--vm-template")]
    public string? VmTemplate { get; set; }
}