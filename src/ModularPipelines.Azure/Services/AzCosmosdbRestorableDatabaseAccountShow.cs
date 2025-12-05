using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "restorable-database-account")]
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