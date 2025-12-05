using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apim", "restore")]
public record AzApimRestoreOptions(
[property: CliOption("--backup-name")] string BackupName,
[property: CliOption("--container-name")] string ContainerName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-account-key")] int StorageAccountKey,
[property: CliOption("--storage-account-name")] int StorageAccountName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}