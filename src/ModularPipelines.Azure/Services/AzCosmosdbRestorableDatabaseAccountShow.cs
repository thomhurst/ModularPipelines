using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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