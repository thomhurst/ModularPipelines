using System.Diagnostics.CodeAnalysis;

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