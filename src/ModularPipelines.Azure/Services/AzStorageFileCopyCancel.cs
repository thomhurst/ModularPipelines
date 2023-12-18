using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "file", "copy")]
public class AzStorageFileCopyCancel
{
    public AzStorageFileCopyCancel(
        AzStorageFileCopyCancelStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageFileCopyCancelStoragePreview StoragePreview { get; }
}