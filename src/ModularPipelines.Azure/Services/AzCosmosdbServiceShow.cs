using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "service")]
public class AzCosmosdbServiceShow
{
    public AzCosmosdbServiceShow(
        AzCosmosdbServiceShowCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbServiceShowCosmosdbPreview CosmosdbPreview { get; }
}