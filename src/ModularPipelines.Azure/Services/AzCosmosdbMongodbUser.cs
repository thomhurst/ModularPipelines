using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb")]
public class AzCosmosdbMongodbUser
{
    public AzCosmosdbMongodbUser(
        AzCosmosdbMongodbUserDefinition definition
    )
    {
        Definition = definition;
    }

    public AzCosmosdbMongodbUserDefinition Definition { get; }
}