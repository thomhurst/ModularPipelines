using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb")]
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