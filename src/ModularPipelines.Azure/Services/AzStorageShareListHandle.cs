using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "share")]
public class AzStorageShareListHandle
{
    public AzStorageShareListHandle(
        AzStorageShareListHandleStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageShareListHandleStoragePreview StoragePreview { get; }
}

