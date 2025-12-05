using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb")]
public class AzCosmosdbShow
{
    public AzCosmosdbShow(
        AzCosmosdbShowCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbShowCosmosdbPreview CosmosdbPreview { get; }
}