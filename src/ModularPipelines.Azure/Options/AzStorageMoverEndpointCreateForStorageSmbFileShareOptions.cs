using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage-mover", "endpoint", "create-for-storage-smb-file-share")]
public record AzStorageMoverEndpointCreateForStorageSmbFileShareOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--file-share-name")] string FileShareName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-account-id")] int StorageAccountId,
[property: CliOption("--storage-mover-name")] string StorageMoverName
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }
}