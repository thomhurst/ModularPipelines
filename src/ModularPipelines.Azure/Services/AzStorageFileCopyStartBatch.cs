using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "file", "copy")]
public class AzStorageFileCopyStartBatch
{
    public AzStorageFileCopyStartBatch(
        AzStorageFileCopyStartBatchStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageFileCopyStartBatchStoragePreview StoragePreview { get; }
}