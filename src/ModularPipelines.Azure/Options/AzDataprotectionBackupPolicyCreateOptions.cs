using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dataprotection", "backup-policy", "create")]
public record AzDataprotectionBackupPolicyCreateOptions(
[property: CliOption("--backup-policy-name")] string BackupPolicyName,
[property: CliOption("--policy")] string Policy,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions;