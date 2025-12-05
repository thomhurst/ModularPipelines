using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "account")]
public class AzStorageAccountCreate
{
    public AzStorageAccountCreate(
        AzStorageAccountCreateStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageAccountCreateStoragePreview StoragePreview { get; }
}