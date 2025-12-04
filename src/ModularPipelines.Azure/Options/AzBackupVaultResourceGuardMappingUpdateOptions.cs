using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("backup", "vault", "resource-guard-mapping", "update")]
public record AzBackupVaultResourceGuardMappingUpdateOptions(
[property: CliOption("--resource-guard-id")] string ResourceGuardId
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tenant-id")]
    public string? TenantId { get; set; }
}