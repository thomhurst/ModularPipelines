using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security")]
public class AzSecurityAtp
{
    public AzSecurityAtp(
        AzSecurityAtpCosmosdb cosmosdb,
        AzSecurityAtpStorage storage
    )
    {
        Cosmosdb = cosmosdb;
        Storage = storage;
    }

    public AzSecurityAtpCosmosdb Cosmosdb { get; }

    public AzSecurityAtpStorage Storage { get; }
}