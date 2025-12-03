using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "account")]
public class AzStorageAccountUpdate
{
    public AzStorageAccountUpdate(
        AzStorageAccountUpdateStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageAccountUpdateStoragePreview StoragePreview { get; }
}