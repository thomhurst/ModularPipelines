using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dataprotection", "backup-instance", "create")]
public record AzDataprotectionBackupInstanceCreateOptions(
[property: CliOption("--backup-instance")] string BackupInstance,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}