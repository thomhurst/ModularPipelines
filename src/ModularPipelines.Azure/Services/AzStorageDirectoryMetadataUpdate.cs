using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "directory", "metadata")]
public class AzStorageDirectoryMetadataUpdate
{
    public AzStorageDirectoryMetadataUpdate(
        AzStorageDirectoryMetadataUpdateStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageDirectoryMetadataUpdateStoragePreview StoragePreview { get; }
}