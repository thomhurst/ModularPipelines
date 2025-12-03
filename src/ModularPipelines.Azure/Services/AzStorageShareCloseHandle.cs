using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "share")]
public class AzStorageShareCloseHandle
{
    public AzStorageShareCloseHandle(
        AzStorageShareCloseHandleStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageShareCloseHandleStoragePreview StoragePreview { get; }
}