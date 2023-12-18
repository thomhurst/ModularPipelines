using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "account", "backup", "show")]
public record AzNetappfilesAccountBackupShowOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--backup-name")] string BackupName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;