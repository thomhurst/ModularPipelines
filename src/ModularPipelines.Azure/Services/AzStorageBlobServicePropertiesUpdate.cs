using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "blob", "service-properties")]
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