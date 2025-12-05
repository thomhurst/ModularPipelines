using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "create")]
public record AzKeyvaultCreateOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--administrators")]
    public string? Administrators { get; set; }

    [CliOption("--bypass")]
    public string? Bypass { get; set; }

    [CliOption("--default-action")]
    public string? DefaultAction { get; set; }

    [CliFlag("--enable-purge-protection")]
    public bool? EnablePurgeProtection { get; set; }

    [CliFlag("--enable-rbac-authorization")]
    public bool? EnableRbacAuthorization { get; set; }

    [CliFlag("--enabled-for-deployment")]
    public bool? EnabledForDeployment { get; set; }

    [CliFlag("--enabled-for-disk-encryption")]
    public bool? EnabledForDiskEncryption { get; set; }

    [CliFlag("--enabled-for-template-deployment")]
    public bool? EnabledForTemplateDeployment { get; set; }

    [CliOption("--hsm-name")]
    public string? HsmName { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--network-acls")]
    public string? NetworkAcls { get; set; }

    [CliOption("--network-acls-ips")]
    public string? NetworkAclsIps { get; set; }

    [CliOption("--network-acls-vnets")]
    public string? NetworkAclsVnets { get; set; }

    [CliFlag("--no-self-perms")]
    public bool? NoSelfPerms { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--retention-days")]
    public string? RetentionDays { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}