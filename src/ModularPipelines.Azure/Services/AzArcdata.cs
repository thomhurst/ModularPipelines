using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzArcdata
{
    public AzArcdata(
        AzArcdataAdConnector adConnector,
        AzArcdataDc dc,
        AzArcdataResourceKind resourceKind
    )
    {
        AdConnector = adConnector;
        Dc = dc;
        ResourceKind = resourceKind;
    }

    public AzArcdataAdConnector AdConnector { get; }

    public AzArcdataDc Dc { get; }

    public AzArcdataResourceKind ResourceKind { get; }
}