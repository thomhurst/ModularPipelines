using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb")]
public class AzCosmosdbUpdate
{
    public AzCosmosdbUpdate(
        AzCosmosdbUpdateCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbUpdateCosmosdbPreview CosmosdbPreview { get; }
}