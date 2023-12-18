using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "update")]
public record AzKeyvaultUpdateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

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

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--retention-days")]
    public string? RetentionDays { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }
}

