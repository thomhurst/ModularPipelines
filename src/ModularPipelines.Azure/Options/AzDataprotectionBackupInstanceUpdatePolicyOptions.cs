using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-instance", "update-policy")]
public record AzDataprotectionBackupInstanceUpdatePolicyOptions(
[property: CommandSwitch("--backup-instance-name")] string BackupInstanceName,
[property: CommandSwitch("--policy-id")] string PolicyId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}