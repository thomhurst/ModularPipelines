using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "vault", "resource-guard-mapping", "update")]
public record AzBackupVaultResourceGuardMappingUpdateOptions(
[property: CommandSwitch("--resource-guard-id")] string ResourceGuardId
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tenant-id")]
    public string? TenantId { get; set; }
}