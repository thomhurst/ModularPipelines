using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "service")]
public class AzCosmosdbServiceUpdate
{
    public AzCosmosdbServiceUpdate(
        AzCosmosdbServiceUpdateCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbServiceUpdateCosmosdbPreview CosmosdbPreview { get; }
}