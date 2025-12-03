using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("desktopvirtualization", "hostpool", "create")]
public record AzDesktopvirtualizationHostpoolCreateOptions(
[property: CliOption("--host-pool-type")] string HostPoolType,
[property: CliOption("--load-balancer-type")] string LoadBalancerType,
[property: CliOption("--name")] string Name,
[property: CliOption("--preferred-app-group-type")] string PreferredAppGroupType,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--custom-rdp-property")]
    public string? CustomRdpProperty { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--friendly-name")]
    public string? FriendlyName { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--max-session-limit")]
    public string? MaxSessionLimit { get; set; }

    [CliOption("--personal-desktop-assignment-type")]
    public string? PersonalDesktopAssignmentType { get; set; }

    [CliOption("--registration-info")]
    public string? RegistrationInfo { get; set; }

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

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--validation-environment")]
    public bool? ValidationEnvironment { get; set; }

    [CliOption("--vm-template")]
    public string? VmTemplate { get; set; }
}