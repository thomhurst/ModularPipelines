using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "blob", "copy")]
public class AzStorageBlobCopyStart
{
    public AzStorageBlobCopyStart(
        AzStorageBlobCopyStartStorageBlobPreview storageBlobPreview
    )
    {
        StorageBlobPreview = storageBlobPreview;
    }

    public AzStorageBlobCopyStartStorageBlobPreview StorageBlobPreview { get; }
}