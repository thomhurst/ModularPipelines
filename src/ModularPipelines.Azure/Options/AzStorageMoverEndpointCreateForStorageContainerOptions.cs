using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage-mover", "endpoint", "create-for-storage-container")]
public record AzStorageMoverEndpointCreateForStorageContainerOptions(
[property: CliOption("--container-name")] string ContainerName,
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-account-id")] int StorageAccountId,
[property: CliOption("--storage-mover-name")] string StorageMoverName
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }
}