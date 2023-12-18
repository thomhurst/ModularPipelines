using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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