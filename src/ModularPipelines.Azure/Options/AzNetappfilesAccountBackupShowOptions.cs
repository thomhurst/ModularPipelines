using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("netappfiles", "account", "backup", "show")]
public record AzNetappfilesAccountBackupShowOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--backup-name")] string BackupName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;