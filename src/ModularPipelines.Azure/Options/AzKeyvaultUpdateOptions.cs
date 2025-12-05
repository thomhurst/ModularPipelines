using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "update")]
public record AzKeyvaultUpdateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

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

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--retention-days")]
    public string? RetentionDays { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }
}