using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dataprotection", "recovery-point", "show")]
public record AzDataprotectionRecoveryPointShowOptions : AzOptions
{
    [CliOption("--backup-instance-name")]
    public string? BackupInstanceName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--recovery-point-id")]
    public string? RecoveryPointId { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }
}