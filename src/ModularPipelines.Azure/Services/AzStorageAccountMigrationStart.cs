using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account", "migration")]
public class AzStorageAccountMigrationStart
{
    public AzStorageAccountMigrationStart(
        AzStorageAccountMigrationStartStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageAccountMigrationStartStoragePreview StoragePreview { get; }
}