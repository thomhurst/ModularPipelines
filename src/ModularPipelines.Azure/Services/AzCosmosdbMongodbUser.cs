using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

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