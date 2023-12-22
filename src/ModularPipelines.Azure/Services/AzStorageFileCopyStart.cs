using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "file", "copy")]
public class AzStorageFileCopyStart
{
    public AzStorageFileCopyStart(
        AzStorageFileCopyStartStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageFileCopyStartStoragePreview StoragePreview { get; }
}