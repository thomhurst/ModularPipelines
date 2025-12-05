using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "file")]
public class AzStorageFileDeleteBatch
{
    public AzStorageFileDeleteBatch(
        AzStorageFileDeleteBatchStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageFileDeleteBatchStoragePreview StoragePreview { get; }
}