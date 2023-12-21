using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "file")]
public class AzStorageFileUrl
{
    public AzStorageFileUrl(
        AzStorageFileUrlStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageFileUrlStoragePreview StoragePreview { get; }
}