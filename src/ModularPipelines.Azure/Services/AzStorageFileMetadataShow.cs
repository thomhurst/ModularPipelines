using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "file", "metadata")]
public class AzStorageFileMetadataShow
{
    public AzStorageFileMetadataShow(
        AzStorageFileMetadataShowStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageFileMetadataShowStoragePreview StoragePreview { get; }
}