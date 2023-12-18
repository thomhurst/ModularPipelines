using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "restorable-database-account")]
public class AzCosmosdbRestorableDatabaseAccountShow
{
    public AzCosmosdbRestorableDatabaseAccountShow(
        AzCosmosdbRestorableDatabaseAccountShowCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbRestorableDatabaseAccountShowCosmosdbPreview CosmosdbPreview { get; }
}