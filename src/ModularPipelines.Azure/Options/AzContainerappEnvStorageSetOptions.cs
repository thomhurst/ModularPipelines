using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "env", "storage", "set")]
public record AzContainerappEnvStorageSetOptions(
[property: CliOption("--access-mode")] string AccessMode,
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--azure-file-account-key")] int AzureFileAccountKey,
[property: CliOption("--azure-file-share-name")] string AzureFileShareName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-name")] string StorageName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}