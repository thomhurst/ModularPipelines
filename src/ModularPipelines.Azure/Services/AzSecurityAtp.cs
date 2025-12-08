using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("security")]
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