using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

