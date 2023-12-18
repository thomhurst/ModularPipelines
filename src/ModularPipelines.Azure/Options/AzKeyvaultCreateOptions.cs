using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "create")]
public record AzKeyvaultCreateOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--administrators")]
    public string? Administrators { get; set; }

    [CommandSwitch("--bypass")]
    public string? Bypass { get; set; }

    [CommandSwitch("--default-action")]
    public string? DefaultAction { get; set; }

    [BooleanCommandSwitch("--enable-purge-protection")]
    public bool? EnablePurgeProtection { get; set; }

    [BooleanCommandSwitch("--enable-rbac-authorization")]
    public bool? EnableRbacAuthorization { get; set; }

    [BooleanCommandSwitch("--enabled-for-deployment")]
    public bool? EnabledForDeployment { get; set; }

    [BooleanCommandSwitch("--enabled-for-disk-encryption")]
    public bool? EnabledForDiskEncryption { get; set; }

    [BooleanCommandSwitch("--enabled-for-template-deployment")]
    public bool? EnabledForTemplateDeployment { get; set; }

    [CommandSwitch("--hsm-name")]
    public string? HsmName { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--network-acls")]
    public string? NetworkAcls { get; set; }

    [CommandSwitch("--network-acls-ips")]
    public string? NetworkAclsIps { get; set; }

    [CommandSwitch("--network-acls-vnets")]
    public string? NetworkAclsVnets { get; set; }

    [BooleanCommandSwitch("--no-self-perms")]
    public bool? NoSelfPerms { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--retention-days")]
    public string? RetentionDays { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}