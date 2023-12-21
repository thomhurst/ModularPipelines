using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "file")]
public class AzStorageFileGenerateSas
{
    public AzStorageFileGenerateSas(
        AzStorageFileGenerateSasStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageFileGenerateSasStoragePreview StoragePreview { get; }
}