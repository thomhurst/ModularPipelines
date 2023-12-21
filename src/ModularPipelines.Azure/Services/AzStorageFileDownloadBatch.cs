using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "file")]
public class AzStorageFileDownloadBatch
{
    public AzStorageFileDownloadBatch(
        AzStorageFileDownloadBatchStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageFileDownloadBatchStoragePreview StoragePreview { get; }
}