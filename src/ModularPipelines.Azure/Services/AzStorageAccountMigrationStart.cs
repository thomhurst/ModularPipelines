using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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