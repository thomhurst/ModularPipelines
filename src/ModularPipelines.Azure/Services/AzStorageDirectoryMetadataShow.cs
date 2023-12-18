using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "directory", "metadata")]
public class AzStorageDirectoryMetadataShow
{
    public AzStorageDirectoryMetadataShow(
        AzStorageDirectoryMetadataShowStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageDirectoryMetadataShowStoragePreview StoragePreview { get; }
}