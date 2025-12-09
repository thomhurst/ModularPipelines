using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dataprotection", "backup-instance", "update-policy")]
public record AzDataprotectionBackupInstanceUpdatePolicyOptions(
[property: CliOption("--backup-instance-name")] string BackupInstanceName,
[property: CliOption("--policy-id")] string PolicyId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}