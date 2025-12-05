using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "file", "metadata")]
public class AzStorageFileMetadataUpdate
{
    public AzStorageFileMetadataUpdate(
        AzStorageFileMetadataUpdateStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageFileMetadataUpdateStoragePreview StoragePreview { get; }
}