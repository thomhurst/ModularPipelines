using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "service")]
public class AzCosmosdbServiceCreate
{
    public AzCosmosdbServiceCreate(
        AzCosmosdbServiceCreateCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbServiceCreateCosmosdbPreview CosmosdbPreview { get; }
}