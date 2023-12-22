using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "blob", "service-properties")]
public class AzStorageBlobServicePropertiesUpdate
{
    public AzStorageBlobServicePropertiesUpdate(
        AzStorageBlobServicePropertiesUpdateStorageBlobPreview storageBlobPreview
    )
    {
        StorageBlobPreview = storageBlobPreview;
    }

    public AzStorageBlobServicePropertiesUpdateStorageBlobPreview StorageBlobPreview { get; }
}