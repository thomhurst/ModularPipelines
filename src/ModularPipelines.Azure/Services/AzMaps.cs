using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzMaps
{
    public AzMaps(
        AzMapsAccount account,
        AzMapsCreator creator,
        AzMapsMap map
    )
    {
        Account = account;
        Creator = creator;
        Map = map;
    }

    public AzMapsAccount Account { get; }

    public AzMapsCreator Creator { get; }

    public AzMapsMap Map { get; }
}