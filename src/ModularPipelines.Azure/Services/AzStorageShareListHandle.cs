using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "share")]
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