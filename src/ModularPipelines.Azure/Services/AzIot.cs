using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzIot
{
    public AzIot(
        AzIotCentral central,
        AzIotDevice device,
        AzIotDps dps,
        AzIotDu du,
        AzIotEdge edge,
        AzIotHub hub,
        AzIotOps ops,
        AzIotProduct product
    )
    {
        Central = central;
        Device = device;
        Dps = dps;
        Du = du;
        Edge = edge;
        Hub = hub;
        Ops = ops;
        Product = product;
    }

    public AzIotCentral Central { get; }

    public AzIotDevice Device { get; }

    public AzIotDps Dps { get; }

    public AzIotDu Du { get; }

    public AzIotEdge Edge { get; }

    public AzIotHub Hub { get; }

    public AzIotOps Ops { get; }

    public AzIotProduct Product { get; }
}