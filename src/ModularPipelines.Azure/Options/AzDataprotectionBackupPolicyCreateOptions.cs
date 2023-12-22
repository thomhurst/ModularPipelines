using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-policy", "create")]
public record AzDataprotectionBackupPolicyCreateOptions(
[property: CommandSwitch("--backup-policy-name")] string BackupPolicyName,
[property: CommandSwitch("--policy")] string Policy,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions;