using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "restorable-database-account")]
public class AzCosmosdbRestorableDatabaseAccountList
{
    public AzCosmosdbRestorableDatabaseAccountList(
        AzCosmosdbRestorableDatabaseAccountListCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbRestorableDatabaseAccountListCosmosdbPreview CosmosdbPreview { get; }
}