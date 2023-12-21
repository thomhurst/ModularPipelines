using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "table", "restorable-resource")]
public class AzCosmosdbTableRestorableResourceList
{
    public AzCosmosdbTableRestorableResourceList(
        AzCosmosdbTableRestorableResourceListCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbTableRestorableResourceListCosmosdbPreview CosmosdbPreview { get; }
}