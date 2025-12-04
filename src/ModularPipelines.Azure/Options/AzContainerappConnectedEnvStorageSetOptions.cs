using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "connected-env", "storage", "set")]
public record AzContainerappConnectedEnvStorageSetOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-name")] string StorageName
) : AzOptions
{
    [CliOption("--access-mode")]
    public string? AccessMode { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--azure-file-account-key")]
    public int? AzureFileAccountKey { get; set; }

    [CliOption("--azure-file-share-name")]
    public string? AzureFileShareName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}